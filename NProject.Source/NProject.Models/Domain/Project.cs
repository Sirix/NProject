using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NProject.Models.Domain
{
    public class Project
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Column(TypeName="money")]
        public decimal TotalCost { get; set; }
        public int Progress { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? DeliveryDate { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
        public virtual User Customer { get; set; }
        public virtual ICollection<User> Team { get; set; }
        [Required]
        public virtual ProjectStatus Status { get; set; }

        public Project()
        {
// ReSharper disable DoNotCallOverridableMethodsInConstructor
            Tasks = new List<Task>();
            Team = new List<User>();
// ReSharper restore DoNotCallOverridableMethodsInConstructor

        }
    }
}