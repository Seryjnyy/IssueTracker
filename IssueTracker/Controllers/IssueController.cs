using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IssueTracker.Models;
using IssueTrackerDataLibrary.BusinessLogic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace IssueTracker.Controllers
{
    [Authorize]
    public class IssueController : Controller
    {
        /// <summary>
        /// The method directs the user to the page containing
        /// a form to create a issue.
        /// </summary>

        public ActionResult CreateIssue(int projectID)
        {
            TempData["projectID"] = projectID;
            return View();
        }

        /// <summary>
        /// The method is for when the user fills out the form and
        /// submits it. IssueProcessor in the IssueTrackerDataLibrary
        /// is used to save the data into the database.
        /// </summary>
        /// <param name="model">
        /// Represents the data from the form that was filled out.
        /// </param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateIssue(IssueModel model)
        {
            if (ModelState.IsValid)
            {
                int recordsCreated = IssueProcessor.CreateIssue(
                    User.Identity.GetUserId(),
                    model.Assignee,
                    model.Description,
                    DateTime.Now,
                    model.DateTimeDeadline,
                    model.Label,
                    model.Priority,
                    (int) TempData["projectID"]);
                return RedirectToAction("../Home/About");
            }

            return View();
        }

        public ActionResult ViewIssuesForProject(int projectID)
        {
            var data = IssueProcessor.ViewIssuesForProject(projectID);
            List<IssueModel> issues = new List<IssueModel>();
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            foreach(var row in data)
            {
                var user = userManager.FindById(row.AuthorID);
                IssueModel model = new IssueModel
                {
                    Creator = row.AuthorID,
                    CreatorName = user.FirstName + " " + user.LastName,
                    Assignee = row.AssigneeID,
                    Description = row.Description,
                    DateTimeCreated = row.DateTimeCreated,
                    DateTimeDeadline = row.DateTimeDeadline,
                    Label = row.Label,
                    Priority = row.Priority
                };
                issues.Add(model);
            }

            return View(issues);
        }
    }
}