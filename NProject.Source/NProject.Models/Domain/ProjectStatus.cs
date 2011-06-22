using System.ComponentModel.DataAnnotations;

namespace NProject.Models.Domain
{
    public class ProjectStatus
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsBaseStatus { get; private set; }

        internal ProjectStatus AsBase()
        {
            IsBaseStatus = true;
            return this;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}