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
    public class BlogCategoriesController : BaseController
    {
        private DataPalkia db = new DataPalkia();

        public ActionResult Index()
        {
            return View(db.BlogCategories.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogCategories blogCategories = db.BlogCategories.Find(id);
            if (blogCategories == null)
            {
                return HttpNotFound();
            }
            return View(blogCategories);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BlogCategoryID,BlogCategoryName")] BlogCategories blogCategories)
        {
            if (ModelState.IsValid)
            {
                db.BlogCategories.Add(blogCategories);
                db.SaveChanges();
                TempData["Notice_Create_Success"] = true;
                return RedirectToAction("Index");
            }

            return View(blogCategories);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogCategories blogCategories = db.BlogCategories.Find(id);
            if (blogCategories == null)
            {
                return HttpNotFound();
            }
            return View(blogCategories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BlogCategoryID,BlogCategoryName")] BlogCategories blogCategories)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blogCategories).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Notice_Save_Success"] = true;
                return RedirectToAction("Index");
            }
            return View(blogCategories);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogCategories blogCategories = db.BlogCategories.Find(id);
            if (blogCategories == null)
            {
                return HttpNotFound();
            }
            return View(blogCategories);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                BlogCategories blogCategories = db.BlogCategories.Find(id);
                db.BlogCategories.Remove(blogCategories);
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
