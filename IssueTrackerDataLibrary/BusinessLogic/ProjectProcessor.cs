using System;
using System.Collections.Generic;
using System.Text;
using IssueTrackerDataLibrary.Models;
using IssueTrackerDataLibrary.DataAccess;

namespace IssueTrackerDataLibrary.BusinessLogic
{
    public static class ProjectProcessor
    {
        /*        public static string GetFullNameOfID(string userID)
                {
                    string sql = string.Format("select FirstName, LastName from dbo. where UserID = '{0}';", userID);
                }*/

        public static string GetProjectName(int projectID)
        {
            string sql = string.Format("select Name from dbo.Project where ProjectId = {0};", projectID);
            return SqlDataAccess.LoadData<string>(sql)[0];
        }
        // we do not allow to change name
/*        public static int UpdateName(int projectID, string newName)
        {
            string sql = string.Format("update dbo.Project set Name = {0} where ProjectId = {1};", newName, projectID);
            return SqlDataAccess.ExecuteStatement(sql);
        }*/
        public static int UpdateDescription(int projectID, string newDescription)
        {
            string sql = string.Format("update dbo.Project set Description = '{0}' where ProjectId = {1};", newDescription, projectID);
            return SqlDataAccess.ExecuteStatement(sql);
        }

        public static int RemoveProject(int projectID)
        {
            string sql = string.Format("delete from dbo.Project where ProjectId = {0};", projectID);

            return SqlDataAccess.ExecuteStatement(sql);
        }

        public static List<ProjectModel> ViewAllUserProjects(string userID)
        {
            string sql = string.Format("select distinct ProjectID from dbo.ProjectUser where UserID = '{0}';", userID);
            List<int> projectIDs =  SqlDataAccess.LoadData<int>(sql);
            List<ProjectModel> projects = new List<ProjectModel>();


            foreach (int projectID in projectIDs)
            {
                projects.Add(ViewProject(projectID));    
            }

            return projects;
        }

        public static ProjectModel ViewProject(int projectID)
        {
            string sql = string.Format("select * from dbo.Project where ProjectId = '{0}';", projectID);
            return SqlDataAccess.LoadData<ProjectModel>(sql)[0];
        }

        /// <summary>
        /// Create the project first, then adds the creator to ProjectUser table as a project creator.
        /// </summary>
        /// <returns>
        /// Rows successfully inserted. 2 means everything was successfull, any less means there was a problem.
        /// </returns>
        public static int CreateProjectAndSetCreator(string creatorID, string name, string description, DateTime dateTimeCreated)
        {
            int recordsCreated = CreateProject(creatorID, name, description, dateTimeCreated);

            int projectID = GetProjectID(creatorID, name, description, dateTimeCreated);

            recordsCreated += AddProjectUser(projectID, creatorID, "Creator");

            return recordsCreated;
        }

        /// <summary>
        /// Get the ProjectID by matching the rest of the column values.
        /// </summary>
        /// <returns>
        /// The ProjectID for a project.
        /// </returns>
        public static int GetProjectID(string creatorID, string name, string description, DateTime dateTimeCreated)
        {
            string sql = @"select ProjectID from dbo.Project where UserID = @UserID and Name = @Name and Description = @Description and DateTimeCreated = @DateTimeCreated;";
            ProjectModel data = new ProjectModel
            {
                UserID = creatorID,
                Name = name,
                Description = description,
                DateTimeCreated = dateTimeCreated
            };

            return SqlDataAccess.LoadData<int, ProjectModel>(sql, data)[0];
        }

        /// <summary>
        /// Create project by inserting data into Project table.
        /// </summary>
        /// <returns>
        /// Amount of rows successfully inserted.
        /// </returns>
        public static int CreateProject(string creatorID, string name, string description, DateTime dateTimeCreated)
        {

            ProjectModel data = new ProjectModel 
            {
                UserID = creatorID,
                Name = name,
                Description = description,
                DateTimeCreated = dateTimeCreated
            };

            string sql = @"insert into dbo.Project (UserID, Name, Description, DateTimeCreated) values 
            (@UserID, @Name, @Description, @DateTimeCreated);";

            return SqlDataAccess.SaveData(sql, data);
        }

        /// <summary>
        /// Create a project user by inserting data into ProjectUser table.
        /// </summary>
        /// <returns>
        /// Amount of rows successfully inserted.
        /// </returns>
        public static int AddProjectUser(int projectID, string userID, string role)
        {
            ProjectUserModel data = new ProjectUserModel
            {
                ProjectID = projectID,
                UserID = userID,
                Role = role
            };

            string sql = @"insert into dbo.ProjectUser (ProjectID, UserID, Role) values (@ProjectId, @UserID, @Role)";

           
            return SqlDataAccess.SaveData(sql, data);
        }



/*        public static int ViewAllUserProjects(int userID)
        {
            string sql = string.Format("select ProjectID from dbo.Project where UserID = {0};", userID);

            List<string> projectIDs = DataAccess.SqlDataAccess.LoadData<string>(sql);

            foreach(string projectID in projectIDs)
            {
                sql = string.Format("select * from dbo.Project");
            }
            //use projectids to return project models

        }*/
    }
}
