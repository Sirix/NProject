using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using NProject.Models.Domain;
using NProject.Models.Infrastructure;

namespace NProject.BLL
{
    public class TaskService
    {
        [Dependency]
        public IAccessPoint AccessPoint { get; set; }

        public IEnumerable<Task> GetTasksForProject(int projectId)
        {
            return AccessPoint.Tasks.Where(t => t.Project.Id == projectId);
        }
    }
}
