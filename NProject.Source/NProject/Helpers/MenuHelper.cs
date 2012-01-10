using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using NProject.Models.Domain;

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
                var role = SessionStorage.User.Role;

                switch (role)
                {
                    case UserRole.TopManager:
                        menu.AppendFormat("<li>{0}</li>",
                                          helper.ActionLink("Projects", "List", "Projects").ToHtmlString());
                        break;

                    case UserRole.Customer:
                    case UserRole.Manager:
                        //menu.AppendFormat("<li>{0}</li>",
                        //                  helper.ActionLink("Meetings", "List", "Meeting").ToHtmlString());
                        menu.AppendFormat("<li>{0}</li>",
                                          helper.ActionLink("Projects", "List", "Projects").ToHtmlString());
                        break;

                    case UserRole.Programmer:
                        menu.AppendFormat("<li>{0}</li>",
                                          helper.ActionLink("Projects", "List", "Projects").ToHtmlString());
                        break;

                    case UserRole.Admin:
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

    public static class UIHelper
    {
        /// <summary>
        /// Creates a html element "select" from the specified enum.
        /// </summary>
        /// <typeparam name="TEnum">Type of enum which will be used.</typeparam>
        public static IEnumerable<SelectListItem> CreateSelectListFromEnum<TEnum>()
        {
            return UIHelper.CreateSelectListFromEnum(default(TEnum));
        }
        /// <summary>
        /// Creates a html element "select" from the specified enum and marks element as selected
        /// </summary>
        /// <typeparam name="TEnum">Type of enum which will be used.</typeparam>
        /// <param name="selectedValue">Value to select in output list.</param>
        public static IEnumerable<SelectListItem> CreateSelectListFromEnum<TEnum>(TEnum selectedValue)
        {
            var values = Enum.GetValues(typeof(TEnum));
            var names = Enum.GetNames(typeof(TEnum));
            int i = 0;
            var result = new List<SelectListItem>();
            foreach (TEnum v in values)
                result.Add(new SelectListItem { Text = names[i++], Value = i.ToString(), Selected = v.Equals(selectedValue) });

            return result;
        }
    }
}