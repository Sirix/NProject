using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NProject.Models.Infrastructure;

namespace NProject.Models.Domain
{
    public class Meeting
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime? Meeting_date { get; set; }

        public virtual User Organizer { get; set; }
        public virtual ICollection<User> Members { get; set; }
        public virtual ProjectStatus Result_status { get; set; }

        public Meeting()
        {
// ReSharper disable DoNotCallOverridableMethodsInConstructor
            Members = new List<User>();
// ReSharper restore DoNotCallOverridableMethodsInConstructor

        }
    }
}