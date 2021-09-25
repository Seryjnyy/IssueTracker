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
        // new stuff

        public ActionResult ViewCommentsForIssuePartialView(int issueID)
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

            return PartialView(comments);
        }
        public ActionResult CreateCommentForIssuePartialView()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment(CommentModel data)
        {
            if (ModelState.IsValid)
            {
                ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                int projectId = IssueProcessor.FindProjectIDThroughIssueID(data.IssueID);
                DateTime dateTimeCreated = DateTime.Now;

                int recordsCreated = CommentProcessor.CreateCommentForIssue(
                    User.Identity.GetUserId(),
                    dateTimeCreated,
                    data.Content,
                    data.IssueID);

                // notify creator
                // notify assignee

                var issueData = IssueProcessor.ViewIssue(data.IssueID);
                var commentator = userManager.FindById(User.Identity.GetUserId());
                string projectName = ProjectProcessor.GetProjectName(issueData.ProjectID);
                string notificationContent = "";

                // Create notification for issue assignee
                if (issueData.AssigneeID != null)
                {
                    // do not notify assignee, if comentator is assignee
                    if(User.Identity.GetUserId() != issueData.AssigneeID)
                    {
                        notificationContent = string.Format("{0} has commented on the issue: {1}, which you are assigned to, in project: {2}.", (commentator.FirstName + " " + commentator.LastName), issueData.Name, projectName);
                        NotificationProcessor.CreateNotification(issueData.AssigneeID, notificationContent);
                    }

                }

                // do not notify author, if commentator is the author
                if(User.Identity.GetUserId() != issueData.AuthorID)
                {
                    // Create notification for issue creator
                    notificationContent = string.Format("{0} has commented on the issue: {1}, which you created, in project: {2}.", (commentator.FirstName + " " + commentator.LastName), issueData.Name, projectName);
                    NotificationProcessor.CreateNotification(issueData.AuthorID, notificationContent);
                }



                ActivityProcessor.CreateProjectActivity(User.Identity.GetUserId(), projectId, dateTimeCreated, " commented on a issue.");
                ActivityProcessor.CreateIssueActivity(User.Identity.GetUserId(), data.IssueID, dateTimeCreated, " commented on the issue.");

                return RedirectToAction("ViewIssue", "Issue", new {issueID = data.IssueID});
            }

            return RedirectToAction("ViewIssue", "Issue", new { issueID = data.IssueID });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="issueID"></param>
        /// <returns></returns>
/*        public ActionResult ViewCommentsForIssue(int issueID)
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
        }*/

/*        public ActionResult CreateCommentForIssue(int issueID)
        {
            TempData["IssueID"] = issueID;
            ViewBag.IssueID = issueID;
            return View();
        }*/

/*        [HttpPost]
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

                recordsCreated = ActivityProcessor.CreateProjectActivity(User.Identity.GetUserId(), projectId, dateTimeCreated, " commented on a issue.");
                RedirectToAction("../Home/About");
            }

            return View();
        }*/
    }
}