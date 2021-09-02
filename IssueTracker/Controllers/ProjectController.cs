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
    public class ProjectController : Controller
    {
        /// <summary>
        /// Page to show the user all their projects.
        /// </summary>
        public ActionResult ViewProjects()
        {
            var data = ProjectProcessor.ViewAllUserProjects(User.Identity.GetUserId());
            List<ProjectModel> projects = new List<ProjectModel>();
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            foreach (var row in data)
            {
                var creator = userManager.FindById(row.UserID);
                projects.Add(new ProjectModel 
                { 
                    Name = row.Name,
                    Description = row.Description, 
                    Creator = row.UserID,
                    CreatorName = creator.FirstName + " " + creator.LastName,
                    DateTimeCreated = row.DateTimeCreated,
                    ProjectID = row.ProjectID
                });
            }

            return View(projects);
        }

        /// <summary>
        /// Page to enable a user to create a project.
        /// </summary>
        public ActionResult CreateProject()
        {
            return View();
        }

        /// <summary>
        /// Handles the data from the Create a project form.
        /// </summary>
        /// <param name="model">
        /// The model holding data from the form.
        /// </param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProject(ProjectModel model)
        {
            if (ModelState.IsValid)
            {
                int recordsCreated = ProjectProcessor.CreateProjectAndSetCreator(
                    User.Identity.GetUserId(),
                    model.Name,
                    model.Description,
                    DateTime.Now
                    );

                return RedirectToAction("../Home/About");
            }

            return View();
        }
    }
}