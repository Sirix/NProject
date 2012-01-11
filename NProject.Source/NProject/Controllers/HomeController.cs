using System.Web.Mvc;
using System.Web.Security;
using NProject.Infrastructure;
using NProject.Models;
using NProject.Models.Domain;

namespace NProject.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
            ////becouse we provide only one role to one user
            //string role = Roles.Provider.GetRolesForUser(User.Identity.Name)[0];
            ////get base location for this role from db and redirect
            //var redirectData = ((IRedirectByRole)Roles.Provider).GetBaseLocationForRole(role);

            //return RedirectToAction(redirectData[1], redirectData[0]);
        }

        public ActionResult About()
        {
            return View();
        }

        [AuthorizedToRoles(AllowedRoles = UserRole.Manager)]
        public JsonResult TestManagerAction()
        {
            return new JsonResult() {Data = "123", JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }
    }
}