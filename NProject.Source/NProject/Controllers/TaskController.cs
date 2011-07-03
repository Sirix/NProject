using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using NProject.Models;
using NProject.Models.Domain;
using NProject.Models.Infrastructure;

namespace NProject.Controllers
{
    [HandleError]
    public class TaskController : Controller
    {
        [Dependency]
        public IAccessPoint AccessPoint { get; set; }

        /// <summary>
        /// Add task to project which specified by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles="PM")]
        public ActionResult AddToProject(int id)
        {
            FillViewAddToProject(id);

            return View();
        }

        private void FillViewAddToProject(int id)
        {
            var project = AccessPoint.Projects.First(p => p.Id == id);
            ViewData["ProjectTitle"] = project.Name;

            ViewData["Statuses"] =
                AccessPoint.ProjectStatuses.Select(p => new SelectListItem { Text = p.Name, Value = SqlFunctions.StringConvert((double)p.Id) }).
                    ToList();

            ViewData["Users"] =
                project.Team.Where(u => u.Role.Name == "Programmer" && u.UserState == UserState.Working).Select(
                    u =>
                    new SelectListItem
                    {
                        Text = u.Username,
                        Value = u.Id.ToString(),
                    });
        }

        [Authorize(Roles = "PM")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToProject(int id, int userId, int statusId, Task t)
        {
            if (!ModelState.IsValid)
            {
                FillViewAddToProject(id);
                return View();
            }
            else
            {
                var project = AccessPoint.Projects.First(p => p.Id == id);
                t.Project = project;
                t.Status = AccessPoint.ProjectStatuses.First(s => s.Id == statusId);
                t.Responsible = AccessPoint.Users.First(u => u.Id == userId);
                t.CreationDate = DateTime.Now;
                project.Tasks.Add(t);

                AccessPoint.SaveChanges();
                return RedirectToAction("list", "Projects");
            }
        }
        //
        // GET: /Task/Edit/5
 
        [Authorize(Roles="PM")]
        public ActionResult Edit(int id)
        {
            var task = AccessPoint.Tasks.First(i => i.Id == id);
            var user = AccessPoint.Users.First(u => u.Username == User.Identity.Name);
            //if this is a pm of a project
            if (!task.Project.Team.Contains(user))
            {
                TempData["ErrorMessage"] = "You're not eligible to modify this task.";
                RedirectToAction("Tasks", "Projects", new { id = task.Project.Id }).ExecuteResult(ControllerContext);
            }

            ViewData["Statuses"] =
                AccessPoint.ProjectStatuses.Select(
                    p =>
                    new SelectListItem
                        {
                            Text = p.Name,
                            Value = SqlFunctions.StringConvert((double) p.Id),
                            Selected = task.Status.Name == p.Name
                        }).
                    ToList();

            ViewData["Users"] =
                task.Project.Team.Where(u => u.Role.Name == "Programmer").Select(
                    u =>
                    new SelectListItem
                        {
                            Text = u.Username,
                            Value = u.Id.ToString(),
                            Selected = u.Id == task.Responsible.Id
                        });
            ViewData["ProjectId"] = task.Project.Id;
            return View(task);
        }

        //
        // POST: /Task/Edit/5

        [HttpPost]
        [Authorize(Roles = "PM"), ValidateAntiForgeryToken]
        public ActionResult Edit(Task t, int responsibleId, int statusId)
        {
            var task = AccessPoint.Tasks.First(i => i.Id == t.Id);
            var user = AccessPoint.Users.First(u => u.Username == User.Identity.Name);
            //if this is a pm of a project
            if (!task.Project.Team.Contains(user))
            {
                TempData["ErrorMessage"] = "You're not eligible to modify this task.";
                return RedirectToAction("Tasks", "Projects", new {id = task.Project.Id});
            }
            //check for a new responsible user
            var respUser = AccessPoint.Users.First(i => i.Id == responsibleId);
            if (!task.Project.Team.Contains(respUser))
            {
                TempData["ErrorMessage"] = "Selected responsible user is not in this project's team.";
                return RedirectToAction("Tasks", "Projects", new { id = task.Project.Id });
            }

            if (ModelState.IsValid)
            {
                t.Project = task.Project;
                t.Responsible = respUser;
                t.Status = AccessPoint.ProjectStatuses.First(i => i.Id == statusId);
                AccessPoint.ObjectContext.ApplyCurrentValues("Tasks", t);
                AccessPoint.SaveChanges();

                TempData["InformMessage"] = "Task has been updated.";
                return RedirectToAction("Tasks", "Projects", new {id = task.Project.Id});
            }
            else
            {
                ViewData["Statuses"] =
                    AccessPoint.ProjectStatuses.Select(
                        p =>
                        new SelectListItem
                            {
                                Text = p.Name,
                                Value = SqlFunctions.StringConvert((double) p.Id),
                                Selected = task.Status.Name == p.Name
                            }).
                        ToList();

                ViewData["Users"] =
                    task.Project.Team.Where(u => u.Role.Name == "Programmer").Select(
                        u =>
                        new SelectListItem
                            {
                                Text = u.Username,
                                Value = u.Id.ToString(),
                                Selected = u.Id == task.Responsible.Id
                            });
                return View(t);
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
    }
}
