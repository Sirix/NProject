using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using NProject.BLL;
using NProject.Helpers;
using NProject.Infrastructure;
using NProject.Models;
using NProject.Models.Domain;
using NProject.Models.Infrastructure;
using NProject.Models.ViewModels;
using NProject.ViewModels;

namespace NProject.Controllers
{
    [HandleError]
    public class TaskController : Controller
    {
        [Dependency]
        public TaskService TaskService { get; set; }

        [Dependency]
        public IAccessPoint AccessPoint { get; set; }

        private Project _project;
        /// <summary>
        /// Add task to project which specified by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizedToRoles(UserRole.Manager)]
        public ActionResult AddToProject(int id = 0)
        {
            var result = ValidateAccessToProject(id);
            if (result != null) return result;

            var model = GetTaskFormViewModel(id);
            
            return View(model);
        }
        //[Authorize(Roles="PM")]
        [AuthorizedToRoles(AllowedRoles = UserRole.Manager | UserRole.Programmer)]
        public ActionResult AtProject(int id)
        {
            var tasks = TaskService.GetTasksForProject(id);
            var vm = new TaskListViewModel {Tasks = tasks};

            var project = (new ProjectsRepository()).GetProjectById(id);
            //ValidateAccessToProject(project, "PM", "You're not eligible to view tasks of this project.");

            ViewData["ProjectId"] = id;
            ViewData["ProjectTitle"] = project.Name;
            ViewData["CanCreateTasks"] = SessionStorage.User.Role == UserRole.Manager;
            ViewData["CanExecuteTask"] = SessionStorage.User.Role == UserRole.Programmer;
            return View(vm);
        }


        /// <summary>
        /// Validates currently logged user access to project, which specified by id.
        /// </summary>
        /// <param name="id">Project id</param>
        private ActionResult ValidateAccessToProject(int id)
        {
            var user = AccessPoint.Users.First(u => u.Username == User.Identity.Name);
            _project = AccessPoint.Projects.FirstOrDefault(p => p.Id == id);

            if (_project == null)
            {
                TempData["ErrorMessage"] = "Selected project does not exist.";
                //RedirectToAction("List", "Projects").ExecuteResult(ControllerContext);
                return RedirectToAction("List", "Projects");
            }

            if (!_project.Team.Contains(user))
            {
                TempData["ErrorMessage"] = "You're not eligible to manage tasks of this project.";
                //RedirectToAction("List", "Projects").ExecuteResult(ControllerContext);
                return RedirectToAction("List", "Projects");
            }
            return null;
        }

        /// <summary>
        /// Creates and fills view model to addToProject view.
        /// </summary>
        /// <param name="projectId">Target project id</param>
        /// <param name="modelToRefresh">If specified, view model will be refreshed instead of creating new</param>
        /// <returns></returns>
        private TaskFormViewModel GetTaskFormViewModel(int projectId, TaskFormViewModel modelToRefresh = null)
        {
            var model = modelToRefresh ?? new TaskFormViewModel();
            var project = _project ?? AccessPoint.Projects.First(p => p.Id == projectId);

            model.ProjectId = projectId;
            model.ProjectTitle = project.Name;
            model.Statuses = UIHelper.CreateSelectListFromEnum<ItemStatus>();

                //AccessPoint.ProjectStatuses.Select(
                //    p => new SelectListItem {Text = p.Name, Value = SqlFunctions.StringConvert((double) p.Id)}).
                //    ToList();

            model.Programmers =
                project.Team.Where(u => u.Role == UserRole.Programmer && u.UserState == UserState.Working).Select(
                    u =>
                    new SelectListItem
                        {
                            Text = u.Username,
                            Value = u.Id.ToString(),
                        });

            return model;
        }

        [AuthorizedToRoles(UserRole.Manager)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToProject(TaskFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //if model has no info about target project, we can't refill it, so show error
                if (model.ProjectId == 0)
                {
                    TempData["ErrorMessage"] = "Unable to detect target project.";
                    return RedirectToAction("List", "Projects");
                }
                model = GetTaskFormViewModel(model.ProjectId, model);
                return View(model);
            }
            var result = ValidateAccessToProject(model.ProjectId);
            if (result != null) return result;

            var t = model.Task;
            //_project already selected by ValidateAccessToProject
            t.Project = _project;

            t.Status = AccessPoint.ProjectStatuses.First(s => s.Id == model.StatusId);
            //t.Status = (ItemStatus)model.StatusId;
                
            var responsible = AccessPoint.Users.FirstOrDefault(u => u.Id == model.ResponsibleUserId);
            if (responsible == null)
            {
                TempData["ErrorMessage"] = "Unable to detect target responsible user.";
                model = GetTaskFormViewModel(model.ProjectId, model);
                return View(model);
            }
            t.Responsible = responsible;
            t.CreationDate = DateTime.Now;
            _project.Tasks.Add(t);

            AccessPoint.SaveChanges();
            TempData["InformMessage"] = "Task created.";
            return RedirectToAction("Tasks", "Projects", new {id = t.Project.Id});
        }
        //
        // GET: /Task/Edit/5

        [AuthorizedToRoles(UserRole.Manager)]
        public ActionResult Edit(int id)
        {
            var user = AccessPoint.Users.First(u => u.Username == User.Identity.Name);
            var task = AccessPoint.Tasks.FirstOrDefault(i => i.Id == id);
            if (task == null)
            {
                TempData["ErrorMessage"] = "No task found.";
                return RedirectToAction("List", "Projects");
            }
            //if this is a pm of a project
            if (!task.Project.Team.Contains(user))
            {
                TempData["ErrorMessage"] = "You're not eligible to modify this task.";
                return RedirectToAction("Tasks", "Projects", new {id = task.Project.Id});
            }
            var model = GetTaskFormViewModel(task.Project.Id);
            model.Task = task;
            model.Statuses.First(i => int.Parse(i.Value) == task.Status.Id).Selected = true;
            model.Programmers.First(i => int.Parse(i.Value) == task.Responsible.Id).Selected = true;

            return View(model);
        }

        //
        // POST: /Task/Edit/5

        [HttpPost]
        [AuthorizedToRoles(UserRole.Manager)]
        [ ValidateAntiForgeryToken]
        public ActionResult Edit(TaskFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = AccessPoint.Users.First(u => u.Username == User.Identity.Name);
                var task = AccessPoint.Tasks.First(i => i.Id == model.Task.Id);
                if (task == null)
                {
                    TempData["ErrorMessage"] = "No task found.";
                    return RedirectToAction("List", "Projects");
                }
                //if current user not a pm of the project
                if (!task.Project.Team.Contains(user))
                {
                    TempData["ErrorMessage"] = "You're not eligible to edit this task.";
                    return RedirectToAction("Tasks", "Projects", new { id = task.Project.Id });
                }
                //check for a new responsible user
                var respUser = AccessPoint.Users.FirstOrDefault(i => i.Id == model.ResponsibleUserId);
                if (respUser == null || !task.Project.Team.Contains(respUser))
                {
                    TempData["ErrorMessage"] = "Selected responsible user is not in this project's team.";
                    return RedirectToAction("Tasks", "Projects", new { id = task.Project.Id });
                }

                AccessPoint.ObjectContext.ApplyCurrentValues("Tasks", model.Task);
                task.Responsible = respUser;
                var status = AccessPoint.ProjectStatuses.First(i => i.Id == model.StatusId);
                task.Status = status;
                AccessPoint.SaveChanges();

                TempData["InformMessage"] = "Task has been updated.";
                return RedirectToAction("Tasks", "Projects", new {id = task.Project.Id});
            }
            else
            {
                //if model has no info about task, we can't refill it, so show error
                if (model.Task.Id == 0)
                {
                    TempData["ErrorMessage"] = "Unable to detect target task.";
                    return RedirectToAction("List", "Projects");
                }
                model = GetTaskFormViewModel(model.ProjectId, model);
                model.Statuses.First(i => int.Parse(i.Value) == model.StatusId).Selected = true;
                model.Programmers.First(i => int.Parse(i.Value) == model.ResponsibleUserId).Selected = true;

                return View(model);
            }
        }

        //
        // GET: /Task/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Task/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [AuthorizedToRoles(UserRole.Programmer)]
        public ActionResult Take(int id)
        {
            var user = AccessPoint.Users.First(u => u.Username == User.Identity.Name);
            var task = AccessPoint.Tasks.First(t => t.Id == id);
            //check for user is responsible for this task
            if (task.Responsible.Id != user.Id)
            {
                TempData["ErrorMessage"] = "You're not eligible to take this task";
                return RedirectToAction("Tasks", "Projects", new {id = task.Project.Id});
            }
            //check for already taken task
            if (task.Status.Name == "Developing")
            {
                TempData["ErrorMessage"] = "You already took this task.";
                return RedirectToAction("Tasks", "Projects", new {id = task.Project.Id});
            }
            task.Status = AccessPoint.ProjectStatuses.First(p => p.Name == "Developing");
            AccessPoint.SaveChanges();
            TempData["InformMessage"] = "You have taken this task";
            return RedirectToAction("Tasks", "Projects", new {id = task.Project.Id});
        }

        [AuthorizedToRoles(UserRole.Programmer)]
        public ActionResult Complete(int id)
        {
            CheckConditionsForCompleteTask(id);
            var task = AccessPoint.Tasks.First(t => t.Id == id);

            ViewData["Statuses"] =
               AccessPoint.ProjectStatuses.Select(
                   u => new SelectListItem { Text = u.Name, Value = SqlFunctions.StringConvert((double)u.Id) });
            return View(task);
        }

        [HttpPost]
        [AuthorizedToRoles(UserRole.Programmer)]
        public ActionResult Complete(int id, int statusId, int spentTime = 0)
        {
            CheckConditionsForCompleteTask(id);
            var task = AccessPoint.Tasks.First(t => t.Id == id);
            if (spentTime <= 0)
            {
                TempData["ErrorMessage"] = "Spent time in hours must be greater than zero.";
                ViewData["Statuses"] =
                    AccessPoint.ProjectStatuses.Select(
                        u => new SelectListItem {Text = u.Name, Value = SqlFunctions.StringConvert((double) u.Id)});
                return View(task);
            }
            //task.EstimatedTime = (task.EndDate - task.BeginDate);
            //task.EndDate = DateTime.Now;
            task.SpentTime = spentTime;
            task.Status = AccessPoint.ProjectStatuses.First(p => p.Id == statusId);
            AccessPoint.SaveChanges();

            TempData["InformMessage"] = "Task has been updated.";
            return RedirectToAction("Tasks", "Projects", new {id = task.Project.Id});
        }

        private void CheckConditionsForCompleteTask(int taskId)
        {
            var user = AccessPoint.Users.First(u => u.Username == User.Identity.Name);
            var task = AccessPoint.Tasks.First(t => t.Id == taskId);
            //check for user is responsible for this task
            if (task.Responsible.Id != user.Id)
            {
                TempData["ErrorMessage"] = "You're not eligible to complete this task";
                RedirectToAction("Tasks", "Projects", new {id = task.Project.Id}).ExecuteResult(ControllerContext);
            }
            //check for already finished task
            if (task.Status.Name == "Finished")
            {
                TempData["ErrorMessage"] = "This task already finished";
                RedirectToAction("Tasks", "Projects", new {id = task.Project.Id}).ExecuteResult(ControllerContext);
            }
        }
    }
}
