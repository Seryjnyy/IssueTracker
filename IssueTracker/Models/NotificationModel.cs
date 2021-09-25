using System;
using System.Collections.Generic;
using System.Text;

namespace IssueTracker.Models
{
    public class NotificationModel
    {
        public int NotificationID { get; set; }
        public string UserID { get; set; }
        public string AuthorID { get; set; }
        public int ProjectID { get; set; }
        public int IssueID { get; set; }
        public string Content { get; set; }
        public DateTime DateTimeCreated { get; set; }

    }
}
