using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NProject.Models.Domain
{
    public class Company
    {
        /// <summary>
        /// Company id
        /// </summary>
        [Key]
        public virtual int Id { get; set; }
        /// <summary>
        /// Reference to owner of this company
        /// </summary>
        //public virtual int OwnerId { get; set; }
       // public virtual User Owner { get; set; }

        /// <summary>
        /// Company name
        /// </summary>
        [Required, StringLength(100)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Date of creation
        /// </summary>
        public virtual DateTime CreationDate { get; private set; }

        /// <summary>
        /// True, if this company has no restrictions; otherwise, false
        /// </summary>
        public virtual bool IsActivated { get; set; }
        /// <summary>
        /// Collection of company projects
        /// </summary>
        public virtual ICollection<Project> Projects { get; set; }

        public Company()
        {
            CreationDate = DateTime.UtcNow;
        }
    }
}