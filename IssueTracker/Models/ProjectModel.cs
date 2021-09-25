using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IssueTracker.Models
{
    public class ProjectModel
    {
        // Part of form

        /// <summary>
        /// Represents the name of the project.
        /// </summary>
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 1)]
        public string Name { get; set; }

        /// <summary>
        /// Represents the description of the project.
        /// </summary>
        [StringLength(maximumLength: 200, MinimumLength = 0)]
        public string Description { get; set; }

        // Not part of form

        /// <summary>
        /// Id of project.
        /// </summary>
        public int ProjectID { get; set; }

        /// <summary>
        /// Represents the user id of the creator.
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// Represents the creators name.
        /// First name and last name together.
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        /// Represents the time the issue was created.
        /// Stored in the format YYYY-MM-DD HH:MI:SS.
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime DateTimeCreated { get; set; }

        public bool IsCreatorOrAdmin { get; set; }
        /// <summary>
        /// Represents the users that are part of the project,
        /// along with their roles.
        /// </summary>
        /*public List<ProjectUserModel> Members { get; set; }*/

    }
}