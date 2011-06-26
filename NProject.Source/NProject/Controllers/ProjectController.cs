using System;
using System.Collections.Generic;
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
        [Authorize(Roles="PM, Director")]
        public ActionResult List()
        {
            List<Project> projects;

            if (User.IsInRole("Director"))
            {
                projects = AccessPoint.Projects.ToList();
                ViewData["TableTitle"] = "All company's projects";
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
        // GET: /Project/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Project/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Project/Create

        [HttpPost]
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
        public ActionResult Team(int id)
        {
            return View(AccessPoint.Projects.First(p => p.Id == id).Team.ToList());
        }
    }
}
