using System;
using System.Collections.Generic;
using System.Text;
using IssueTrackerDataLibrary.Models;
using IssueTrackerDataLibrary.DataAccess;

namespace IssueTrackerDataLibrary.BusinessLogic
{
    public static class ActivityProcessor
    {
        public static int CreateProjectActivity(string userID, int projectID, DateTime dateTimeCreated, string activityContent)
        {
            string sql = @"insert into dbo.ProjectActivity (UserID, ProjectID, DateTimeCreated, ActivityContent) values (@UserID, @ProjectId, @DateTimeCreated, @ActivityContent);";

            ActivityModel data = new ActivityModel
            {
                UserID = userID,
                ProjectID = projectID,
                DateTimeCreated = dateTimeCreated,
                ActivityContent = activityContent
            };

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<ActivityModel> ViewActivityForProject(int projectID)
        {
            string sql = string.Format("select * from dbo.ProjectActivity where ProjectID = {0} ORDER BY DateTimeCreated DESC;", projectID);

            return SqlDataAccess.LoadData<ActivityModel>(sql);
        }

        public static List<ActivityModel> ViewAllUserActivity(string userID)
        {
            string sql = string.Format("select ActivityContent, DateTimeCreated from dbo.IssueActivity where UserID = '{0}' "
                                       + "union select ActivityContent, DateTimeCreated from dbo.ProjectActivity where UserID = '{0}' ORDER BY DateTimeCreated DESC;", userID);

            return SqlDataAccess.LoadData<ActivityModel>(sql);
        }

        public static int CreateIssueActivity(string userID, int issueID, DateTime dateTimeCreated, string activityContent)
        {
            string sql = @"insert into dbo.IssueActivity (UserID, IssueID, DateTimeCreated, ActivityContent) values (@UserID, @IssueID, @DateTimeCreated, @ActivityContent);";

            ActivityModel data = new ActivityModel
            {
                UserID = userID,
                IssueID = issueID,
                DateTimeCreated = dateTimeCreated,
                ActivityContent = activityContent
            };

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<ActivityModel> ViewActivityForIssue(int issueID)
        {
            string sql = string.Format("select * from dbo.IssueActivity where IssueID = {0} ORDER BY DateTimeCreated DESC;", issueID);
            var data = SqlDataAccess.LoadData<ActivityModel>(sql);
            return data;
        }
    }
}
