using System;
using System.Collections.Generic;
using System.Text;
using IssueTrackerDataLibrary.Models;
using IssueTrackerDataLibrary.DataAccess;

namespace IssueTrackerDataLibrary.BusinessLogic
{
    public static class ActivityProcessor
    {
        public static int CreateActivity(string userID, int projectID, DateTime dateTimeCreated, string activityContent)
        {
            string sql = @"insert into dbo.Activity (UserID, ProjectID, DateTimeCreated, ActivityContent) values (@UserID, @ProjectId, @DateTimeCreated, @ActivityContent);";

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
            string sql = string.Format("select * from dbo.Activity where ProjectID = {0}", projectID);

            return SqlDataAccess.LoadData<ActivityModel>(sql);
        }
    }
}
