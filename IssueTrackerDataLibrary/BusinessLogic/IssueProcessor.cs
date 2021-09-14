using IssueTrackerDataLibrary.Models;
using IssueTrackerDataLibrary.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace IssueTrackerDataLibrary.BusinessLogic
{
    public static class IssueProcessor
    {
        public static int MarkIssueCompleted(int issueID)
        {
            string sql = string.Format("update dbo.Issues set Completed = 1 where IssueId = {0}", issueID);
            return SqlDataAccess.ExecuteStatement(sql);
        }

        public static int RemoveIssue(int issueID)
        {
            string sql = string.Format("delete from dbo.Issues where IssueId = {0};", issueID);

            return SqlDataAccess.ExecuteStatement(sql);
        }
        /// <summary>
        /// Add Issue to the database. It will process the data
        /// so it is inserted correctly by using the appropriate sql statement.
        /// </summary>
        /// <returns>
        /// The number of correctly inserted rows of data.
        /// </returns>
        public static int CreateIssue(string authorId, string assigneeId, string description,
            DateTime dateTimeCreated, DateTime dateTimedeadline, int label, int priority, int projectId)
        {
            IssueModel data = new IssueModel
            {
                AuthorID = authorId,
                AssigneeID = assigneeId,
                Description = description,
                DateTimeCreated = dateTimeCreated,
                DateTimeDeadline = dateTimedeadline,
                Label = label,
                Priority = priority,
                ProjectID = projectId
            };

            string sql = @"insert into dbo.Issues (AuthorID, Description, DateTimeCreated, DateTimeDeadline, Label, Priority, AssigneeID, ProjectID)
            values (@AuthorID, @Description, @DateTimeCreated, @DateTimeDeadline, @Label, @Priority, @AssigneeID, @ProjectID);";


            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<IssueModel> ViewIssuesForProject(int projectID)
        {
            string sql = string.Format("select * from dbo.Issues where ProjectID = {0};", projectID);

            return SqlDataAccess.LoadData<IssueModel>(sql);
        }

        public static int FindProjectIDThroughIssueID(int issueID)
        {
            string sql = string.Format("select distinct ProjectID from dbo.Issues where IssueId = {0};", issueID);

            return SqlDataAccess.LoadData<int>(sql)[0];
 
        }
    }
}
