using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NProject.Models.Domain;
using NProject.Models.Infrastructure;

namespace NProject.Infrastructure
{
    public class AuthorizedToRolesAttribute : AuthorizeAttribute
    {
        public UserRole AllowedRoles { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new ViewResult { ViewName = "AccessError" };
            }
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated) return false;

            string userName = httpContext.User.Identity.Name;
            using (var dba = new DbAccessPoint())
            {
                var u = dba.Users.First(i => i.Username == userName);
                return (AllowedRoles.HasFlag(u.Role));
            }
        }
    }
}