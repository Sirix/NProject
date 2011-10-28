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
    public class MeetingController : Controller
    {
        [Dependency]
        public IAccessPoint AccessPoint { get; set; }

        // GET: /Meeting/List

        [Authorize]
        public ActionResult List()
        {
            var model = new MeetingsListViewModel();
            var meetings = Enumerable.Empty<Meeting>();
            var role = Roles.Provider.GetRolesForUser(User.Identity.Name)[0];

            switch (role)
            {
                case "Customer":
                    var customer = AccessPoint.Users.First(u => u.Username == User.Identity.Name);
                    meetings = AccessPoint.Meeting.Where(p => p.Organizer.Id == customer.Id).ToList();
                    model.TableTitle = "All your meetings";
                    break;

                case "PM":
                    User manager = AccessPoint.Users.First(u => u.Username == User.Identity.Name && u.role == (byte)UserRole.Manager);
                    meetings = AccessPoint.Meeting.ToList().Where(p => p.Members.Contains(manager)).ToList();
                    model.TableTitle = "All meetings you are member of";
                    break;
            }
            model.Meetings = meetings;
            model.UserCanCreateAndDeleteMeeting = role == "Customer";
            model.UserCanManageMeetings = (role == "Customer" || role == "PM");

            return View(model);
        }

    }
}
