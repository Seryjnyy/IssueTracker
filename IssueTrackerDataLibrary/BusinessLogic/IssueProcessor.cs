using IssueTrackerDataLibrary.Models;
using IssueTrackerDataLibrary.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace IssueTrackerDataLibrary.BusinessLogic
{
    public static class IssueProcessor
    {

        public static int UpdateIssueDescription(int issueID, string description)
        {
            string sql = string.Format("update dbo.Issue set Description = '{0}' where IssueId = {1};", description, issueID);

            return SqlDataAccess.ExecuteStatement(sql);
        }
        public static List<IssueModel> ViewIssuesCreatedByUser(string userID)
        {
            string sql = string.Format("select * from dbo.Issue where AuthorID = '{0}';", userID);

            return SqlDataAccess.LoadData<IssueModel>(sql);
        }

        public static List<IssueModel> ViewIssuesAssignedToUser(string userID)
        {
            string sql = string.Format("select * from dbo.Issue where AssigneeID = '{0}';", userID);

            return SqlDataAccess.LoadData<IssueModel>(sql);
        }

        public static List<string> FindAllUserIDForProject(int projectID)
        {
            string sql = string.Format("select UserID from dbo.ProjectUser where ProjectID = {0};", projectID);

            return SqlDataAccess.LoadData<string>(sql);
        }

        public static IssueModel ViewIssue(int issueID)
        {
            string sql = string.Format("select * from dbo.Issue where IssueId = {0};", issueID);

            return SqlDataAccess.LoadData<IssueModel>(sql)[0];
        }

/*        public static int MarkIssueCompleted(int issueID)
        {
            string sql = string.Format("update dbo.Issues set Completed = 1 where IssueId = {0}", issueID);
            return SqlDataAccess.ExecuteStatement(sql);
        }*/

        public static int RemoveIssue(int issueID)
        {
            string sql = string.Format("delete from dbo.Issue where IssueId = {0};", issueID);

            return SqlDataAccess.ExecuteStatement(sql);
        }
        /// <summary>
        /// Add Issue to the database. It will process the data
        /// so it is inserted correctly by using the appropriate sql statement.
        /// </summary>
        /// <returns>
        /// The number of correctly inserted rows of data.
        /// </returns>
        public static int CreateIssue(string authorId, string assigneeId, string description, string name, string type,
            DateTime dateTimeCreated, DateTime dateTimedeadline, string status, string priority, int projectId)
        {
            IssueModel data = new IssueModel
            {
                AuthorID = authorId,
                AssigneeID = assigneeId,
                Description = description,
                DateTimeCreated = dateTimeCreated,
                DateTimeUpdated = dateTimeCreated,
                DateTimeDeadline = dateTimedeadline,
                Status = status,
                Priority = priority,
                ProjectID = projectId,
                Name = name,
                Type = type
            };

            string sql = @"insert into dbo.Issue (AuthorID, Description, DateTimeCreated, DateTimeDeadline, Priority, AssigneeID, ProjectID, Status, Type, Name, DateTimeUpdated)
            values (@AuthorID, @Description, @DateTimeCreated, @DateTimeDeadline, @Priority, @AssigneeID, @ProjectID, @Status, @Type, @Name, @DateTimeUpdated);";


            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<IssueModel> ViewIssuesForProject(int projectID)
        {
            string sql = string.Format("select * from dbo.Issue where ProjectID = {0};", projectID);

            return SqlDataAccess.LoadData<IssueModel>(sql);
        }

        public static int FindProjectIDThroughIssueID(int issueID)
        {
            string sql = string.Format("select distinct ProjectID from dbo.Issue where IssueId = {0};", issueID);

            return SqlDataAccess.LoadData<int>(sql)[0];
 
        }
    }
}
