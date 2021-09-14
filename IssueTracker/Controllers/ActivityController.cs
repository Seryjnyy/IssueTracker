using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IssueTrackerDataLibrary.BusinessLogic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using IssueTracker.Models;

namespace IssueTracker.Controllers
{

    [Authorize]
    public class ActivityController : Controller
    {
        /// <summary>
        /// Shows all activity for a project
        /// </summary>
        /// <returns>
        /// A view that takes the ActivityModel and displays it.
        /// </returns>
        public ActionResult ViewActivityForProject(int projectID)
        {
            return View(FindActivityForProject(projectID));
        }

        [HttpGet]
        public PartialViewResult ViewActivityForProjectPartialView(int projectID)
        {
            ViewBag.projectID = projectID;
            return PartialView(FindActivityForProject(projectID));
        }

        public List<ActivityModel> FindActivityForProject(int projectID)
        {
            var data = ActivityProcessor.ViewActivityForProject(projectID);
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            List<ActivityModel> activities = new List<ActivityModel>();

            foreach (var row in data)
            {
                var user = userManager.FindById(row.UserID);
                activities.Add(new ActivityModel
                {
                    UserID = row.UserID,
                    ProjectID = row.ProjectID,
                    DateTimeCreated = row.DateTimeCreated,
                    ActivityContent = row.ActivityContent,
                    UserName = user.FirstName + " " + user.LastName
                });
            };

            return activities;
        }
    }
}