using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NProject.Models.Infrastructure;

namespace NProject.Models.Domain
{
    [Flags]
    public enum UserRole
    {
        Unspecified = 0,
        Programmer = 2,
        Manager = 4,
        TopManager = 8,
        Customer = 16,
        Tester = 32,
        Admin = 64
    }
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
        public byte state;

        [NotMapped]
        public UserRole Role
        {
            get { return (UserRole)role; }
            set { role = (byte)value; }
        }

        [Obsolete("This property is used only by EF. Use Role property instead.")]
        public byte role;

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Meeting> Meetings { get; set; }

        public User()
        {
            UserState = UserState.Working;
        }
    }
}