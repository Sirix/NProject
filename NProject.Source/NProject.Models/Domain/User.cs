using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NProject.Models.Infrastructure;

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

        [NotMapped]
        public UserState UserState
        {
            get { return (UserState) state; }
            set { state = (byte) value; }
        }

        [Obsolete("This property is used only by EF. Use UserState property instead.")]
        public byte state { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Project> Projects { get; set; }

        public User()
        {
            UserState = UserState.Working;
        }
    }
}