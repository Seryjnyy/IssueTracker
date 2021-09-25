using IssueTracker.Models;
using IssueTrackerDataLibrary.BusinessLogic;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace IssueTracker.Controllers
{
    public class NotificationController : Controller
    {
        public List<NotificationModel> FetchAllNotifications()
        {
            string userID = User.Identity.GetUserId();
            List<NotificationModel> notifications = ConvertToModel(NotificationProcessor.AllNotificationsForUser(userID));

            return notifications;
        }

        public List<NotificationModel> FetchThreeNewestNotifcations()
        {
            string userID = User.Identity.GetUserId();
            // change the method called in processor
            List<NotificationModel> notifications = ConvertToModel(NotificationProcessor.ThreeNewestNotifications(userID));

            return notifications;
        }

        private List<NotificationModel> ConvertToModel(List<IssueTrackerDataLibrary.Models.NotificationModel> data)
        {
            List<NotificationModel> notifications = new List<NotificationModel>();

            foreach (var row in data)
            {
                notifications.Add(new NotificationModel
                {
                    Content = row.Content,
                    DateTimeCreated = row.DateTimeCreated
                });
            }

            return notifications;
        }

        // GET: Notification
        public ActionResult ViewNotifications()
        {
            return PartialView(FetchAllNotifications());
        }

        public ActionResult ViewNotificationsNavbar()
        {
            return PartialView(FetchThreeNewestNotifcations());
        }
    }
}