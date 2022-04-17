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
    [Authorize(Roles = "0")]
    public class SuppliersController : BaseController
    {
        private DataPalkia db = new DataPalkia();

        public ActionResult Index()
        {
            return View(db.Suppliers.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suppliers suppliers = db.Suppliers.Find(id);
            if (suppliers == null)
            {
                return HttpNotFound();
            }
            return View(suppliers);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SupplierID,CompanyName,ContactName,Address,Phone,Email")] Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                db.Suppliers.Add(suppliers);
                db.SaveChanges();
                TempData["Notice_Create_Success"] = true;
                return RedirectToAction("Index");
            }

            return View(suppliers);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suppliers suppliers = db.Suppliers.Find(id);
            if (suppliers == null)
            {
                return HttpNotFound();
            }
            return View(suppliers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SupplierID,CompanyName,ContactName,Address,Phone,Email")] Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(suppliers).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Notice_Save_Success"] = true;
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suppliers suppliers = db.Suppliers.Find(id);
            if (suppliers == null)
            {
                return HttpNotFound();
            }
            return View(suppliers);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Suppliers suppliers = db.Suppliers.Find(id);
                db.Suppliers.Remove(suppliers);
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
