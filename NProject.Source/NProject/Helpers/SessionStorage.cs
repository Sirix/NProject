using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NProject.Models.Domain;

namespace NProject.Helpers
{
    public static class SessionStorage
    {
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
    }
}