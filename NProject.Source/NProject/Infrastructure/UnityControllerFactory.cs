using System;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace NProject
{
    public class UnityControllerFactory : DefaultControllerFactory
    {
        private readonly IUnityContainer _container;

        public UnityControllerFactory(IUnityContainer container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null) return null;

            return _container.Resolve(controllerType) as IController;
        }
    }
}