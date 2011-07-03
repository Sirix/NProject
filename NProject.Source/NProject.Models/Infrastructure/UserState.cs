using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NProject.Models.Infrastructure
{
    public enum UserState : byte
    {
        Undefined = 0,
        Free = 1,
        OnProject = 2,
        OnHolidays = 3,
        Dismissed = 4
    }
}
