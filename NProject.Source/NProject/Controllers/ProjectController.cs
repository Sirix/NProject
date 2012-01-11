using System;
using System.Collections;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Practices.Unity;
using NProject.BLL;
using NProject.Helpers;
using NProject.Infrastructure;
using NProject.Models.Domain;
using NProject.Models.Infrastructure;
using System.Collections.Generic;
using NProject.ViewModels;

namespace NProject.Controllers
{
    [HandleError]
    public class ProjectsController : Controller
    {
        private ProjectService ProjectService { get; set; }
        [Dependency]
        public IAccessPoint AccessPoint { get; set; }

        public ProjectsController()
        {
            ProjectService = new ProjectService();       
        }
        //
        // GET: /Projects/
        [AuthorizedToRoles]
        public ActionResult List()
        {
            var model = new ProjectListViewModel();
            int userId = SessionStorage.User.Id;
            var role = SessionStorage.User.Role;
            var projects = ProjectService.GetProjectListForUserByRole(userId);

            switch (role)
            {
                case UserRole.TopManager:
                    model.TableTitle = "All company's projects";
                    break;

                case UserRole.Customer:
                    model.TableTitle = "All your projects";
                    break;

                case UserRole.Manager:
                case UserRole.Programmer:
                    model.TableTitle = "Projects in which you're involved";
                    break;
            }
            model.Projects = projects;
            model.UserCanCreateAndDeleteProject = role == UserRole.TopManager;
            model.UserIsCustomer = role == UserRole.Customer;
            model.UserCanManageMeetings = role == UserRole.Manager;

            return View(model);
        }

        private void ValidateAccessToProject(Project project, string role, string errorMessage)
        {
            if (SessionStorage.User.Role != UserRole.TopManager &&
                !project.Team.Select(u => u.User.Id).Contains(SessionStorage.User.Id))
            {
                TempData["ErrorMessage"] = errorMessage;
                RedirectToAction("List").ExecuteResult(ControllerContext);
            }
        }

        /// <summary>
        /// Output form for add staff to project team
        /// </summary>
        /// <param name="id">Project id</param>
        /// <returns></returns>
        [AuthorizedToRoles(UserRole.Manager)]
        public ActionResult AddStaff(int id)
        {
            ViewData["ProjectId"] = id;
            var project = ProjectService.GetProjectById(id);
            ViewData["ProjectTitle"] = project.Name;
            ValidateAccessToProject(project, "PM", "You are not eligible to manage staff of this project");

            ////get active programmers
            //ViewData["Users"] = new SelectList(
            //    AccessPoint.Users.Where(
            //    u =>
            //    u.role == (int) UserRole.Programmer &&
            //    //u.state == (int) (UserState.Working) &&
            //    !u.Projects.Select(i => i.Id).Contains(project.Id)).ToList(), "Id", "Username");

            return View();
        }

        [AuthorizedToRoles]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddStaff(int id, int userId)
        {
            //var project = ProjectService.GetProjectById(id);
            //ValidateAccessToProject(project, "PM", "You are not eligible to manage staff of this project.");
            //var user = AccessPoint.Users.First(u => u.Id == userId);
            //if (user.Role != UserRole.Programmer || user.UserState != UserState.Working)
            //{
            //    TempData["ErrorMessage"] = "You can add only working programmers to project.";
            //    return RedirectToAction("Team", new {id = id});
            //}

            //project.Team.Add(user);
            //AccessPoint.SaveChanges();
            TempData["InformMessage"] = "Programmer has been added to the project's team";
            return RedirectToAction("Team", new { id = id });
        }


        //
        // GET: /Project/Create

        [AuthorizedToRoles(AllowedRoles = UserRole.TopManager)]
        public ActionResult Create()
        {
            bool access = this.ValidateAccess("project.create");
            FillCreateEditForm();
            ViewData["PageTitle"] = "Create";
            return View();
        }

        //
        // POST: /Project/Create

        [HttpPost]
        [AuthorizedToRoles(AllowedRoles = UserRole.TopManager)]
        public ActionResult Create(int statusId, int pmId, int customerId, Project p)
        {
            try
            {
                if (!ModelState.IsValid) throw new ArgumentException();

                var pmUser = AccessPoint.Users.First(i => i.Id == pmId);
               // if (pmUser.Role  != UserRole.Manager)
                //{
                //    TempData["ErrorMessage"] = "You must select PM in PM field.";
                //    throw new ArgumentException();
                    
                //}
                var customUser = AccessPoint.Users.First(i => i.Id == customerId);
                //if (customUser.Role != UserRole.Customer)
                //{
                //    TempData["ErrorMessage"] = "You must select customer in customer field.";
                //    throw new ArgumentException();

                //}
                p.Status = AccessPoint.ProjectStatuses.First(i => i.Id == statusId);
                p.Team.Add(new TeamMate {Role = UserRole.Manager, User = pmUser});
                p.Customer = customUser;
                AccessPoint.Projects.Add(p);
                AccessPoint.SaveChanges();

                return RedirectToAction("List");
            }
            catch (ArgumentException)
            {
                FillCreateEditForm();
                ViewData["PageTitle"] = "Create";
                return View();
            }
        }

        private void FillCreateEditForm()
        {
            //ViewData["PM"] =
            //  AccessPoint.Users.Where(u => u.role== (int)UserRole.Manager).Select(
            //      u => new SelectListItem { Text = u.Name, Value = SqlFunctions.StringConvert((double)u.Id) }).ToList();
            //ViewData["Customer"] =
            //    AccessPoint.Users.Where(u => u.role == (int)UserRole.Customer).Select(
            //        u => new SelectListItem { Text = u.Name, Value = SqlFunctions.StringConvert((double)u.Id) }).ToList();
            ViewData["Statuses"] =
               AccessPoint.ProjectStatuses.Select(
                   u => new SelectListItem { Text = u.Name, Value = SqlFunctions.StringConvert((double)u.Id) }).ToList();
        }
        //
        // GET: /Project/Edit/5

        [AuthorizedToRoles(AllowedRoles=UserRole.TopManager)]
        public ActionResult Edit(int id)
        {
            var project = ProjectService.GetProjectById(id);
            ViewData["PageTitle"] = "Edit";
            FillCreateEditForm();
            //(ViewData["PM"] as IEnumerable<SelectListItem>).First(i => i.Text == project.Team.First(u => u.Role== UserRole.Manager).Name).
           //     Selected = true;
            (ViewData["Customer"] as IEnumerable<SelectListItem>).First(i => i.Text == project.Customer.Name).
                Selected = true;
            (ViewData["Statuses"] as IEnumerable<SelectListItem>).First(i => i.Text == project.Status.Name).
                Selected = true;

            return View("Create", project);
        }

        //
        // POST: /Project/Edit/5
        [AuthorizedToRoles(AllowedRoles=UserRole.TopManager)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int statusId, int pmId, int customerId, Project p)
        {
            try
            {
                var project = ProjectService.GetProjectById(p.Id);
                if (!ModelState.IsValid) throw new ArgumentException();

                var pmUser = AccessPoint.Users.First(i => i.Id == pmId);
                //if (pmUser.Role != UserRole.Manager)
                //{
                //    TempData["ErrorMessage"] = "You must select PM in PM field.";
                //    throw new ArgumentException();

                //}
                var customUser = AccessPoint.Users.First(i => i.Id == customerId);
                //if (customUser.Role != UserRole.Customer)
                //{
                //    TempData["ErrorMessage"] = "You must select customer in customer field.";
                //    throw new ArgumentException();

                //}
                p.Status = AccessPoint.ProjectStatuses.First(i => i.Id == statusId);
                p.Team = project.Team;
                p.Tasks = project.Tasks;
                p.Team.Remove(p.Team.First(u => u.Role == UserRole.Manager));
                p.Team.Add(new TeamMate { Role = UserRole.Manager, User = pmUser });
                p.Customer = customUser;
                var pref = AccessPoint.Projects.First(pr => pr.Id == p.Id);
                AccessPoint.ObjectContext.ApplyCurrentValues("Projects", p);
                AccessPoint.SaveChanges();

                TempData["InformMessage"] = "Project has been updated.";
                return RedirectToAction("List");
            }
            catch (ArgumentException)
            {
                FillCreateEditForm();
                ViewData["PageTitle"] = "Edit";
                return View("Create", p);
            }
        }

        //
        // GET: /Project/Delete/5

        [Authorize(Roles = "Director")]
        public ActionResult Delete(int id)
        {
            var project = ProjectService.GetProjectById(id);
            return View(project);
        }

        //
        // POST: /Project/Delete/5

        [Authorize(Roles = "Director")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection values)
        {
            var project = ProjectService.GetProjectById(id);
            project.Team.Clear();

            AccessPoint.Projects.Remove(project);
            AccessPoint.SaveChanges();

            TempData["InformMessage"] = "Project removed";
            return RedirectToAction("List");
        }

        /// <summary>
        /// Outputs team on project with specified id
        /// </summary>
        /// <param name="id">Project id</param>
        /// <returns></returns>
        public ActionResult Team(int id)
        {
            var project = ProjectService.GetProjectById(id);
            ValidateAccessToProject(project, "PM", "You are not eligible to view team of this project");

            ViewData["ProjectId"] = id;
            ViewData["CanManageTeam"] = SessionStorage.User.Role == UserRole.Manager;

            return View(project.Team.OrderBy(u => u.Role).ToList());
        }

        private void CheckRemoveStaffConditions(Project project, User user)
        {
            if (!project.Team.Any(t => t.User.Id == user.Id))
            {
                TempData["ErrorMessage"] =
                    "This user is not in the team of this project, so you can't remove himself from project team.";
                RedirectToAction("Team", new {id = project.Id}).ExecuteResult(ControllerContext);
            }
            if ((new UserService().GetUserRoleInProject(user, project) == UserRole.Manager))
            {
                TempData["ErrorMessage"] = "You can't remove yourself from project team.";
                RedirectToAction("Team", new {id = project.Id}).ExecuteResult(ControllerContext);
            }
            //user has unfinished tasks
            if (project.Tasks.Where(t => t.Responsible.Id == user.Id && t.Status.Name != "Finished").Any())
            {
                TempData["ErrorMessage"] = "Please, transfer tasks of this programmer to other before his removing.\nOr, just complete them.";
                RedirectToAction("Team", new { id = project.Id }).ExecuteResult(ControllerContext);
            }
        }
        /// <summary>
        /// Removes user from project team
        /// </summary>
        /// <param name="id">Project id</param>
        /// <param name="userId">user id</param>
        /// <returns></returns>
        [AuthorizedToRoles(UserRole.Manager)]
        public ActionResult RemoveStaff(int id, int userId)
        {
            var project = ProjectService.GetProjectById(id);
            ValidateAccessToProject(project, "PM", "You are not eligible to manage team of this project.");
            var user = AccessPoint.Users.First(i => i.Id == userId);
            CheckRemoveStaffConditions(project, user);

            ViewData["UserName"] = user.Name;
            ViewData["UserId"] = user.Id;
            ViewData["ProjectName"] = project.Name;
            ViewData["ProjectId"] = project.Id;
            return View();
        }

        [AuthorizedToRoles(UserRole.Manager)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult RemoveStaff(int id, int userId, FormCollection values)
        {
            var project = ProjectService.GetProjectById(id);
            ValidateAccessToProject(project, "PM", "You are not eligible to manage team of this project.");
            var user = AccessPoint.Users.First(i => i.Id == userId);
            CheckRemoveStaffConditions(project, user);

            project.Team.Remove(project.Team.First(t => t.User.Id == user.Id));
            AccessPoint.SaveChanges();
            TempData["InformMessage"] = string.Format("User {0} has been removed from \"{1}\"'s team", user.Name,
                                                      project.Name);

            return RedirectToAction("Team", new { id = project.Id });
        }

        [AuthorizedToRoles]
        public ActionResult Details(int id)
        {
            var project = ProjectService.GetProjectById(id);
            var user = AccessPoint.Users.First(u => u.Id == SessionStorage.User.Id);
            var role = SessionStorage.User.Role;
            ViewData["ShowEditAction"] = role == UserRole.TopManager;
            switch (role)
            {
                case UserRole.TopManager:
                    return View(project);

                case UserRole.Customer:
                    if (project.Customer.Id == user.Id)
                        return View(project);
                    break;

                case UserRole.Manager:
                case UserRole.Programmer:
                    if (project.Team.Any(t => t.User.Id == user.Id))
                        return View(project);
                    break;
            }
            TempData["ErrorMessage"] = "You can't view information about this project";
            return RedirectToAction("List");
        }
    }
}
