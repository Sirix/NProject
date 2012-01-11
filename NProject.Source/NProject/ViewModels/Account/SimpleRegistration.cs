using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NProject.ViewModels.Account
{
    public class SimpleRegistration
    {
        [Required]
        public string Email { get; set; }
        [Required, StringLength(255, MinimumLength = 2)]
        public string Password { get; set; }
        [Required, StringLength(10, MinimumLength=2)]
        public string Name { get; set; }

        [Required]
        [Range(-24, 24)]
        public int TimeShiftFromUtc { get; set; }
    }
}