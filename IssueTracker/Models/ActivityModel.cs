using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTracker.Models
{
    public class ActivityModel
    {
        /// <summary>
        /// Represents the name of the project the activity happend in.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Represents the name of the user who created the comment.
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Represents the actual message in the comment.
        /// </summary>
        public string Content { get; set; }

    }
}