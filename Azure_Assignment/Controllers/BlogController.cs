using Azure_Assignment.DAO;
using Azure_Assignment.EF;
using Azure_Assignment.Providers;
using PagedList;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Azure_Assignment.Controllers
{
    public class BlogController : Controller
    {
        private DataPalkia db = new DataPalkia();
        private FTPServerProvider ftp = new FTPServerProvider();

        CategoryDAO categoryDAO = new CategoryDAO();
        SupplierDAO supplierDAO = new SupplierDAO();
        BlogCategoriesDAO blogCategoriesDAO = new BlogCategoriesDAO();
        BlogDAO blogDAO = new BlogDAO();
        BlogCommentDAO blogCommentDAO = new BlogCommentDAO();

        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            ViewBag.BlogCategoies = blogCategoriesDAO.getAllBlogCategories();
            var model = blogDAO.getBlog().ToPagedList(pageNumber, pageSize);
            return View(model);
        }

        public ActionResult BlogList(int? id, int? page)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Home");
            }
            if (db.BlogCategories.Find(id) == null)
            {
                return RedirectToAction("Error", "Home");
            }

            if (page == null) page = 1;
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            var model = blogDAO.getBlogOfBlogCategories(id).ToPagedList(pageNumber, pageSize);
            ViewBag.BlogCategoies = blogCategoriesDAO.getAllBlogCategories();
            ViewBag.BlogCategoryName = db.BlogCategories.Find(id).BlogCategoryName;
            ViewBag.CurrentBlogCate = id;
            return View(model);
        }

        public ActionResult BlogDetail(int? id, int? page)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Home");
            }
            if (page == null) page = 1;
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            
            ViewBag.BlogCategoies = blogCategoriesDAO.getAllBlogCategories();
            var blogDetail = db.Blogs.Find(id);
            blogDetail.Thumbnail = new ImageProvider().LoadImage(blogDetail.Thumbnail, "imgBlogs");
            ViewBag.Blog = blogDetail;

            var model = blogCommentDAO.getCommentList(id).ToPagedList(pageNumber, pageSize);
            return View(model);
        }

        [HttpPost]
        public ActionResult AddComment(string txtFullName, string txtPhone, string txtEmail, string txaComment, int? BlogID)
        {
            if (BlogID == null)
            {
                return RedirectToAction("Error", "Home");
            }

            if (txtFullName == null || txtPhone == null || txtEmail == null || txaComment == null)
            {
                ViewBag.ErrorMessage = "Please complete the form before submitting";
                return RedirectToAction("BlogDetail", "Blog", new { id = BlogID });
            }
            var cmt = new BlogComments();

            cmt.FullName = txtFullName;
            cmt.Phone = txtPhone;
            cmt.Email = txtEmail;
            cmt.Comment = txaComment;
            cmt.BlogID = BlogID;
            var blogComment = new BlogCommentDAO();
            bool isValid = false;
            try
            {
                isValid = blogComment.Insert(cmt);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Please complete the form before submitting";
                return RedirectToAction("BlogDetail", "Blog", new { id = BlogID });
            }
            
            if (isValid == true)
            {
                TempData["Notice_Submit_Success"] = true;
                return RedirectToAction("BlogDetail", "Blog", new { id = cmt.BlogID });
            }

            return RedirectToAction("BlogDetail", "Blog", new { id = cmt.BlogID });
        }
    }
}