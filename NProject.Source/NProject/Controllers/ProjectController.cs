using System;
using System.Collections;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Practices.Unity;
using NProject.BLL;
using NProject.Models;
using NProject.Models.Domain;
using NProject.Models.Infrastructure;
using System.Collections.Generic;

namespace NProject.Controllers
{
    [HandleError]
    public class ProjectsController : Controller
    {
        [Dependency]
        public IProjectsRepository Repository { get; set; }
        [Dependency]
        public IAccessPoint AccessPoint { get; set; }

        //
        // GET: /Projects/
        [Authorize(Roles = "PM, Director, Customer, Programmer")]
        public ActionResult List()
        {
            var model = new ProjectListViewModel();
            var projects = Repository.GetProjectListForUserByRole(User.Identity.Name);
            var role = Roles.Provider.GetRolesForUser(User.Identity.Name)[0];

            switch (role)
            {
                case "Director":
                    model.TableTitle = "All company's projects";
                    break;

                case "Customer":
                    model.TableTitle = "All your projects";
                    break;

                case "PM":
                case "Programmer":
                    model.TableTitle = "Projects in which you're involved";
                    break;
            }
            model.Projects = projects;
            model.UserCanCreateAndDeleteProject = role == "Director";
            model.UserIsCustomer = role == "Customer";
            model.UserCanManageMeetings = role == "PM";

            return View(model);
        }

        //
        // GET: /Project/Tasks/5
        [Authorize(Roles = "PM, Director, Programmer")]
        public ActionResult Tasks(int id)
        {
            var project = Repository.GetProjectById(id);
            ValidateAccessToProject(project, "PM", "You're not eligible to view tasks of this project.");

            var tasks = project.Tasks.ToList();
            ViewData["ProjectId"] = id;
            ViewData["ProjectTitle"] = project.Name;
            ViewData["CanCreateTasks"] = AccessPoint.Users.First(i => i.Username == User.Identity.Name).Role.Name == "PM";
            ViewData["CanExecuteTask"] = AccessPoint.Users.First(i => i.Username == User.Identity.Name).Role.Name == "Programmer";
            return View("ProjectTasks", tasks);
        }

        private void ValidateAccessToProject(Project project, string role, string errorMessage)
        {
            var user = AccessPoint.Users.First(i => i.Username == User.Identity.Name);
            if (!project.Team.Contains(user) && user.Role.Name != "Director")
            {
                TempData["ErrorMessage"] = errorMessage;
                RedirectToAction("List").ExecuteResult(ControllerContext);
            }
        }
        
        /// <summary>
        /// Output form for add staff to project team
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "PM")]
        public ActionResult AddStaff(int id)
        {
            ViewData["ProjectId"] = id;
            var project = Repository.GetProjectById(id);
            ViewData["ProjectTitle"] = project.Name;
            ValidateAccessToProject(project, "PM", "You are not eligible to manage staff of this project");

            //get active programmers
            ViewData["Users"] =
                AccessPoint.Users.Where(
                    u =>
                    u.Role.Name == "Programmer" &&
                    (u.state) == (byte)(UserState.Working) &&
                    !u.Projects.Select(i => i.Id).Contains(project.Id)).
                    Select(
                        u => new SelectListItem { Text = u.Username, Value = SqlFunctions.StringConvert((double)u.Id) }).
                    ToList();

            return View();
        }

        [Authorize(Roles = "PM")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddStaff(int id, int userId)
        {
            var project = Repository.GetProjectById(id);
            ValidateAccessToProject(project, "PM", "You are not eligible to manage staff of this project.");
            var user = AccessPoint.Users.First(u => u.Id == userId);
            if (user.Role.Name != "Programmer" || user.UserState != UserState.Working)
            {
                TempData["ErrorMessage"] = "You can add only working programmers to project.";
                return RedirectToAction("Team", new { id = id });
            }

            project.Team.Add(user);
            AccessPoint.SaveChanges();
            TempData["InformMessage"] = "Programmer has been added to the project's team";
            return RedirectToAction("Team", new { id = id });
        }


        //
        // GET: /Project/Create

        [Authorize(Roles = "Director")]
        public ActionResult Create()
        {
            FillCreateEditForm();
            ViewData["PageTitle"] = "Create";
            return View();
        }

        //
        // POST: /Project/Create

        [HttpPost]
        [Authorize(Roles = "Director")]
        public ActionResult Create(int statusId, int pmId, int customerId, Project p)
        {
            try
            {
                if (!ModelState.IsValid) throw new ArgumentException();

                var pmUser = AccessPoint.Users.First(i => i.Id == pmId);
                if (pmUser.Role.Name != "PM")
                {
                    TempData["ErrorMessage"] = "You must select PM in PM field.";
                    throw new ArgumentException();
                    
                }
                var customUser = AccessPoint.Users.First(i => i.Id == customerId);
                if (customUser.Role.Name != "Customer")
                {
                    TempData["ErrorMessage"] = "You must select customer in customer field.";
                    throw new ArgumentException();

                }
                p.Status = AccessPoint.ProjectStatuses.First(i => i.Id == statusId);
                p.Team.Add(pmUser);
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
            ViewData["PM"] =
              AccessPoint.Users.Where(u => u.Role.Name == "PM").Select(
                  u => new SelectListItem { Text = u.Username, Value = SqlFunctions.StringConvert((double)u.Id) }).ToList();
            ViewData["Customer"] =
                AccessPoint.Users.Where(u => u.Role.Name == "Customer").Select(
                    u => new SelectListItem { Text = u.Username, Value = SqlFunctions.StringConvert((double)u.Id) }).ToList();
            ViewData["Statuses"] =
               AccessPoint.ProjectStatuses.Select(
                   u => new SelectListItem { Text = u.Name, Value = SqlFunctions.StringConvert((double)u.Id) }).ToList();
        }
        //
        // GET: /Project/Edit/5

        [Authorize(Roles="Director")]
        public ActionResult Edit(int id)
        {
            var project = Repository.GetProjectById(id);
            ViewData["PageTitle"] = "Edit";
            FillCreateEditForm();
            (ViewData["PM"] as IEnumerable<SelectListItem>).First(i => i.Text == project.Team.First(u => u.Role.Name == "PM").Username).
                Selected = true;
            (ViewData["Customer"] as IEnumerable<SelectListItem>).First(i => i.Text == project.Customer.Username).
                Selected = true;
            (ViewData["Statuses"] as IEnumerable<SelectListItem>).First(i => i.Text == project.Status.Name).
                Selected = true;

            return View("Create", project);
        }

        //
        // POST: /Project/Edit/5
        [Authorize(Roles = "Director")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int statusId, int pmId, int customerId, Project p)
        {
            try
            {
                var project = Repository.GetProjectById(p.Id);
                if (!ModelState.IsValid) throw new ArgumentException();

                var pmUser = AccessPoint.Users.First(i => i.Id == pmId);
                if (pmUser.Role.Name != "PM")
                {
                    TempData["ErrorMessage"] = "You must select PM in PM field.";
                    throw new ArgumentException();

                }
                var customUser = AccessPoint.Users.First(i => i.Id == customerId);
                if (customUser.Role.Name != "Customer")
                {
                    TempData["ErrorMessage"] = "You must select customer in customer field.";
                    throw new ArgumentException();

                }
                p.Status = AccessPoint.ProjectStatuses.First(i => i.Id == statusId);
                p.Team = project.Team;
                p.Tasks = project.Tasks;
                p.Team.Remove(p.Team.First(u => u.Role.Name == "PM"));
                p.Team.Add(pmUser);
                p.Customer = customUser;
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
            var project = Repository.GetProjectById(id);
            return View(project);
        }

        //
        // POST: /Project/Delete/5

        [Authorize(Roles = "Director")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection values)
        {
            var project = Repository.GetProjectById(id);
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
        [Authorize(Roles = "PM, Director, Programmer")]
        public ActionResult Team(int id)
        {
            var project = Repository.GetProjectById(id);
            ValidateAccessToProject(project, "PM", "You are not eligible to view team of this project");

            ViewData["ProjectId"] = id;
            ViewData["CanManageTeam"] = AccessPoint.Users.First(u => u.Username == User.Identity.Name).Role.Name ==
            "PM";
            return View(project.Team.OrderBy(u => u.Role.Name).ToList());
        }
        private void CheckRemoveStaffConditions(Project project, User user)
        {
            if (!project.Team.Contains(user))
            {
                TempData["ErrorMessage"] =
                    "This user is not in the team of this project, so you can't remove himself from project team.";
                RedirectToAction("Team", new { id = project.Id }).ExecuteResult(ControllerContext);
            }
            if (user.Role.Name == "PM")
            {
                TempData["ErrorMessage"] = "You can't remove yourself from project team.";
                RedirectToAction("Team", new { id = project.Id }).ExecuteResult(ControllerContext);
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
        [Authorize(Roles = "PM")]
        public ActionResult RemoveStaff(int id, int userId)
        {
            var project = Repository.GetProjectById(id);
            ValidateAccessToProject(project, "PM", "You are not eligible to manage team of this project.");
            var user = AccessPoint.Users.First(i => i.Id == userId);
            CheckRemoveStaffConditions(project, user);

            ViewData["UserName"] = user.Username;
            ViewData["UserId"] = user.Id;
            ViewData["ProjectName"] = project.Name;
            ViewData["ProjectId"] = project.Id;
            return View();
        }

        [Authorize(Roles = "PM")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult RemoveStaff(int id, int userId, FormCollection values)
        {
            var project = Repository.GetProjectById(id);
            ValidateAccessToProject(project, "PM", "You are not eligible to manage team of this project.");
            var user = AccessPoint.Users.First(i => i.Id == userId);
            CheckRemoveStaffConditions(project, user);

            project.Team.Remove(user);
            AccessPoint.SaveChanges();
            TempData["InformMessage"] = string.Format("User {0} has been removed from \"{1}\"'s team", user.Username,
                                                      project.Name);

            return RedirectToAction("Team", new { id = project.Id });
        }

        [Authorize(Roles = "PM, Director, Customer, Programmer")]
        public ActionResult Details(int id)
        {
            var project = Repository.GetProjectById(id);
            var user = AccessPoint.Users.First(u => u.Username == User.Identity.Name);
            string role = user.Role.Name;
            ViewData["ShowEditAction"] = role == "Director";
            switch (role)
            {
                case "Director":
                    return View(project);

                case "Customer":
                    if (project.Customer.Id == user.Id)
                        return View(project);
                    break;

                case "PM":
                case "Programmer":
                    if (project.Team.Contains(user))
                        return View(project);
                    break;
            }
            TempData["ErrorMessage"] = "You can't view information about this project";
            return RedirectToAction("List");
        }
    }
}
