using IssueTrackerDataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IssueTrackerDataLibrary.BusinessLogic
{
    public static class IssueProcessor
    {
        /// <summary>
        /// Add Issue to the database. It will process the data
        /// so it is inserted correctly by using the appropriate sql statement.
        /// </summary>
        /// <returns>
        /// The number of correctly inserted rows of data.
        /// </returns>
        public static int CreateIssue(string authorId, string assigneeId, string description,
            DateTime dateTimeCreated, DateTime dateTimedeadline, int label, int priority)
        {
            IssueModel data = new IssueModel
            {
                AuthorID = authorId,
                AssigneeID = assigneeId,
                Description = description,
                DateTimeCreated = dateTimeCreated,
                DateTimeDeadline = dateTimedeadline,
                Label = label,
                Priority = priority
            };

            string sql;
            if((data.AssigneeID == null) && (data.DateTimeDeadline == null))
            {
                sql = @"insert into dbo.Issues (AuthorID, Description, TimeCreated, Label, Priority)
            values (@AuthorID, @Description, @DateTimeCreated, @Label, @Priority);";
            }
            else if(data.DateTimeDeadline == null)
            {
                sql = @"insert into dbo.Issues (AuthorID, Description, TimeCreated, Label, Priority, AssigneeID)
            values (@AuthorID, @Description, @DateTimeCreated, @Label, @Priority, @AssigneeID);";
            }
            else if(data.AssigneeID == null)
            {
                sql = @"insert into dbo.Issues (AuthorID, Description, TimeCreated, Deadline, Label, Priority)
            values (@AuthorID, @Description, @DateTimeCreated, @DateTimeDeadline, @Label, @Priority);";
            }
            else
            {
            sql = @"insert into dbo.Issues (AuthorID, Description, TimeCreated, Deadline, Label, Priority, AssigneeID)
            values (@AuthorID, @Description, @DateTimeCreated, @DateTimeDeadline, @Label, @Priority, @AssigneeID);";
            }


            return DataAccess.SqlDataAccess.SaveData(sql, data);
        }
    }
}
