using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IssueTrackerDataLibrary.BusinessLogic;
using Microsoft.AspNet.Identity.Owin;
using IssueTracker.Models;
using Microsoft.AspNet.Identity;

namespace IssueTracker.Controllers
{
    [Authorize]
    public class ProjectUserController : Controller
    {
        public ActionResult ViewProjectUsers(int projectID)
        {
            var data = ProjectUserProcessor.ViewProjectUsers(projectID);
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            List<ProjectUserModel> projectUsers = new List<ProjectUserModel>();

            foreach (var row in data)
            {
                var user = userManager.FindById(row.UserID);
                ProjectUserModel model = new ProjectUserModel
                {
                    UserID = row.UserID,
                    ProjectID = row.ProjectID,
                    Role = row.Role,
                    UserEmail = user.Email,
                    UserName = user.FirstName + " " + user.LastName
                };
                projectUsers.Add(model);
            }

            return View(projectUsers);
        }
    }
}