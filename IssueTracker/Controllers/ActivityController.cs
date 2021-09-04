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
        public ActionResult ViewActivityForProject(int projectID)
        {
            var data = ActivityProcessor.ViewActivityForProject(projectID);
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            List<ActivityModel> activities = new List<ActivityModel>();

            foreach (var row in data)
            {
                var user = userManager.FindById(row.UserID);
                ActivityModel model = new ActivityModel
                {
                    UserID = row.UserID,
                    ProjectID = row.ProjectID,
                    DateTimeCreated = row.DateTimeCreated,
                    ActivityContent = row.ActivityContent,
                    UserName = user.FirstName + " " + user.LastName
                };
                activities.Add(model);
            };

            return View(activities);
        }
    }
}