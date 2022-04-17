using Azure_Assignment.EF;
using Azure_Assignment.Providers;
using Azure_Assignment.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure_Assignment.DAO
{
    public class BlogDAO
    {
        private DataPalkia db = new DataPalkia();
        private FTPServerProvider ftp = new FTPServerProvider();
        private string ftpChild = "imgBlogs";


        public List<BlogViewModel> getBlog()
        {
            var list = (from blog in db.Blogs
                        orderby blog.WritingDate descending
                        select new BlogViewModel()
                        {
                            BlogID = blog.BlogID,
                            BlogName = blog.BlogName,
                            Username = blog.Username,
                            Content = blog.Content,
                            BlogCategoryID = blog.BlogCategoryID,
                            WritingDate = blog.WritingDate,
                            Thumbnail = blog.Thumbnail
                        }).ToList();
            foreach (var item in list)
            {
                //item.Thumbnail = ftp.Get(item.Thumbnail, ftpChild);
                item.Thumbnail = new ImageProvider().LoadImage(item.Thumbnail, ftpChild);
            }
            return list;
        }

        public List<BlogViewModel> getBlogOfBlogCategories(int? id)
        {
            var list = (from blogcate in db.BlogCategories
                                join blog in db.Blogs on blogcate.BlogCategoryID equals blog.BlogCategoryID
                                where blog.BlogCategoryID == id
                                select new BlogViewModel()
                                {
                                    BlogID = blog.BlogID,
                                    BlogName = blog.BlogName,
                                    Username = blog.Username,
                                    Content = blog.Content,
                                    BlogCategoryID = blog.BlogCategoryID,
                                    WritingDate = blog.WritingDate,
                                    Thumbnail = blog.Thumbnail
                                }).ToList();
            foreach (var item in list)
            {
                //item.Thumbnail = ftp.Get(item.Thumbnail, ftpChild);
                item.Thumbnail = new ImageProvider().LoadImage(item.Thumbnail, ftpChild);
            }
            return list;
        }

        public List<BlogViewModel> getBlogdetail(int? id)
        {
            var list = (from blog in db.Blogs
                        where blog.BlogID == id
                        select new BlogViewModel()
                        {
                            BlogID = blog.BlogID,
                            BlogName = blog.BlogName,
                            Username = blog.Username,
                            Content = blog.Content,
                            BlogCategoryID = blog.BlogCategoryID,
                            WritingDate = blog.WritingDate,
                            Thumbnail = blog.Thumbnail
                        }).ToList();
            foreach (var item in list)
            {
                //item.Thumbnail = ftp.Get(item.Thumbnail, ftpChild);
                item.Thumbnail = new ImageProvider().LoadImage(item.Thumbnail, ftpChild);
            }
            return list;
        }
    }
}