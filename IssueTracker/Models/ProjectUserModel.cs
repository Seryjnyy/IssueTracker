using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IssueTracker.Models
{
    public class ProjectUserModel
    {
        // Part of the form

        /// <summary>
        /// Represents the project role given to the user.
        /// </summary>
        [Required]
        public string Role { get; set; }

        /// <summary>
        /// Represents the email of the user.
        /// </summary>
        [Required]
        [StringLength(maximumLength: 256, MinimumLength = 3)]
        [DisplayName("User Email")]
        public string UserEmail { get; set; }

        // Not part of the form

        /// <summary>
        /// Represents the ID of the project the user is on.
        /// </summary>
        public int ProjectID { get; set; }

        /// <summary>
        /// Represents the ID of the user.
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Represents the users name.
        /// </summary>
        public string UserName { get; set; }

        public int ProjectUserID { get; set; }

    }
}