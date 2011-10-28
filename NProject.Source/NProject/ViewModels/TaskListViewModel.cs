using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NProject.Models.Domain;

namespace NProject.ViewModels
{
    public class TaskListViewModel : SiteMasterViewModel
    {
        public IEnumerable<Task> Tasks { get; set; }

        public string ProjectTitle { get; set; }
    }
}