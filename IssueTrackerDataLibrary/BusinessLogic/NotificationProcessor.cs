using IssueTrackerDataLibrary.DataAccess;
using IssueTrackerDataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IssueTrackerDataLibrary.BusinessLogic
{
    public static class NotificationProcessor
    {
        public static int CreateNotification(string userID, string content)
        {
            string sql = string.Format("insert into dbo.Notification (UserID, Content, DateTimeCreated) values ('{0}', '{1}', GETDATE());", userID, content);

            return SqlDataAccess.ExecuteStatement(sql);
        }

        public static List<NotificationModel> AllNotificationsForUser(string userID)
        {
            string sql = string.Format("select * from dbo.Notification where UserID = '{0}' order by DateTimeCreated desc;", userID);

            return SqlDataAccess.LoadData<NotificationModel>(sql);
        }

        public static List<NotificationModel> ThreeNewestNotifications(string userID)
        {
            string sql = string.Format("select top 3 * from dbo.Notification where UserID = '{0}' order by DateTimeCreated desc;", userID);

            return SqlDataAccess.LoadData<NotificationModel>(sql);
        }
    }
}
