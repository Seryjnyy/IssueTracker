using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IssueTrackerDataLibrary.Models
{
    public class ProjectModel
    {
        /// <summary>
        /// Represents the ID of the project.
        /// </summary>
        public int ProjectID { get; set; }

        /// <summary>
        /// Represents the name of the project.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Represents the description of the project.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Represents the user id of the creator.
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Represents the time the issue was created.
        /// Stored in the format YYYY-MM-DD HH:MI:SS.
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime DateTimeCreated { get; set; }

        /// <summary>
        /// Represents the users that are part of the project,
        /// along with their roles.
        /// </summary>
        /*public List<ProjectUserModel> Members { get; set; }*/

    }
}
