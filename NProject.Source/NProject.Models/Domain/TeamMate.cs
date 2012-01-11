using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace NProject.Models.Domain
{
    /// <summary>
    /// This class is used for support different roles of user in different projects
    /// </summary>
    public class TeamMate
    {
        public virtual int Id { get; set; }
        public virtual Project Project { get; set; }
        public virtual User User { get; set; }

        [NotMapped]
        public virtual UserRole Role
        {
            get { return (UserRole) Role_EF; }
            set { Role_EF = (int) value; }
        }

        [Column("Role")] public int Role_EF{get; set; }
    }

    //public class RoleWrapper
    //{
    //    private UserRole _t;

    //    public int Value
    //    {
    //        get { return (int) _t; }
    //        set { _t = (UserRole) value; }
    //    }

    //    public UserRole EnumValue
    //    {
    //        get { return _t; }
    //        set { _t = value; }
    //    }

    //    public static implicit operator RoleWrapper(UserRole p)
    //    {
    //        return new RoleWrapper {EnumValue = p};
    //    }

    //    public static implicit operator UserRole(RoleWrapper pw)
    //    {
    //        if (pw == null) return UserRole.Unspecified;
    //        else return pw.EnumValue;
    //    }
    //}
}
