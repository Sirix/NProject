using System.Collections.Generic;
using NProject.Models.Domain;

namespace NProject.Models
{
    public class ProjectListViewModel
    {
        public string TableTitle { get; set; }
        public bool UserCanCreateAndDeleteProject { get; set; }
        public bool UserCanManageMeetings { get; set; }
        public bool UserIsCustomer { get; set; }

        public IEnumerable<Project> Projects { get; set; }
    }
}