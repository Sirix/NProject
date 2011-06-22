using System.ComponentModel.DataAnnotations;

namespace NProject.Models.Domain
{
    public class Task
    {
        public int Id { get; set; }
        [Required]
        public virtual Project Project { get; set; } 
        [Required]
        public string Description { get; set; } 
    }
}