using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IssueTracker.Models
{
    public class CommentModel
    {
        // Part of the form

        /// <summary>
        /// Represents the comment that was written out.
        /// </summary>
        [Required]
        [StringLength(maximumLength: 400, MinimumLength = 1)]
        public string Content { get; set; }

        // Not part of form

        /// <summary>
        /// Represents the date time the comment was written.
        /// Stored in the format YYYY-MM-DD HH:MI:SS.
        /// </summary>
        public DateTime DateTimeCreated { get; set; }

        /// <summary>
        /// Represents the id of the commentator.
        /// </summary>
        public string AuthorID { get; set; }

        /// <summary>
        /// Represents the name of the author.
        /// First and last name together.
        /// </summary>
        public string AuthorName { get; set; }


    }
}