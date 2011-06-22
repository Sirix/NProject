using System.ComponentModel.DataAnnotations;

namespace NProject.Models.Domain
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Hash { get; set; }
        public string Email { get; set; }

        [Required]
        public virtual Role Role { get; set; }
    }
}