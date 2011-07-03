using System.Collections.Generic;
using NProject.Models.Domain;

namespace NProject.Models
{
    public class MeetingsListViewModel
    {
        public string TableTitle { get; set; }
        public bool UserCanCreateAndDeleteMeeting { get; set; }
        public bool UserCanManageMeetings { get; set; }

        public IEnumerable<Meeting> Meetings { get; set; }
    }
}