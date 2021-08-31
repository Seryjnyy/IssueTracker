using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTracker.Models
{
    public class ProjectUserModel
    {
        /// <summary>
        /// Represents the user.
        /// </summary>
        public UserModel User { get; set; }

        /// <summary>
        /// Represents the role of the user on the project.
        /// </summary>
        public string Role { get; set; }
    }
}