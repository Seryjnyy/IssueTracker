using System;
using System.Collections.Generic;
using System.Text;
using IssueTrackerDataLibrary.Models;
using IssueTrackerDataLibrary.DataAccess;

namespace IssueTrackerDataLibrary.BusinessLogic
{
    public static class CommentProcessor
    {
        public static List<CommentModel> ViewCommentsForIssue(int issueID)
        {
            string sql = string.Format("select * from dbo.Comment where IssueID = {0};", issueID);
            return SqlDataAccess.LoadData<CommentModel>(sql);
        }

        public static int CreateCommentForIssue(string authorId, DateTime dateTimeCreated, string content, int issueID)
        {

            string sql = @"insert into dbo.Comment (AuthorID, DateTimeCreated, Content, IssueID) 
                            values (@AuthorID, @DateTimeCreated, @Content, @IssueID);";

            CommentModel data = new CommentModel
            {
                AuthorID = authorId,
                DateTimeCreated = dateTimeCreated,
                Content = content,
                IssueID = issueID
            };

            return SqlDataAccess.SaveData(sql, data);
        }

        public static int RemoveComment(int commentID)
        {
            string sql = string.Format("delete from dbo.Comment where CommentID = {0};", commentID);

            return SqlDataAccess.ExecuteStatement(sql);
        }
    }
}
