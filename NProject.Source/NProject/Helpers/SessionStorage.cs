using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using NProject.Models.Domain;

namespace NProject.Helpers
{
    [Serializable]
    internal class UserSessionInfo
    {
        public int Id { get; set; }
        public UserRole Role { get; set; }
    }

    internal static class SessionStorage
    {
        public static UserSessionInfo User
        {
            get { return HttpContext.Current.Session["UserSessionInfo"] as UserSessionInfo; }
            set { HttpContext.Current.Session["UserSessionInfo"] = value; }
        }

        [Obsolete]
        public static int UserId
        {
            get { return (int)HttpContext.Current.Session["UserId"]; }
            set { HttpContext.Current.Session["UserId"] = value; }
        }
        [Obsolete]
        public static UserRole UserRole
        {
            get { return (UserRole)HttpContext.Current.Session["UserRole"]; }
            set { HttpContext.Current.Session["UserRole"] = value; }
        }
    }
}