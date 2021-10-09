using IssueTrackerDataLibrary.Models;
using IssueTrackerDataLibrary.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace IssueTrackerDataLibrary.BusinessLogic
{
    public static class IssueProcessor
    {
        // functions for chart visualisations

        // these count the clsoed issues too, might need to be changed
        public static List<DataCountModel> CountAllPriorities(string userID)
        {
            string sql = string.Format("select Priority as Name, count(Priority) as Count from dbo.Issue group by Priority;", userID);
            return SqlDataAccess.LoadData<DataCountModel>(sql);
        }

        // these count the clsoed issues too, might need to be changed
        public static List<DataCountModel> CountAllTypes(string userID)
        {
            string sql = string.Format("select Type as Name, count(Type) as Count from dbo.Issue group by Type;", userID);
            return SqlDataAccess.LoadData<DataCountModel>(sql);
        }

        public static List<DataCountModel> CountAllStatus(string userID)
        {
            string sql = string.Format("select Status as Name, count(Status) as Count from dbo.Issue group by Status;", userID);
            return SqlDataAccess.LoadData<DataCountModel>(sql);
        }


        // functions for chart visualisations

        public static int UpdateIssue(int issueID, string description, DateTime dateTimeDeadline, string priority, string type, string status, string assigneeID)
        {
            IssueModel data = new IssueModel
            {
                IssueId = issueID,
                Description = description,
                DateTimeDeadline = dateTimeDeadline,
                DateTimeUpdated = DateTime.Now,
                Priority = priority,
                Type = type,
                Status = status,
                AssigneeID = assigneeID
            };

            string sql = @"update dbo.Issue set Description = @Description, DateTimeDeadline = @DateTimeDeadline, Priority = @Priority, Type = @Type, Status = @Status, AssigneeID = @AssigneeID, DateTimeUpdated = @DateTimeUpdated where IssueId = @IssueID;";
            return SqlDataAccess.SaveData(sql, data);        
        }

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
