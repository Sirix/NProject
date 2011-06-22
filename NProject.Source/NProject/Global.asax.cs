using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using NProject.Models;
using NProject.Models.Infrastructure;

namespace NProject
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static IUnityContainer Container;
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(null, "projects/", new { controller = "Projects", action = "List" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
            
        }

        protected void Application_Start()
        {
            ConfigureUnity();
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
        }

        private void ConfigureUnity()
        {
            Container = new UnityContainer();
            Container.LoadConfiguration();
            UnityServiceLocator locator = new UnityServiceLocator(Container);

            ServiceLocator.SetLocatorProvider(() => locator);

            ControllerBuilder.Current.SetControllerFactory(new UnityControllerFactory(Container));
        }
    }
}