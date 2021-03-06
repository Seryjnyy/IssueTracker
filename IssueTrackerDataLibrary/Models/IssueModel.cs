using System;
using System.Collections.Generic;
using System.Text;

namespace IssueTrackerDataLibrary.Models
{
    public class IssueModel
    {
        public int IssueId { get; set; }
        /// <summary>
        /// Represents the user who created the issuse.
        /// </summary>
        public string AuthorID { get; set; }

        /// <summary>
        /// Represents the user who the issue was assigned to.
        /// Can be null to represent that no one was assigned.
        /// </summary>
        public string AssigneeID { get; set; }

        /// <summary>
        /// Represents the description of the issue. 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Represents the time the issue was created.
        /// Stored in the format YYYY-MM-DD HH:MI:SS.
        /// </summary>
        public DateTime DateTimeCreated { get; set; }

        /// <summary>
        /// Represents the deadline for the issue to be 
        /// resolved by.
        /// </summary>
        public DateTime DateTimeDeadline { get; set; }

        public DateTime DateTimeUpdated { get; set; }

        public string Status { get; set; }

        public string Priority { get; set; }

        public int ProjectID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

    }
}
