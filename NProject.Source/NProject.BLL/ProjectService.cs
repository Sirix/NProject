using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NProject.Models.Domain;
using NProject.Models.Infrastructure;

namespace NProject.BLL
{
    public class ProjectService
    {
        private IAccessPoint AccessPoint { get; set; }

        public ProjectService()
        {
            this.AccessPoint = ServiceLocator.Current.GetInstance<IAccessPoint>();
        }

        public IEnumerable<Project> GetProjectListForUserByRole(int userId)
        {
            IEnumerable<Project> projects = Enumerable.Empty<Project>();

            var user = AccessPoint.Users.Single(i => i.Id == userId);
            return projects = AccessPoint.Projects.ToList();
            ////;
            ////switch (user.Role)
            ////{
            ////    case UserRole.TopManager:
            ////        projects = AccessPoint.Projects.ToList();
            ////        break;

            ////    case UserRole.Customer:
            ////        projects = AccessPoint.Projects.Where(p => p.Customer.Id == user.Id).ToList();
            ////        break;

            ////    //case "PM":

            ////    //    projects = AccessPoint.Projects.ToList().Where(p => p.Team.Contains(user)).ToList();
            ////    //    break;

            ////    case UserRole.Programmer:
            ////    case UserRole.Manager:
            ////        projects = user.Projects.ToList();
            ////        break;
            ////}

            ////return projects;
        }

        public Project GetProjectById(int id)
        {
            return AccessPoint.Projects.First(p => p.Id == id);
        }

    }
}
