using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTracker.Models.ViewModel
{
    public class DashboardViewModel
    {
        public DataCountModel Priority { get; set; }

        public DataCountModel Status { get; set; }

        public DataCountModel Type { get; set; }
    }
}