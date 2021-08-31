using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IssueTracker.Models;
using IssueTrackerDataLibrary.BusinessLogic;
using Microsoft.AspNet.Identity;

namespace IssueTracker.Controllers
{
    [Authorize]
    public class IssueController : Controller
    {
        /// <summary>
        /// The method directs the user to the page containing
        /// a form to create a issue.
        /// </summary>
        public ActionResult CreateIssue()
        {
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
                    model.Priority);
                return RedirectToAction("../Home/About");
            }

            return View();
        }
    }
}