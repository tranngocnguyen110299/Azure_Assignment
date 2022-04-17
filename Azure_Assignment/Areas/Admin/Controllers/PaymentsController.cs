using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Azure_Assignment.EF;
using Rotativa;

namespace Azure_Assignment.Areas.Admin.Controllers
{
    [Authorize(Roles = "0")]
    public class PaymentsController : BaseController
    {
        private DataPalkia db = new DataPalkia();

        // GET: Admin/Payments
        public ActionResult Index()
        {
            return View(db.Payments.ToList());
        }

        // GET: Admin/Payments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payments payments = db.Payments.Find(id);
            if (payments == null)
            {
                return HttpNotFound();
            }
            return View(payments);
        }

        // GET: Admin/Payments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PaymentID,PaymentName")] Payments payments)
        {
            if (ModelState.IsValid)
            {
                db.Payments.Add(payments);
                db.SaveChanges();
                TempData["Notice_Create_Success"] = true;
                return RedirectToAction("Index");
            }

            return View(payments);
        }

        // GET: Admin/Payments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payments payments = db.Payments.Find(id);
            if (payments == null)
            {
                return HttpNotFound();
            }
            return View(payments);
        }

        // POST: Admin/Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PaymentID,PaymentName")] Payments payments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payments).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Notice_Save_Success"] = true;
                return RedirectToAction("Index");
            }
            return View(payments);
        }

        // GET: Admin/Payments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payments payments = db.Payments.Find(id);
            if (payments == null)
            {
                return HttpNotFound();
            }
            return View(payments);
        }

        // POST: Admin/Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Payments payments = db.Payments.Find(id);
                db.Payments.Remove(payments);
                db.SaveChanges();
                TempData["Notice_Delete_Success"] = true;
            }
            catch (Exception)
            {
                TempData["Notice_Delete_Fail"] = true;
            }
            return RedirectToAction("Index");
        }

        public ActionResult PrintAll()
        {
            var q = new ActionAsPdf("Index");
            return q;
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
