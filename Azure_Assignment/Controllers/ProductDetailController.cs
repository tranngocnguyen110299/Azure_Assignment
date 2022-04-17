using Azure_Assignment.DAO;
using Azure_Assignment.EF;
using Azure_Assignment.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Azure_Assignment.Controllers
{
    public class ProductDetailController : Controller
    {
        // GET: ProductDetail
        private DataPalkia db = new DataPalkia();
        private FTPServerProvider ftp = new FTPServerProvider();

        ProductDetailDAO productDetail = new ProductDetailDAO();
        FeedbackDAO feedback = new FeedbackDAO();
        CategoryDAO categoryDAO = new CategoryDAO();
        SupplierDAO supplierDAO = new SupplierDAO();

        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.Product = db.Products.Find(id);
                ViewBag.Image = productDetail.geAllImagesOfProduct(id).Take(4);
                ViewBag.Feedback = feedback.geAllFeedbackOfProduct(id);

            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(Feedbacks feedback)
        {
            if (ModelState.IsValid)
            {
                var feedbackDAO = new FeedbackDAO().Insert(feedback);
                if (feedbackDAO == true)
                {
                    return RedirectToAction("Index", "ProductDetail", new { id = feedback.ProductID });
                }
            }
            return RedirectToAction("Index", "ProductDetail", new { id = feedback.ProductID });
        }
    }
}