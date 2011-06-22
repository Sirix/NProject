using System.Web.Mvc;
using System.Web.Security;
using NProject.Models;
using NProject.Models.Domain;
using NProject.Models.Infrastructure;

namespace NProject.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private readonly IAccessPoint _db;

        public HomeController(IAccessPoint dbAccessPoint)
        {
            _db = dbAccessPoint;
        }

        public ViewResult Index()
        {
            //string role = ((RolePrincipal) User).GetRoles()[0];
            //var redirectData = ((IRedirectByRole)Roles.Provider).GetBaseLocationForRole(role);

            //return RedirectToRoute(new { controller = redirectData[0], action = redirectData[1] });
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}