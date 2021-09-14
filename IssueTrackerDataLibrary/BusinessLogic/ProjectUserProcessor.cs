using System;
using System.Collections.Generic;
using System.Text;
using IssueTrackerDataLibrary.Models;
using IssueTrackerDataLibrary.DataAccess;

namespace IssueTrackerDataLibrary.BusinessLogic
{
    public static class ProjectUserProcessor
    {
        public static List<ProjectUserModel> ViewProjectUsers(int projectID)
        {
            string sql = string.Format("select * from dbo.ProjectUser where ProjectID = {0};", projectID);

            return SqlDataAccess.LoadData<ProjectUserModel>(sql);
        }

        public static bool UserBelongsToProject(int projectID, string userID)
        {
            return DoesProjectUserHaveThisRole(projectID, userID, 1);
        }

        public static bool DoesProjectUserHaveThisRole(int projectID, string userID, int role)
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
        public static int FindUserRoleInProject(int projectID, string userID)
        {
            string sql = string.Format("select Role from dbo.ProjectUser where ProjectID = {0} and UserID = '{1}';", projectID, userID);

            List<int> userRoles = SqlDataAccess.LoadData<int>(sql);

            // user has no role in project
            if (userRoles.Count == 0)
                return -1;

            // User should only have 1 role in project, therefore there should only be 1 result.
            return userRoles[0];
        }
    }
}
