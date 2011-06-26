using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NProject.Models.Domain;

namespace NProject.Models
{
    public class TaskAddToProjectViewModel
    {
        public int Id { get; set; }
        public string ProjectTitle { get; set; }
        public Task Task { get; set; }
        public List<SelectListItem> Users { get; set; }
        public List<SelectListItem> Statuses { get; set; }
    }
}