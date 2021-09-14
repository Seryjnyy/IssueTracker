using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IssueTracker.Models
{
    public class UserModel
    {
        /// <summary>
        /// Represents the email of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Represents the first name of the user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Represents the last name of the user.
        /// </summary>
        public string LastName { get; set; }


    }
}