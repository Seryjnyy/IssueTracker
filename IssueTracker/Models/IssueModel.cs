using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IssueTracker.Models
{
    public class IssueModel
    {
        // Part of form

        /// <summary>
        /// Represents the description of the issue.
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [StringLength(maximumLength: 400, MinimumLength = 0)]
        public string Description { get; set; }

        /// <summary>
        /// Represents the deadline for the issue.
        /// resolved by.
        /// </summary>
        [DataType(DataType.DateTime)]
        [DisplayName("Deadline")]
        public DateTime DateTimeDeadline { get; set; }

        /// <summary>
        /// Represents the list of lables applied to the issue.
        /// Max amount of values !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// Need to change back to list !!!!!!!!!!!!!!!!!!!!!!!!
        /// </summary>
        [Range(1, 4)]
        public int Label { get; set; }

        /// <summary>
        /// Represents the importance, the value will be
        /// used to sort issues.
        /// </summary>
        [Required]
        [Range(1, 4)]
        public int Priority { get; set; }

        // Not part of form

        public int IssueID { get; set; }
        /// <summary>
        /// Represents the user id of the creator.
        /// </summary>
        public string Creator { get; set; }

        public string CreatorName { get; set; }

        /// <summary>
        /// Represents the user who the issue was assigned to.
        /// Can be null to represent that no one was assigned.
        /// </summary>
        public string Assignee { get; set; }

        public string AssigneeName { get; set; }

        /// <summary>
        /// Represents the time the issue was created.
        /// Stored in the format YYYY-MM-DD HH:MI:SS.
        /// </summary>
        public DateTime DateTimeCreated { get; set; }

        /// <summary>
        /// Represents the id of the project.
        /// </summary>
        public int ProjectID { get; set; }
    }
}