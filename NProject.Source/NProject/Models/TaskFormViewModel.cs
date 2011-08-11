using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using NProject.Models.Domain;

namespace NProject.Models.ViewModels
{
    public class TaskFormViewModel
    {
        [Required(ErrorMessage = "You must specify project id.")]
        [Range(1, int.MaxValue, ErrorMessage = "You must specify project.")]
        public int ProjectId { get; set; }
        [Required(ErrorMessage = "You must specify responsible user id.")]
        [Range(1, int.MaxValue, ErrorMessage = "You must specify user.")]
        public int ResponsibleUserId { get; set; }
        [Required(ErrorMessage = "You must specify status id.")]
        [Range(1, int.MaxValue, ErrorMessage = "You must specify status.")]
        public int StatusId { get; set; }

        public string ProjectTitle { get; set; }
        public Task Task { get; set; }
        public IEnumerable<SelectListItem> Programmers { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
    }
}