﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IssueTrackerDataLibrary.Models
{
    public class ProjectUserModel
    {
        /// <summary>
        /// Represents the ID of the project the user is on.
        /// </summary>
        public int ProjectID { get; set; }

        /// <summary>
        /// Represents the ID of the user.
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Represents the project role given to the user.
        /// </summary>
        public int Role { get; set; }
    }
}