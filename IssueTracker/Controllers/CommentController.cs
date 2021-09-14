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
    public class CommentController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="issueID"></param>
        /// <returns></returns>
        public ActionResult ViewCommentsForIssue(int issueID)
        {
            var data = CommentProcessor.ViewCommentsForIssue(issueID);
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            List<CommentModel> comments = new List<CommentModel>();
            
            // Finding ProjectID through searching database
            // there surely must be a better way
            ViewBag.IssueID = issueID;
            ViewBag.ProjectID = IssueProcessor.FindProjectIDThroughIssueID(issueID);

            foreach (var row in data)
            {
                var user = userManager.FindById(row.AuthorID);
                CommentModel model = new CommentModel
                {
                    AuthorID = row.AuthorID,
                    AuthorName = user.FirstName + " " + user.LastName,
                    Content = row.Content,
                    DateTimeCreated = row.DateTimeCreated
                };
                comments.Add(model);
            }

            return View(comments);
        }
        public ActionResult CreateCommentForIssue(int issueID)
        {
            TempData["IssueID"] = issueID;
            ViewBag.IssueID = issueID;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCommentForIssue(CommentModel data)
        {
            if (ModelState.IsValid)
            {
                int issueID = (int)TempData["IssueID"];
                int projectId = IssueProcessor.FindProjectIDThroughIssueID(issueID);
                DateTime dateTimeCreated = DateTime.Now;

                int recordsCreated = CommentProcessor.CreateCommentForIssue(
                    User.Identity.GetUserId(),
                    dateTimeCreated, 
                    data.Content,
                    issueID);

                recordsCreated = ActivityProcessor.CreateActivity(User.Identity.GetUserId(), projectId, dateTimeCreated, " commented on a issue.");
                RedirectToAction("../Home/About");
            }

            return View();
        }
    }
}