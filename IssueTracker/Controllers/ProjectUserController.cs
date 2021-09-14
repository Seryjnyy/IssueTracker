using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IssueTrackerDataLibrary.BusinessLogic;
using Microsoft.AspNet.Identity.Owin;
using IssueTracker.Models;
using Microsoft.AspNet.Identity;
using System.Web.Services;
using System.Web.Script.Services;

namespace IssueTracker.Controllers
{
    [Authorize]
    public class ProjectUserController : Controller
    {
/*        public ActionResult AddProjectUser()
        {
            // probably use claims somehow idk, man figure it out
            // ajax and jquery for dynamic search
        }*/

        public ActionResult EditProjectUserRolePartialView(int projectID)
        {
            return View();
        }

        public ActionResult ManageMembers(int projectID)
        {
            ViewBag.projectID = projectID;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageMembers(ProjectUserModel data)
        {
            dynamic showMessageString = string.Empty;

            if (ModelState.IsValid)
            {
                // validate email
                ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var users = userManager.Users.ToList();
                bool emailExists = false;
                string userID = "";

                foreach (var user in users)
                {
                    if(user.Email == data.UserEmail)
                    {
                        emailExists = true;
                        userID = user.Id;
                        break;
                    }
                }

                

                if (!emailExists)
                {
                    showMessageString = new
                    {
                        param1 = 404,
                        param2 = "Error wrong email or role"
                    };
                    return Json(showMessageString, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    int projectID = (int)TempData["projectID"];

                    int recordsCreated = ProjectProcessor.AddProjectUser(projectID, userID, data.Role);
/*                    return RedirectToAction("ManageMembers", new { projectID = projectID });*/
                    showMessageString = new
                    {
                        param1 = 200,
                        param2 = "You have enter correct date !!!"
                    };
                    return Json(showMessageString, JsonRequestBehavior.AllowGet);
                }
            }

            showMessageString = new
            {
                param1 = 404,
                param2 = "Error wrong email or role bu at the end"
            };
            return Json(showMessageString, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult AddProjectUserPartialView(int projectID)
        {
            ViewBag.projectID = projectID;
            TempData["projectID"] = projectID;
            return PartialView();
        }

        public PartialViewResult EditProjectUserRole(int projectID)
        {
            ViewBag.projectID = projectID;
            TempData["projectID"] = projectID;
            return PartialView();
        }

        public JsonResult AllUsersEmailAndName(int projectID)
        {
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            List<UserModel> users = new List<UserModel>();

            var data = userManager.Users.ToList();

            foreach (var row in data)
            {
                UserModel model = new UserModel
                {
                    Email = row.Email,
                    FirstName = row.FirstName,
                    LastName = row.LastName
                };
                users.Add(model);
            }
            return Json(users, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Currently not used because the ajax autocomplete needs to be reworked.
        /// Fetches Users that are part of a project in Json format.
        /// </summary>
        /// <returns>
        /// Returns a List of ProjectUserModel in Json format.
        /// </returns>
        public JsonResult FindUsersInProjectJson(int projectID)
        {
            var data = FindUsersInProject(projectID);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// It is responsible for fetching and mapping ProjectUsers from 
        /// the database into ProjectUserModels.
        /// It fetches users that are part of a project.
        /// </summary>
        private List<ProjectUserModel> FindUsersInProject(int projectID)
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
            return projectUsers;
        }


        /// <summary>
        /// Not really needed since it was a experiment with ajax.
        /// Action for Ajax call, it fetches ProjectUsers that are part
        /// of the project.
        /// </summary>
        /// <returns>
        /// A partial view, that contains the table for ProjectUserModel.
        /// </returns>
        [HttpGet]
        public PartialViewResult ViewUsers(int projectID)
        {
            var data = FindUsersInProject(projectID);

            foreach(var user in data)
            {
                if (user.Role == 1 && user.UserID == User.Identity.GetUserId())
                    ViewBag.canRemove = true;
            }

            ViewBag.projectID = projectID;

            return PartialView(data);
        }



        public ActionResult ViewProjectUsers(int projectID)
        {
            ViewBag.projectID = projectID;
            var data = FindUsersInProject(projectID);

            return View(data);
        }

        /// <summary>
        /// Enables only a creator of the project to add a user
        /// to the project. This is the form for adding a user.
        /// </summary>
        /// <returns>
        /// View for the add user form.
        /// </returns>
        public ActionResult AddProjectUser(int projectID)
        {
            // Is the user the creator of project?
            bool hasAbilityToAdd = ProjectUserProcessor.UserBelongsToProject(projectID, User.Identity.GetUserId());

            if (!hasAbilityToAdd)
                RedirectToAction("ViewProjectUsers", new {projectId = projectID});

            ViewBag.projectID = projectID;

            return View();
        }

        /// <summary>
        /// The HttpPost action that validates form 
        /// and then adds the user to the ProjectUser table.
        /// </summary>
        /// <param name="data">
        /// Form data.
        /// </param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProjectUser(ProjectUserModel data)
        {
            // redirect user back to proejct page, or view project users page
            return View();
        }
    }
}

