using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NProject.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Index()
        {
            return RedirectToAction("Http404");
        }

        public ActionResult Http404()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}
