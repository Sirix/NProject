using System;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Practices.Unity;
using NProject.Models;
using NProject.Models.Domain;
using NProject.Models.Infrastructure;

namespace NProject.Controllers
{
    [HandleError]
    public class ProjectsController : Controller
    {
        [Dependency]
        public IAccessPoint AccessPoint { get; set; }

        //
        // GET: /Projects/
        [Authorize(Roles = "PM, Director, Customer, Programmer")]
        public ActionResult List()
        {
            var model = new ProjectListViewModel();
            var projects = Enumerable.Empty<Project>();
            var role = Roles.Provider.GetRolesForUser(User.Identity.Name)[0];

            switch (role)
            {
                case "Director":
                    projects = AccessPoint.Projects.ToList();
                    model.TableTitle = "All company's projects";
                    break;

                case "Customer":
                    var customer = AccessPoint.Users.First(u => u.Username == User.Identity.Name);
                    projects = AccessPoint.Projects.Where(p => p.Customer.Id == customer.Id).ToList();
                    model.TableTitle = "All your projects";
                    break;

                case "PM":
                    User manager = AccessPoint.Users.First(u => u.Username == User.Identity.Name && u.Role.Name == "PM");
                    projects = AccessPoint.Projects.ToList().Where(p => p.Team.Contains(manager)).ToList();
                    model.TableTitle = "Projects in which you're involved";
                    break;

                case "Programmer":
                    projects = AccessPoint.Users.First(u => u.Username == User.Identity.Name).Projects.ToList();
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
            var project = GetProjectById(id);
            ValidateAccessToProject(project, "PM", "You're not eligible to view tasks of this project.");

            var tasks = project.Tasks.ToList();
            ViewData["ProjectId"] = id;
            ViewData["CanCreateProjects"] = AccessPoint.Users.First(i => i.Username == User.Identity.Name).Role.Name == "PM";
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
        private Project GetProjectById(int id)
        {
            return AccessPoint.Projects.First(p => p.Id == id);
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
            var project = GetProjectById(id);
            ViewData["ProjectTitle"] = project.Name;
            ValidateAccessToProject(project, "PM", "You are not eligible to manage staff of this project");

            //get active programmers
            ViewData["Users"] =
                AccessPoint.Users.Where(
                    u =>
                    u.Role.Name == "Programmer" &&
                    (u.state) == (byte) (UserState.Working) &&
                    !u.Projects.Select(i => i.Id).Contains(project.Id)).
                    Select(
                        u => new SelectListItem {Text = u.Username, Value = SqlFunctions.StringConvert((double) u.Id)}).
                    ToList();

            return View();
        }

        [Authorize(Roles = "PM")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddStaff(int id, int userId)
        {
            var project = GetProjectById(id);
            ValidateAccessToProject(project, "PM", "You are not eligible to manage staff of this project.");
            var user = AccessPoint.Users.First(u => u.Id == userId);
            if (user.Role.Name != "Programmer" || user.UserState != UserState.Working)
            {
                TempData["ErrorMessage"] = "You can add only working programmers to project.";
                return RedirectToAction("Team", new {id = id});
            }

            project.Team.Add(user);
            AccessPoint.SaveChanges();
            TempData["InformMessage"] = "Programmer has been added to the project's team";
            return RedirectToAction("Team", new {id = id});
        }


        //
        // GET: /Project/Create

        [Authorize(Roles = "Director")]
        public ActionResult Create()
        {
            ViewData["PM"] =
                AccessPoint.Users.Where(u => u.Role.Name == "PM").Select(
                    u => new SelectListItem {Text = u.Username, Value = SqlFunctions.StringConvert((double) u.Id)});
            ViewData["Customer"] =
                AccessPoint.Users.Where(u => u.Role.Name == "Customer").Select(
                    u => new SelectListItem { Text = u.Username, Value = SqlFunctions.StringConvert((double)u.Id) });
            ViewData["Statuses"] =
               AccessPoint.ProjectStatuses.Select(
                   u => new SelectListItem { Text = u.Name, Value = SqlFunctions.StringConvert((double)u.Id) });

            return View();
        } 

        //
        // POST: /Project/Create

        [HttpPost]
        [Authorize(Roles = "Director")]
        public ActionResult Create(int statusId, int pmId, int customerId, Project p)
        {
            //TODO: Add check for roles of pmId, customerId
            if (ModelState.IsValid)
            {
                p.Status = AccessPoint.ProjectStatuses.First(i => i.Id == statusId);
                p.Team.Add(AccessPoint.Users.First(i => i.Id == pmId));
                p.Customer = AccessPoint.Users.First(i => i.Id == customerId);
                AccessPoint.Projects.Add(p);
                AccessPoint.SaveChanges();

                return RedirectToAction("List");
            }
            else
            {
                ViewData["PM"] =
               AccessPoint.Users.Where(u => u.Role.Name == "PM").Select(
                   u => new SelectListItem { Text = u.Username, Value = SqlFunctions.StringConvert((double)u.Id) });
                ViewData["Customer"] =
                    AccessPoint.Users.Where(u => u.Role.Name == "Customer").Select(
                        u => new SelectListItem { Text = u.Username, Value = SqlFunctions.StringConvert((double)u.Id) });
                ViewData["Statuses"] =
                   AccessPoint.ProjectStatuses.Select(
                       u => new SelectListItem { Text = u.Name, Value = SqlFunctions.StringConvert((double)u.Id) });

                return View();
            }
        }
        
        //
        // GET: /Project/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Project/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Project/Delete/5
 
        [Authorize(Roles="Director")]
        public ActionResult Delete(int id)
        {
            var project = GetProjectById(id);
            return View(project);
        }

        //
        // POST: /Project/Delete/5

        [Authorize(Roles = "Director")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection values)
        {
            var project = GetProjectById(id);
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
        [Authorize(Roles="PM, Director, Programmer")]
        public ActionResult Team(int id)
        {
            var project = GetProjectById(id);
            ValidateAccessToProject(project, "PM", "You are not eligible to view team of this project");

            ViewData["ProjectId"] = id;
            return View(project.Team.OrderBy(u => u.Role.Name).ToList());
        }
        private void CheckRemoveStaffConditions(Project project, User user)
        {
            if (!project.Team.Contains(user))
            {
                TempData["ErrorMessage"] =
                    "This user is not in the team of this project, so you can't remove himself from project team.";
                RedirectToAction("Team", new {id = project.Id}).ExecuteResult(ControllerContext);
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
                RedirectToAction("Team", new {id = project.Id}).ExecuteResult(ControllerContext);
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
            var project = GetProjectById(id);
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
            var project = GetProjectById(id);
            ValidateAccessToProject(project, "PM", "You are not eligible to manage team of this project.");
            var user = AccessPoint.Users.First(i => i.Id == userId);
            CheckRemoveStaffConditions(project, user);

            project.Team.Remove(user);
            AccessPoint.SaveChanges();
            TempData["InformMessage"] = string.Format("User {0} has been removed from \"{1}\"'s team", user.Username,
                                                      project.Name);

            return RedirectToAction("Team", new {id = project.Id});
        }
    }
}
