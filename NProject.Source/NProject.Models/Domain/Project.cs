using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NProject.Models.Domain
{
    public class Project
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        //[Column(TypeName = "money")]
        //public decimal TotalCost { get; set; }

        //[Range(0, 100, ErrorMessage = "Progress must be in percents(0-100)")]
        //public int Progress { get; set; }

        //[Range(0, 100, ErrorMessage = "Price discount must be in percents(0...100)")]
        //public double PriceDiscount { get; set; }

        public DateTime CreationDate { get; private set; }

        [Required, DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime? DeliveryDate { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual User Customer { get; set; }
        public virtual ICollection<TeamMate> Team { get; set; }
        public virtual ProjectStatus Status { get; set; }

        public Project()
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            Tasks = new List<Task>();
            Team = new List<TeamMate>();
            CreationDate = DateTime.UtcNow;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }
    }
}