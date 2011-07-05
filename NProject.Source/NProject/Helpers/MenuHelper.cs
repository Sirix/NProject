using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Security;

namespace NProject.Helpers
{
    public static class MenuHelper
    {
        public static MvcHtmlString MenuButtons(this HtmlHelper helper)
        {
            StringBuilder menu = new StringBuilder();
            try
            {
                string user = helper.ViewContext.HttpContext.User.Identity.Name;
                string role = Roles.Provider.GetRolesForUser(user)[0];

                switch (role)
                {
                    case "Director":
                        menu.AppendFormat("<li>{0}</li>",
                                          helper.ActionLink("Projects", "List", "Projects").ToHtmlString());
                        break;

                    case "Customer":
                    case "PM":
                        menu.AppendFormat("<li>{0}</li>",
                                          helper.ActionLink("Meetings", "List", "Meeting").ToHtmlString());
                        menu.AppendFormat("<li>{0}</li>",
                                          helper.ActionLink("Projects", "List", "Projects").ToHtmlString());
                        break;

                    case "Programmer":
                        menu.AppendFormat("<li>{0}</li>",
                                          helper.ActionLink("Projects", "List", "Projects").ToHtmlString());
                        break;

                    case "Admin":
                        menu.AppendFormat("<li>{0}</li>",
                                          helper.ActionLink("Users", "List", "Account").ToHtmlString());
                        break;
                }
            }
            catch { }
            menu.AppendFormat("<li>{0}</li>",
                                         helper.ActionLink("About", "About", "Home").ToHtmlString());
            return MvcHtmlString.Create(menu.ToString());
        }
    }
}