using Microsoft.Practices.ServiceLocation;
using NProject.Models.Infrastructure;

namespace NProject.BLL
{
    public abstract class BaseService
    {
        protected IAccessPoint Database { get; set; }

        protected BaseService()
        {
            Database = ServiceLocator.Current.GetInstance<IAccessPoint>();
        }
    }
}
