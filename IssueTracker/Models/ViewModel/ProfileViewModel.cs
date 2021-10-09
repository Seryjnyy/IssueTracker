using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTracker.Models
{
    public class ProfileViewModel
    {
        public IndexViewModel IndexView { get; set; }
        public TabSelectModel TabSelect { get; set; }
    }
}