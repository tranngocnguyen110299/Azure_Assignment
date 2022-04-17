using Azure_Assignment.EF;
using Azure_Assignment.Providers;
using Azure_Assignment.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure_Assignment.DAO
{
    public class BlogCommentDAO
    {
        private DataPalkia db = new DataPalkia();
        private FTPServerProvider ftp = new FTPServerProvider();
        public bool Insert(BlogComments blogComment)
        {
            blogComment.CommentingDate = DateTime.Now;
            db.BlogComments.Add(blogComment);
            if (db.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        public List<BlogCommentViewModel> getCommentList(int? id)
        {
            var CommentOfBlog = (from blog in db.Blogs
                                     join blogComm in db.BlogComments on blog.BlogID equals blogComm.BlogID
                                     where blog.BlogID == id
                                     orderby blogComm.CommentingDate descending
                                     select new BlogCommentViewModel()
                                     {
                                         BlogCommentID = blogComm.BlogCommentID,
                                         FullName = blogComm.FullName,
                                         Email = blogComm.Email,
                                         Phone = blogComm.Phone,
                                         Comment = blogComm.Comment,
                                         CommentingDate = blogComm.CommentingDate,
                                         BlogID = blogComm.BlogID
                                     }).ToList();
            return CommentOfBlog;
        }

    }
}