using IssueTracker.Models;
using IssueTracker.Models.ViewModel;
using IssueTrackerDataLibrary.BusinessLogic;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IssueTracker.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public ActionResult ViewDashboard()
        {
            string userID = User.Identity.GetUserId();

            DashboardViewModel model = new DashboardViewModel {
                Priority = ConvertToDataCountModel(IssueProcessor.CountAllPriorities(userID)),
                Type = ConvertToDataCountModel(IssueProcessor.CountAllTypes(userID)),
                Status = ConvertToDataCountModel(IssueProcessor.CountAllStatus(userID))
            };

            return View(model);
        }

        private DataCountModel ConvertToDataCountModel(List<IssueTrackerDataLibrary.Models.DataCountModel> data)
        {
            int listLength = data.Count();

            string[] keys = new string[listLength];
            int[] values = new int[listLength];

            for (int i = 0; i < listLength; i++)
            {
                keys[i] = data[i].Name;
                values[i] = data[i].Count;
            }

            return new DataCountModel
            {
                Name = keys,
                Count = values
            };
        }
    }
}