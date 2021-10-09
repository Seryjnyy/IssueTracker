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
        public ActionResult ViewAllUserActivityPartialView()
        {
            return PartialView("ViewActivityPartialView", FindActivityForUser());
        }

        public ActionResult ViewActivityForIssuePartialView(int issueID)
        {
            return PartialView("ViewActivityPartialView", FindActivityForIssue(issueID));
        }


        public PartialViewResult ViewActivityForProjectPartialView(int projectID)
        {
            return PartialView("ViewActivityPartialView", FindActivityForProject(projectID));
        }
        public List<ActivityModel> FindActivityForUser()
        {
            var data = ActivityProcessor.ViewAllUserActivity(User.Identity.GetUserId());
            return ConvertToModel(data);
        }

        public List<ActivityModel> FindActivityForProject(int projectID)
        {
            var data = ActivityProcessor.ViewActivityForProject(projectID);
            return ConvertToModel(data);
        }
        public List<ActivityModel> FindActivityForIssue(int issueID)
        {
            var data = ActivityProcessor.ViewActivityForIssue(issueID);
            return ConvertToModel(data);
        }

        private List<ActivityModel> ConvertToModel(List<IssueTrackerDataLibrary.Models.ActivityModel> data)
        {
            List<ActivityModel> activities = new List<ActivityModel>();

            foreach (var row in data)
            {
                activities.Add(new ActivityModel
                {
                    DateTimeCreated = row.DateTimeCreated,
                    ActivityContent = row.ActivityContent,
                });
            };

            return activities;
        }

        /// <summary>
        /// Shows all activity for a project
        /// </summary>
        /// <returns>
        /// A view that takes the ActivityModel and displays it.
        /// </returns>
        /*        public ActionResult ViewActivityForProject(int projectID)
                {
                    return View(FindActivityForProject(projectID));
                }*/

    }
}