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
        // removing issue

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveIssue(FormCollection data)
        {
            int issueID = Int32.Parse(data["IssueID"]);
            bool success = IssueProcessor.RemoveIssue(issueID) == 1;

            if(!success)
                RedirectToAction("ViewProject", "Issue", new { issueID = issueID });

            string cameFrom = data["cameFrom"];
            int projectID = Int32.Parse(data["ProjectID"]); ;

            // create projectActivity
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());
            string projectName = ProjectProcessor.GetProjectName(projectID);

            string activityConntet = string.Format("{0} has removed the issue: {1}, in project: {2}.", user.FirstName + " " + user.LastName, data["Name"], projectName);
            ActivityProcessor.CreateProjectActivity(User.Identity.GetUserId(), projectID, DateTime.Now, activityConntet);

            switch (cameFrom)
            {
                case "ManageIssues":
                    return RedirectToAction("ManageIssues", "Issue", new { projectID = projectID});
                case "ViewProject":
                    return RedirectToAction("ViewProject", "Project", new { projectID = projectID });
                default:
                    return RedirectToAction("ViewAllIssues", "Issue");
            }

        }

        // removing issue

        // mapping function
        private IssueModel ConvertSingleModel(IssueTrackerDataLibrary.Models.IssueModel data)
        {
            List<IssueTrackerDataLibrary.Models.IssueModel> temp = new List<IssueTrackerDataLibrary.Models.IssueModel>();
            temp.Add(data);
            return ConvertToModel(temp)[0];
        }

        private List<IssueModel> ConvertToModel(List<IssueTrackerDataLibrary.Models.IssueModel> data)
        {
            List<IssueModel> issues = new List<IssueModel>();
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            foreach (var row in data)
            {
                var user = userManager.FindById(row.AuthorID);
                var assignee = userManager.FindById(row.AssigneeID);

                issues.Add(new IssueModel
                {
                    AuthorID = row.AuthorID,
                    AuthorName = user.FirstName + " " + user.LastName,
                    AssigneeID = row.AssigneeID,
                    AssigneeName = assignee != null ? assignee.FirstName + " " + assignee.LastName : "",
                    Description = row.Description,
                    DateTimeCreated = row.DateTimeCreated,
                    DateTimeDeadline = row.DateTimeDeadline,
                    DateTimeUpdated = row.DateTimeUpdated,
                    DateTimeDeadlinePicker = row.DateTimeCreated.ToString("yyyy-MM-ddTHH:mm:ss"),
                    Status = row.Status,
                    Priority = row.Priority,
                    IssueID = row.IssueId,
                    Type = row.Type,
                    Name = row.Name,
                    ProjectID = row.ProjectID
                });
            }
            return issues;
        }

        // viewing your issues and assigneed to you

        public ActionResult ViewIssuesYouCreated()
        {
            var data = IssueProcessor.ViewIssuesCreatedByUser(User.Identity.GetUserId());
            List<IssueModel> issues = ConvertToModel(data);

            return PartialView("ViewIssues", issues);
        }

        public ActionResult ViewIssuesAssigendToYou()
        {
            var data = IssueProcessor.ViewIssuesAssignedToUser(User.Identity.GetUserId());
            List<IssueModel> issues = ConvertToModel(data);

            ViewBag.moreID = "Assigned";
            return PartialView("ViewIssues", issues);
        }

        public ActionResult ViewAllIssues(string selectTab = "")
        {
            ViewBag.selectTab = selectTab;
            ViewBag.viewLocation = "ViewAllIssues";

            return View();
        }

        // new stuff for manage issues

        public ActionResult ManageIssues(int projectID, string selectTab = "")
        {
/*            TabSelectModel tabSelect = new TabSelectModel
            {
                TabName = selectTab
            };*/

            ViewBag.projectID = projectID;
            ViewBag.selectTab = selectTab;
            ViewBag.projectName = ProjectProcessor.GetProjectName(projectID);
            ViewBag.viewLocation = "ManageIssues";

            return View();
        }

        public ActionResult AllEmailAndIDInProject(int projectID)
        {
            var data = IssueProcessor.FindAllUserIDForProject(projectID);
            List<ProjectUserModel> projectUsers = new List<ProjectUserModel>();
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            foreach (string userID in data)
            {
                var user = userManager.FindById(userID);

                projectUsers.Add(new ProjectUserModel
                {
                    UserID = userID,
                    UserEmail = user.Email,
                    UserName = user.FirstName + " " + user.LastName
                }); ;
            }

            return PartialView("_SelectEmailValueUserID", projectUsers);
        }

        [HttpGet]
        public ActionResult ViewIssuesForProjectPartialView(int projectID, string viewLocation = "")
        {
            var issues = ConvertToModel(IssueProcessor.ViewIssuesForProject(projectID));
            ViewBag.viewLocation = viewLocation;

            return PartialView("ViewIssues", issues);
        }

        public ActionResult CreateIssuePartialView()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateIssuePartialView(IssueModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                DateTime dateTimeCreated = DateTime.Now;
                int recordsCreated = IssueProcessor.CreateIssue(
                    User.Identity.GetUserId(),
                    model.AssigneeID,
                    model.Description,
                    model.Name,
                    model.Type,
                    dateTimeCreated,
                    model.DateTimeDeadline,
                    model.Status,
                    model.Priority,
                    model.ProjectID);


                // create activity
                var user = userManager.FindById(User.Identity.GetUserId());
                string projectName = ProjectProcessor.GetProjectName(model.ProjectID);

                string activityConntet = string.Format("{0} has created a new issue, issue: {1}, in project: {2}.", user.FirstName + " " + user.LastName, model.Name, projectName);
                recordsCreated = ActivityProcessor.CreateProjectActivity(User.Identity.GetUserId(), model.ProjectID, dateTimeCreated, activityConntet);

                // create notification for assigneed user if there is one

                if (model.AssigneeID != null)
                {
                    // do not notify assignee, if issue creator is assignee
                    if(model.AssigneeID != User.Identity.GetUserId())
                    {
                        string notificationContent = string.Format("{0} has assigneed you to the issue: {1}, in project: {2}.", (user.FirstName + " " + user.LastName), model.Name, projectName);
                        NotificationProcessor.CreateNotification(model.AssigneeID, notificationContent);
                    }

                }


                return RedirectToAction("ManageIssues", new {projectID = model.ProjectID });
            }

            return View();
        }

        // new stuff for view issue

        // ViewIssue and its partial views
        public ActionResult ViewIssue(int issueID, string cameFrom = "")
        {
            var issue = ConvertSingleModel(IssueProcessor.ViewIssue(issueID));

            ViewBag.isCreator = (issue.AuthorID == User.Identity.GetUserId());
            // for comment partial view
            ViewBag.issueID = issueID;
            ViewBag.cameFrom = cameFrom;

            return View(issue);
        }

        public ActionResult EditIssuePartialView(int issueID)
        {
            var issue = ConvertSingleModel(IssueProcessor.ViewIssue(issueID));

            return PartialView(issue);
        }

        // currently only changing description
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditIssuePartialView(IssueModel data)
        {
            if (data.Description.Length < 200)
            {
                int recordsUpdated = IssueProcessor.UpdateIssue(data.IssueID,
                                                            data.Description,
                                                            data.DateTimeDeadline,
                                                            data.Priority,
                                                            data.Type,
                                                            data.Status,
                                                            data.AssigneeID);

                // succesfull
                if(recordsUpdated == 1)
                {
                    // should add 
                    // when assigneed user is changed they should be notified that they are not on it anymore
                    // then notify new assignee

                    // create activity, but before that update the way its created
                    ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var editor = userManager.FindById(User.Identity.GetUserId());
                    string projectName = ProjectProcessor.GetProjectName(data.ProjectID);

                    string activityConntet = string.Format("{0} has edited the issue: {1}, in project: {2}.", editor.FirstName + " " + editor.LastName, data.Name, projectName);
                    ActivityProcessor.CreateProjectActivity(User.Identity.GetUserId(), data.ProjectID, DateTime.Now, activityConntet);
                    ActivityProcessor.CreateIssueActivity(User.Identity.GetUserId(), data.IssueID, DateTime.Now, activityConntet);

                    string notificationContent = "";

                    // Create notification for issue assignee
                    if (data.AssigneeID != null)
                    {
                        if(data.AssigneeID != "")
                        {
                            notificationContent = string.Format("{0} has edited the issue: {1}, which you are assigned to, in project: {2}.", (editor.FirstName + " " + editor.LastName), data.Name, projectName);
                            NotificationProcessor.CreateNotification(data.AssigneeID, notificationContent);

                        }
                    }
                    // do not send notif to creator if creator is the one editing
                    if (data.AuthorID != User.Identity.GetUserId())
                    {
                        // Create notification for issue creator
                        notificationContent = string.Format("{0} has edited the issue: {1}, which you created, in project: {2}.", (editor.FirstName + " " + editor.LastName), data.Name, projectName);
                        NotificationProcessor.CreateNotification(data.AuthorID, notificationContent);
                    }

                }

            }


            return RedirectToAction("ViewIssue", "Issue", new {issueID = data.IssueID });
        }

        // ViewIssue and its partial views


        // NEED HTTP POST FOR EDITING ISSUES

        /*        public List<IssueModel> FindIssuesForProject(int projectID)
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
                }*/

        /// <summary>
        /// The method is for when the user fills out the form and
        /// submits it. IssueProcessor in the IssueTrackerDataLibrary
        /// is used to save the data into the database.
        /// </summary>
        /// <param name="model">
        /// Represents the data from the form that was filled out.
        /// </param>
        /*        [HttpPost]
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

                        recordsCreated = ActivityProcessor.CreateProjectActivity(User.Identity.GetUserId(), projectID, dateTimeCreated, " created a new issue.");
                        return RedirectToAction("../Home/About");
                    }

                    return View();
                }*/


        /*        public ActionResult ViewIssuesForProject(int projectID)
                {
                    ViewBag.ProjectID = projectID;
                    return View(FindIssuesForProject(projectID));
                }*/

        /// <summary>
        /// The method directs the user to the page containing
        /// a form to create a issue.
        /// </summary>

        /*        public ActionResult CreateIssue(int projectID)
                {
                    TempData["projectID"] = projectID;
                    ViewBag.ProjectID = projectID;
                    return View();
                }*/

        /*        [HttpGet]
                public PartialViewResult ViewIssuesForProjectPartialView(int projectID)
                {
                    ViewBag.projectID = projectID;
                    return PartialView(FindIssuesForProject(projectID));
                }*/


        /*        public ActionResult RemoveIssue()
        {

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditIssuePartialView()
        {

        }*/
    }
}