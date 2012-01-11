using System;
using System.Web;
using System.Web.Mvc;
using NProject.BLL;
using NProject.Helpers;
using NProject.Models.Domain;

namespace NProject.Infrastructure
{
    public class AuthorizedToRolesAttribute : AuthorizeAttribute
    {
        public UserRole AllowedRoles { get; set; }

        public AuthorizedToRolesAttribute()
        {
            AllowedRoles = UserRole.Unspecified;
        }

        public AuthorizedToRolesAttribute(UserRole roles)
        {
            AllowedRoles = roles;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new ViewResult {ViewName = "AccessError"};
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            //if request is from a guest
            if (!httpContext.Request.IsAuthenticated) return false;

            //if we don't have any specified roles, we allow any role
            if (AllowedRoles == UserRole.Unspecified) return true;

            //if we are here, we have authentificated user, so we can use SessionStorage
            int userId = SessionStorage.User.Id;
            var us = new UserService();

            //return AllowedRoles.HasFlag(us.GetUser(userId).Role);
            return true;
        }
    }
}