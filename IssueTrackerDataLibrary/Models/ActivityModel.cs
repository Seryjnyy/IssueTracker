using System;
using System.Collections.Generic;
using System.Text;

namespace IssueTrackerDataLibrary.Models
{
    public class ActivityModel
    {
        public int ActivityId { get; set; }
        public int ProjectID { get; set; }
        public string UserID { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public string ActivityContent { get; set; }
    }
}
