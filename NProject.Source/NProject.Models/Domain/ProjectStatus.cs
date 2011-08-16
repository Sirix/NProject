using System.ComponentModel.DataAnnotations;

namespace NProject.Models.Domain
{
    [System.Obsolete("Use enum NProject.Models.Domain.ItemStatus instead.")]
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
    public enum ItemStatus
    {
        Created = 1,
        Developing = 2,
        Suspended = 3,
        Finished = 4
    }
}