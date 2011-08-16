using System;
using System.ComponentModel.DataAnnotations;
using NProject.Models.Infrastructure;

namespace NProject.Models.Domain
{
    [DataComparison("BeginDate", "EndDate", ErrorMessage = "Date of task completing must be later than begin date")]
    public class Task
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime? BeginDate { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public DateTime? EstimatedTime { get; set; }
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