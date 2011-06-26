using System.Web.Mvc;
using System.Web.Security;
using NProject.Models;

namespace NProject.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            //becouse we provide only one role to one user
            string role = Roles.Provider.GetRolesForUser(User.Identity.Name)[0];
            //get base location for this role from db and redirect
            var redirectData = ((IRedirectByRole)Roles.Provider).GetBaseLocationForRole(role);

            return RedirectToAction(redirectData[1], redirectData[0]);

        }

        public ActionResult About()
        {
            return View();
        }
    }
}