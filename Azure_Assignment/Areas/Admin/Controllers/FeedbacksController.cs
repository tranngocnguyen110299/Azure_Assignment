using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Azure_Assignment.EF;

namespace Azure_Assignment.Areas.Admin.Controllers
{
    [Authorize(Roles = "0,1")]
    public class FeedbacksController : BaseController
    {
        private DataPalkia db = new DataPalkia();

        public ActionResult Index()
        {
            var feedbacks = db.Feedbacks.Include(f => f.Products);
            return View(feedbacks.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedbacks feedbacks = db.Feedbacks.Find(id);
            Products products = db.Products.Find(feedbacks.ProductID);
            ViewBag.Product = products.ProductName;
            if (feedbacks == null)
            {
                return HttpNotFound();
            }
            return View(feedbacks);
        }

        // GET: Admin/Feedbacks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedbacks feedbacks = db.Feedbacks.Find(id);
            Products products = db.Products.Find(feedbacks.ProductID);
            ViewBag.Product = products.ProductName;
            if (feedbacks == null)
            {
                return HttpNotFound();
            }
            return View(feedbacks);
        }

        // POST: Admin/Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Feedbacks feedbacks = db.Feedbacks.Find(id);
                db.Feedbacks.Remove(feedbacks);
                db.SaveChanges();
                TempData["Notice_Delete_Success"] = true;
            }
            catch (Exception)
            {
                TempData["Notice_Delete_Fail"] = true;
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
