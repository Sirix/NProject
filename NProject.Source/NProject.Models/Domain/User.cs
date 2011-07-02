using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NProject.Models.Domain
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        public string Hash { get; set; }
        [Required]
        public string Email { get; set; }
        public double HouseRate { get; set; }
        public bool IsActive { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Project> Projects { get; set; }

        public User()
        {
            IsActive = true;
            Projects = new List<Project>();
        }
    }
}