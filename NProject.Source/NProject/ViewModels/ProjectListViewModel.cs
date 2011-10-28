using System.Collections.Generic;
using NProject.Models.Domain;

namespace NProject.ViewModels
{
    public class ProjectListViewModel : SiteMasterViewModel
    {
        public string TableTitle { get; set; }
        public bool UserCanCreateAndDeleteProject { get; set; }
        public bool UserCanManageMeetings { get; set; }
        public bool UserIsCustomer { get; set; }

        public IEnumerable<Project> Projects { get; set; }
    }
}