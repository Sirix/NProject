using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using Microsoft.Practices.Unity;
using NProject.Models.Domain;
using NProject.Models.Infrastructure;

namespace NProject.BLL
{
    public interface IProjectsRepository
    {
        IEnumerable<Project> GetProjectListForUserByRole(string username);
        Project GetProjectById(int id);
    }

    public class ProjectsRepository : IProjectsRepository
    {
        [Dependency]
        public IAccessPoint AccessPoint { get; set; }

        public IEnumerable<Project> GetProjectListForUserByRole(string username)
        {
            IEnumerable<Project> projects = Enumerable.Empty<Project>();

            var user = AccessPoint.Users.First(i => i.Username == username);
            string role = user.Role.Name;
            switch (role)
            {
                case "Director":
                    projects = AccessPoint.Projects.ToList();
                    break;

                case "Customer":
                    projects = AccessPoint.Projects.Where(p => p.Customer.Id == user.Id).ToList();
                    break;

                //case "PM":

                //    projects = AccessPoint.Projects.ToList().Where(p => p.Team.Contains(user)).ToList();
                //    break;

                case "Programmer":
                case "PM":
                    projects = user.Projects.ToList();
                    break;
            }

            return projects;
        }

        public Project GetProjectById(int id)
        {
            return AccessPoint.Projects.First(p => p.Id == id);
        }

    }
}
