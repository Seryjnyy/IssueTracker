using System;
using System.Collections.Generic;
using System.Text;
using IssueTrackerDataLibrary.Models;
using IssueTrackerDataLibrary.DataAccess;

namespace IssueTrackerDataLibrary.BusinessLogic
{
    public static class ProjectUserProcessor
    {
        public static List<ProjectUserModel> ViewProjectUsers(int projectID)
        {
            string sql = string.Format("select * from dbo.ProjectUser where ProjectID = {0}", projectID);

            return SqlDataAccess.LoadData<ProjectUserModel>(sql);
        }
    }
}
