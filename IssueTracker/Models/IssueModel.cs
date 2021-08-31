using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IssueTracker.Models
{
    public class IssueModel
    {
        /// <summary>
        /// Represents the user id of the creator.
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// Represents the user who the issue was assigned to.
        /// Can be null to represent that no one was assigned.
        /// </summary>
        public string Assignee { get; set; }

        /// <summary>
        /// Represents the description of the issue.
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }

        /// <summary>
        /// Represents the time the issue was created.
        /// Stored in the format YYYY-MM-DD HH:MI:SS.
        /// </summary>
        public DateTime DateTimeCreated { get; set; }

        /// <summary>
        /// Represents the deadline for the issue.
        /// resolved by.
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime DateTimeDeadline { get; set; }

        /// <summary>
        /// Represents the list of lables applied to the issue.
        /// Max amount of values !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// Need to change back to list !!!!!!!!!!!!!!!!!!!!!!!!
        /// </summary>
        public int Label { get; set; }

        /// <summary>
        /// Represents the importance, the value will be
        /// used to sort issues.
        /// </summary>
        public int Priority { get; set; }
    }
}