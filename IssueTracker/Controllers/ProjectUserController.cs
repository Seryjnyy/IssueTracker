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
        public ActionResult RemoveUserFromProject(int projectUserID, int projectID)
        {
            ProjectUserProcessor.RemoveProjectUser(projectUserID);
            return RedirectToAction("ManageMembers", new {projectID =  projectID});
        }

        public ActionResult EditProjectUserRolePartialView(int projectID)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProjectUserRole(FormCollection collection)
        {
            string role = collection["Role"];
            int projectID = Int32.Parse(collection["ProjectID"]);

            if(collection["projectUsers"] == null)
                return RedirectToAction("ManageMembers", new { projectID = projectID, editUserMessage = "Incorrect users provided, no edit was made." });

            string[] projectUserIDs = collection["projectUsers"].Split(',');

            // stuff to notify users
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleChanger = userManager.FindById(User.Identity.GetUserId());
            string projectName = ProjectProcessor.GetProjectName(projectID);
            string notificationContent = string.Format("{0} has changed your role to {1}, in project: {2}.", (roleChanger.FirstName + " " + roleChanger.LastName), role, projectName);

            int recordsEdited = 0;
            foreach (var strUserID in projectUserIDs)
            {
                int userID = -1;
                try
                {
                    userID = Int32.Parse(strUserID);
                }
                catch (FormatException)
                {
                    continue;
                }
                recordsEdited = + ProjectUserProcessor.EditProjectUserRole(userID, role);

                // Create notification for user whos role got changed
                NotificationProcessor.CreateNotification(strUserID, notificationContent);
            }

            ActivityProcessor.CreateProjectActivity(User.Identity.GetUserId(), projectID, DateTime.Now, " edited user roles.");

            return RedirectToAction(
                "ManageMembers", 
                new {projectID = projectID, 
                    editUserMessage = string.Format("{0} out of {1} records were successfully edited",
                    recordsEdited, projectUserIDs.Length)
                });
        }

        public ActionResult ManageMembers(int projectID,  string addUserMessage = "", string editUserMessage = "")
        {
            // only allow admin and creator
            string userRole = ProjectUserProcessor.FindUserRoleInProject(projectID, User.Identity.GetUserId());
            if (userRole != "Admin" && userRole != "Creator")
                return RedirectToAction("Index", "Home");

            ViewBag.projectID = projectID;
            if (addUserMessage != "")
                ViewBag.addUserMessage = addUserMessage;

            if (editUserMessage != "")
                ViewBag.editUserMessage = editUserMessage;


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // needs renaming 
        public ActionResult AddProjectUserToProject(ProjectUserModel data)
        {
            dynamic showMessageString = string.Empty;
            int projectID = data.ProjectID;

            if (ModelState.IsValid)
            {
                // validate email
                ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = userManager.FindByEmail(data.UserEmail);

                if (user == null)
                    return RedirectToAction("ManageMembers", new { projectID = projectID, addUserMessage = "Unable to add user. Please check the email." });

                string userID = user.Id;
                // check if user already belongs to project
                bool userAlreadyInProject = ProjectUserProcessor.DoesUserBelongToProject(projectID, userID);
                if(userAlreadyInProject)
                    return RedirectToAction("ManageMembers", new { projectID = projectID, addUserMessage = "Unable to add user. User already part of the project." });

                int recordsCreated = ProjectProcessor.AddProjectUser(projectID, userID, data.Role);
                ActivityProcessor.CreateProjectActivity(User.Identity.GetUserId(), projectID, DateTime.Now, " added user to project.");
                return RedirectToAction("ManageMembers", new { projectID = projectID });
            }

            return RedirectToAction("ManageMembers", new { projectID = projectID, addUserMessage = "Unable to add user. Please check the email and role."});
        }

        public PartialViewResult AddProjectUserPartialView(int projectID)
        {
            ViewBag.projectID = projectID;
            TempData["projectID"] = projectID;
            return PartialView();
        }

        public JsonResult AllUserExceptAdminAndCreator(int projectID)
        {
            List<ProjectUserModel> users = AllUserEmailAndIDForProject(projectID);

            for(int i = 0; i < users.Count(); i++)
            {
                if(users[i].Role == "Admin" || users[i].Role == "Creator")
                {
                    users.RemoveAt(i);
                }
            }

            return Json(users, JsonRequestBehavior.AllowGet);
        }

        public List<ProjectUserModel> AllUserEmailAndIDForProject(int projectID)
        {
            var data = ProjectUserProcessor.AllUserIDandIdandRoleForProject(projectID);
            List<ProjectUserModel> projectUsers = new List<ProjectUserModel>();

            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            foreach (var row in data)
            {
                var user = userManager.FindById(row.UserID);
                projectUsers.Add(new ProjectUserModel
                {
                    UserEmail = user.Email,
                    Role = row.Role,
                    ProjectUserID = row.ProjectUserId
                });
            }

            return projectUsers;
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
                projectUsers.Add( new ProjectUserModel
                {
                    UserID = row.UserID,
                    ProjectUserID = row.ProjectUserId,
                    ProjectID = row.ProjectID,
                    Role = row.Role,
                    UserEmail = user.Email,
                    UserName = user.FirstName + " " + user.LastName
                });
            }
            return projectUsers;
        }

        // NOT MAPPIND DATALIBRARY MODELS TO APP MODELS!!!!!!!!!!!!!!

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
                if (user.Role == "Admin" && user.UserID == User.Identity.GetUserId())
                {
                    ViewBag.canRemove = true;
                    break;
                }
            }

            ViewBag.projectID = projectID;

            return PartialView(data);
        }
        /*        public JsonResult AllUserEmailandIDInProject(int projectID)
        {
            var emails = ProjectUserProcessor.FindAllUserIDForProject(projectID);
            List<ProjectUserModel> projectUsers = new List<ProjectUserModel>();

            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var users = userManager.Users.ToList();

            foreach (var email in emails)
            {
                foreach(var user in users)
                {
                    if(user.Email == email)
                    {
                        projectUsers.Add(new ProjectUserModel
                        {
                            UserEmail = email,
                            UserID = user.Id
                        });
                    }
                }


            }
            return Json(projectUsers, JsonRequestBehavior.AllowGet);
        }*/


        /*        public ActionResult ViewProjectUsers(int projectID)
                {
                    ViewBag.projectID = projectID;
                    var data = FindUsersInProject(projectID);

                    return View(data);
                }*/

        /// <summary>
        /// Enables only a creator of the project to add a user
        /// to the project. This is the form for adding a user.
        /// </summary>
        /// <returns>
        /// View for the add user form.
        /// </returns>
        /*        public ActionResult AddProjectUser(int projectID)
                {
                    // Is the user the creator of project?
                    bool hasAbilityToAdd = ProjectUserProcessor.UserBelongsToProject(projectID, User.Identity.GetUserId());

                    if (!hasAbilityToAdd)
                        RedirectToAction("ViewProjectUsers", new {projectId = projectID});

                    ViewBag.projectID = projectID;

                    return View();
                }*/

        /// <summary>
        /// The HttpPost action that validates form 
        /// and then adds the user to the ProjectUser table.
        /// </summary>
        /// <param name="data">
        /// Form data.
        /// </param>
        /*        [HttpPost]
                [ValidateAntiForgeryToken]
                public ActionResult AddProjectUser(ProjectUserModel data)
                {
                    // redirect user back to proejct page, or view project users page
                    return View();
                }*/

        /*        public PartialViewResult EditProjectUserRole(int projectID)
        {
            ViewBag.projectID = projectID;
            TempData["projectID"] = projectID;
            return PartialView();
        }*/

        /// <summary>
        /// Fetches all emails in the Identity db
        /// </summary>
        /// <returns></returns>
        public JsonResult AllUsersEmailAndName()
        {
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            List<UserModel> users = new List<UserModel>();

            var data = userManager.Users.ToList();

            foreach (var row in data)
            {
                if (row.Id == User.Identity.GetUserId())
                    continue;

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


        /*        public ActionResult AddProjectUser()
        {
            // probably use claims somehow idk, man figure it out
            // ajax and jquery for dynamic search
        }*/
    }
}

