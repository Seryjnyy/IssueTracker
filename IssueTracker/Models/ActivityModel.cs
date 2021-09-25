using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTracker.Models
{
    public class ActivityModel
    {
        public int ProjectID { get; set; }
        public int IssueID { get; set; }
        public string UserID { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public string ActivityContent { get; set; }
        public string UserName { get; set; }
    }
}