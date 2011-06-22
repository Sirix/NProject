using System.ComponentModel.DataAnnotations;

namespace NProject.Models.Domain
{
    public class Role
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsBaseRole { get; private set; }

        public string BaseLocation { get; set; }

        internal Role AsBase()
        {
            IsBaseRole = true;
            return this;
        }
        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Description);
        }
    }
}
