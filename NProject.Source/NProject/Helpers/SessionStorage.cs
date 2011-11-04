using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using NProject.Models.Domain;

namespace NProject.Helpers
{
    public static class SessionStorage
    {
        static SessionStorage()
        {
            SessionId = (new Random().Next(1, 1000));
        }
        private static int SessionId;
        public static int UserId
        {
            get { return (int)HttpContext.Current.Session["UserId"]; }
            set { HttpContext.Current.Session["UserId"] = value; }
        }
        public static UserRole UserRole
        {
            get { return (UserRole)HttpContext.Current.Session["UserRole"]; }
            set { HttpContext.Current.Session["UserRole"] = value; }
        }
        private static void CheckSession()
        {
            //if(SessionId == 0)
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}