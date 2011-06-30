using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Practices.Unity;
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
        [Authorize(Roles="PM, Director, Customer")]
        public ActionResult List()
        {
            List<Project> projects;

            if (User.IsInRole("Director"))
            {
                projects = AccessPoint.Projects.ToList();
                ViewData["TableTitle"] = "All company's projects";
            }
            else if (User.IsInRole("Customer"))
            {
                var customer = AccessPoint.Users.First(u => u.Username == User.Identity.Name);

                projects = AccessPoint.Projects.Where(p => p.Customer.Id == customer.Id).ToList();
                ViewData["TableTitle"] = "All your projects";
            }
            else
            {
                User manager = AccessPoint.Users.First(u => u.Username == User.Identity.Name && u.Role.Name == "PM");
                projects = AccessPoint.Projects.ToList().Where(p => p.Team.Contains(manager)).ToList();
                ViewData["TableTitle"] = "Projects in which you're involved";
            }
            return View(projects);
        }

        //
        // GET: /Project/Tasks/5
        [Authorize(Roles = "PM, Director")]
        public ActionResult Tasks(int id)
        {
            var tasks = AccessPoint.Projects.First(p => p.Id == id).Tasks.ToList();
            ViewData["ProjectId"] = id;
            return View("ProjectTasks", tasks);
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
            ViewData["ProjectTitle"] = AccessPoint.Projects.First(p => p.Id == id).Name;

            //get free programmers
            ViewData["Users"] =
                AccessPoint.Users.Where(u => u.Role.Name == "Programmer" && u.Projects.Count == 0).Select(
                    u => new SelectListItem {Text = u.Username, Value = SqlFunctions.StringConvert((double) u.Id)});
            return View();
        }

        [Authorize(Roles = "PM")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddStaff(int id, int userId)
        {
            AccessPoint.Projects.First(p => p.Id == id).Team.Add(AccessPoint.Users.First(u => u.Id == userId));
            AccessPoint.SaveChanges();

            return RedirectToAction("Team", new {id = id});
        }


        //
        // GET: /Project/Create

        [Authorize(Roles = "Director")]
        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Project/Create

        [HttpPost]
        [Authorize(Roles = "Director")]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
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
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Project/Delete/5

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

        /// <summary>
        /// Outputs team on project with specified id
        /// </summary>
        /// <param name="id">Project id</param>
        /// <returns></returns>
        [Authorize(Roles="PM, Director")]
        public ActionResult Team(int id)
        {
            ViewData["ProjectId"] = id;
            return View(AccessPoint.Projects.First(p => p.Id == id).Team.ToList());
        }
    }
}
