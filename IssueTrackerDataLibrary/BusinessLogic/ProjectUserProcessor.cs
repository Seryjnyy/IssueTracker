using System;
using System.Collections.Generic;
using System.Text;
using IssueTrackerDataLibrary.Models;
using IssueTrackerDataLibrary.DataAccess;

namespace IssueTrackerDataLibrary.BusinessLogic
{
    public static class ProjectUserProcessor
    {
        public static string FindUserIdThrougProjectUserID(int projectUserID)
        {
            string sql = string.Format("select UserID from dbo.ProjectUser where ProjectUserId = {0};", projectUserID);

            return SqlDataAccess.LoadData<string>(sql)[0];
        }

        public static int RemoveProjectUser(int projectUserID)
        {
            string sql = string.Format("delete from dbo.ProjectUser where ProjectUserId = {0};", projectUserID);

            return SqlDataAccess.ExecuteStatement(sql);
        }

        public static List<ProjectUserModel> ViewProjectUsers(int projectID)
        {
            string sql = string.Format("select * from dbo.ProjectUser where ProjectID = {0};", projectID);

            return SqlDataAccess.LoadData<ProjectUserModel>(sql);
        }

        /*        public static List<string> FindAllUserIDForProject(int projectID)
                {
                    string sql = string.Format("select UserID from dbo.ProjectUser where ProjectID = {0};", projectID);

                    return SqlDataAccess.LoadData<string>(sql);
                }*/

        public static List<ProjectUserModel> AllUserIDandIdandRoleForProject(int projectID)
        {
            string sql = string.Format("select UserID, ProjectUserId, Role from dbo.ProjectUser where ProjectID = {0};", projectID);

            return SqlDataAccess.LoadData<ProjectUserModel>(sql);
        }

        public static List<string> FindAllEmailsExceptCreatorForProject(int projectID)
        {
            string sql = string.Format("select UserID from dbo.ProjectUser where ProjectID = {0} and Role != 'Admin';", projectID);

            return SqlDataAccess.LoadData<string>(sql);
        }

        public static int EditProjectUserRole(int projectUserID, string role)
        {
            ProjectUserModel data = new ProjectUserModel
            {
                Role = role,
                ProjectUserId = projectUserID
            };

            string sql = @"update dbo.ProjectUser set Role = @Role where ProjectUserId = @ProjectUserId and Role != 'Admin';";

            return SqlDataAccess.SaveData(sql, data);
        }


/*        public static bool UserBelongsToProject(int projectID, string userID)
        {
            return DoesProjectUserHaveThisRole(projectID, userID, "");
        }*/

        public static bool DoesProjectUserHaveThisRole(int projectID, string userID, string role)
        {
            return FindUserRoleInProject(projectID, userID) == role;
        }

        /// <summary>
        /// Check users role in a project. 
        /// </summary>
        /// <returns>
        /// Will return the user role. If user is not part of the project
        /// -1 will be returned instead.
        /// </returns>
        public static string FindUserRoleInProject(int projectID, string userID)
        {
            string sql = string.Format("select Role from dbo.ProjectUser where ProjectID = {0} and UserID = '{1}';", projectID, userID);

            List<string> userRoles = SqlDataAccess.LoadData<string>(sql);

            // user has no role in project
            if (userRoles.Count == 0)
                return "No role";

            // User should only have 1 role in project, therefore there should only be 1 result.
            return userRoles[0];
        }

        public static bool DoesUserBelongToProject(int projectID, string userID)
        {
            string sql = string.Format("select ProjectUserId from dbo.ProjectUser where ProjectID = {0} and UserID = '{1}';", projectID, userID);

            List<ProjectUserModel> data = SqlDataAccess.LoadData<ProjectUserModel>(sql);
            return data.Count > 0 ? true : false;
        }
    }
}
