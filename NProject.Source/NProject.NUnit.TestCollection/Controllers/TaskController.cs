using System.Linq;
using System.Web.Mvc;
using Moq;
using NProject.Controllers;
using NProject.Models.Domain;
using NProject.Models.Infrastructure;
using NUnit.Framework;
using System.Collections.Generic;

namespace NProject.NUnit.TestCollection.Controllers
{
    public static class TestHelper
    {
        public static ControllerContext InstantiateControllerContext(string username, bool authentificated = true)
        {
            var controllerContext = new Mock<ControllerContext>();

            // Just fake the name of the user we want to
            // "authenticate"
            controllerContext.SetupGet(x =>
                                       x.HttpContext.User.Identity.Name).Returns(username);

            // And tell the controllerContext that, sure,
            // we've logged in allright...
            controllerContext.SetupGet(x =>
                                       x.HttpContext.User.Identity.IsAuthenticated)
                .Returns(authentificated);

            controllerContext.SetupGet(x =>
                                       x.HttpContext.Request.IsAuthenticated)
                .Returns(authentificated);
            //controllerContext.SetupGet(x => x.RouteData).Returns();

            return controllerContext.Object;
        }
    }
    public class TaskController_Tests
    {
        [Test]
        public void AddToProject_GET_Redirect_When_Manager_Not_In_Project_Team()
        {
            var taskController = new TaskController();
            taskController.ControllerContext = TestHelper.InstantiateControllerContext("Manager");

            var db = new Mock<IAccessPoint>();
            var user = new User {Username = "Manager", Id = 1};
            db.SetupGet(x => x.Users).Returns(new InMemoryDbSet<User>(user));
            db.SetupGet(x => x.Projects).Returns(
                new InMemoryDbSet<Project>(new Project {Id = 1}));
            taskController.AccessPoint = db.Object;

            RedirectToRouteResult r = (RedirectToRouteResult) taskController.AddToProject(1);
            Assert.AreEqual("Projects", r.RouteValues["controller"]);
            Assert.AreEqual("List", r.RouteValues["action"]);
            Assert.AreEqual("You're not eligible to manage tasks of this project.",
                            taskController.TempData["ErrorMessage"]);
        }
        [Test]
        public void AddToProject_GET_Redirect_At_Unexisting_Target_Project()
        {
            var taskController = new TaskController();
            taskController.ControllerContext = TestHelper.InstantiateControllerContext("Manager");

            var db = new Mock<IAccessPoint>();
            var user = new User { Username = "Manager", Id = 1 };
            db.SetupGet(x => x.Users).Returns(new InMemoryDbSet<User>(user));
            db.SetupGet(x => x.Projects).Returns(
                new InMemoryDbSet<Project>(new Project { Id = 1 }));
            taskController.AccessPoint = db.Object;

            RedirectToRouteResult r = (RedirectToRouteResult)taskController.AddToProject(0);
            Assert.AreEqual("Projects", r.RouteValues["controller"]);
            Assert.AreEqual("List", r.RouteValues["action"]);
            Assert.AreEqual("Selected project does not exist.",
                            taskController.TempData["ErrorMessage"]);
        }
        [Test]
        public void AddToProject_GET_Out_Form()
        {
            var taskController = new TaskController();
            taskController.ControllerContext = TestHelper.InstantiateControllerContext("Manager");

            var db = new Mock<IAccessPoint>();
           // var programmerRole = new Role {Name = "Programmer", Id = 1};
           // var managerRole = new Role { Name = "Manager", Id = 2 };
            var manager = new User {Username = "Manager", Id = 1, Role = UserRole.Manager};
            var programmer = new User {Username = "Programmer", Id = 1, Role = UserRole.Programmer};
            var programmer2 = new User { Username = "Other Programmer", Id = 2, Role = UserRole.Programmer };
            var project = new Project
                              {
                                  Id = 1,
                                  Team = new List<User> {manager, programmer},
                                  Name = "Some test project."
                              };

            //db.SetupGet(x => x.Roles).Returns(new InMemoryDbSet<Role>(programmerRole, managerRole));
            db.SetupGet(x => x.Users).Returns(new InMemoryDbSet<User>(manager, programmer, programmer2));
            db.SetupGet(x => x.Projects).Returns(
                new InMemoryDbSet<Project>(project));
            taskController.AccessPoint = db.Object;

            ViewResult r = (ViewResult) taskController.AddToProject(1);
            var model = r.ViewData.Model as NProject.Models.ViewModels.TaskFormViewModel;

            //we have only one programmer in this project's team
            Assert.AreEqual(1, model.Programmers.Count());
            Assert.AreEqual(1, model.ProjectId);
            Assert.AreEqual(project.Name, model.ProjectTitle);
        }
    }
}
