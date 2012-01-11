using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NProject.Infrastructure
{
    public static class ControllerSecurityExtension
    {
        private static class ProjectSecurityDesc
        {
            public static bool Create()
            {


                return true;
            }
        }

        public static bool ValidateAccess(this Controller controller, string action, int id = 0)
        {
            return true;
        }
    }
}