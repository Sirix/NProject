using System;
using System.ComponentModel.DataAnnotations;

namespace NProject.Models.Domain
{
    public class Task
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime BeginDate { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public int EstimatedTime { get; set; } //in hours
        public int SpentTime { get; set; } //in hours

        public virtual ProjectStatus Status { get; set; }
        public virtual Project Project { get; set; }
        public virtual User Responsible { get; set; }

        public Task()
        {
            CreationDate = DateTime.Now;
        }
    }
}