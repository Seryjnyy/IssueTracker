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
        public ActionResult EditProject(int projectID)
        {
            var data = ProjectProcessor.ViewProject(projectID);

            ProjectModel project = new ProjectModel
            {
                Description = data.Description,
                Name = data.Name,
                ProjectID = projectID
            };

            return PartialView(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProject(ProjectModel data)
        {
            if (data.Description.Length < 200)
            {
                int recordsCreated = ProjectProcessor.UpdateDescription(data.ProjectID, data.Description);
                // could add error messages and that

                // create project activity

                // create activity
                ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = userManager.FindById(User.Identity.GetUserId());

                string activityConntet = string.Format("{0} has edited the project: {1}.", user.FirstName + " " + user.LastName, data.Name);
                recordsCreated = ActivityProcessor.CreateProjectActivity(User.Identity.GetUserId(), data.ProjectID, DateTime.Now, activityConntet);
            }

            return RedirectToAction("ViewProject", new { projectId = data.ProjectID });
        }


        public ActionResult ViewProject(int projectID)
        {
            // only allow admin and creator to see edit function and manageMembers link
            bool isCreatorOrAdmin = false;
            string userRole = ProjectUserProcessor.FindUserRoleInProject(projectID, User.Identity.GetUserId());
            if (userRole == "Admin" || userRole == "Creator")
                isCreatorOrAdmin = true;

            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var data = ProjectProcessor.ViewProject(projectID);
            ViewBag.projectID = projectID;

            var creator = userManager.FindById(data.UserID);
            ProjectModel project = new ProjectModel
            {
                Name = data.Name,
                Description = data.Description,
                Creator = data.UserID,
                CreatorName = creator.FirstName + " " + creator.LastName,
                DateTimeCreated = data.DateTimeCreated,
                ProjectID = data.ProjectID,
                IsCreatorOrAdmin = isCreatorOrAdmin
            };

            ViewBag.viewLocation = "ViewProject";
            return View(project);
        }

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

                return RedirectToAction("ViewProjects");
            }

            return View();
        }

        /*        public ActionResult ViewProjectsTab()
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
        }*/
    }
}