using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTrackerDataLibrary.Models
{
    public class CommentModel
    {
        /// <summary>
        /// Represents the comment that was written out.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Represents the date time the comment was written.
        /// Stored in the format YYYY-MM-DD HH:MI:SS.
        /// </summary>
        public DateTime DateTimeCreated { get; set; }

        /// <summary>
        /// Represents the author of the comment.
        /// </summary>
        public string AuthorID { get; set; }

        public int IssueID { get; set; }

        public int CommentID { get; set; }


    }
}