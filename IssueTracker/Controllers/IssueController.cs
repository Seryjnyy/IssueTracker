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
            ViewBag.ProjectID = projectID;
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
            int projectID = (int)TempData["projectID"];

            if (ModelState.IsValid)
            {
                DateTime dateTimeCreated = DateTime.Now;
                int recordsCreated = IssueProcessor.CreateIssue(
                    User.Identity.GetUserId(),
                    model.Assignee,
                    model.Description,
                    dateTimeCreated,
                    model.DateTimeDeadline,
                    model.Label,
                    model.Priority,
                    projectID);

                recordsCreated = ActivityProcessor.CreateActivity(User.Identity.GetUserId(), projectID, dateTimeCreated, " created a new issue.");
                return RedirectToAction("../Home/About");
            }

            return View();
        }
        public List<IssueModel> FindIssuesForProject(int projectID)
        {
            var data = IssueProcessor.ViewIssuesForProject(projectID);
            List<IssueModel> issues = new List<IssueModel>();
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            foreach (var row in data)
            {
                var user = userManager.FindById(row.AuthorID);

                issues.Add(new IssueModel
                {
                    Creator = row.AuthorID,
                    CreatorName = user.FirstName + " " + user.LastName,
                    Assignee = row.AssigneeID,
                    Description = row.Description,
                    DateTimeCreated = row.DateTimeCreated,
                    DateTimeDeadline = row.DateTimeDeadline,
                    Label = row.Label,
                    Priority = row.Priority,
                    IssueID = row.IssueID
                });
            }
            return issues;
        }

        public ActionResult ViewIssuesForProject(int projectID)
        {
            ViewBag.ProjectID = projectID;
            return View(FindIssuesForProject(projectID));
        }

        [HttpGet]
        public PartialViewResult ViewIssuesForProjectPartialView(int projectID)
        {
            ViewBag.projectID = projectID;
            return PartialView(FindIssuesForProject(projectID));
        }
    }
}