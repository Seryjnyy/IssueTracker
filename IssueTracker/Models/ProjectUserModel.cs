using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTracker.Models
{
    public class ProjectUserModel
    {
        /// <summary>
        /// Represents the ID of the project the user is on.
        /// </summary>
        public int ProjectID { get; set; }

        /// <summary>
        /// Represents the ID of the user.
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Represents the project role given to the user.
        /// </summary>
        public int Role { get; set; }

        public string UserEmail { get; set; }

        public string UserName { get; set; }
    }
}