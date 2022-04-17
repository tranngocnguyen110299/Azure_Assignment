using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Azure_Assignment.EF;
using Azure_Assignment.Providers;

namespace Azure_Assignment.Areas.Admin.Controllers
{
    [Authorize(Roles = "0,1")]
    public class OrdersController : BaseController
    {
        private DataPalkia db = new DataPalkia();

        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Payments).Include(o => o.Users);
            return View(orders.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            orders.Users.Picture = new FTPServerProvider().Get(orders.Users.Picture, "imgUsers");
            var _orderDetail = (from ord in db.OrderDetails
                                join or in db.Orders on ord.OrderID equals or.OrderID
                                join pro in db.Products on ord.ProductID equals pro.ProductID
                                where ord.OrderID == orders.OrderID
                                select ord).ToList();
            ViewBag.OrderDetail = _orderDetail;
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        public ActionResult Ship (int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var order = db.Orders.Find(id);
                order.Status = 1;
                order.ShippedDate = DateTime.Now;
                db.SaveChanges();
            }
            
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            ViewBag.PaymentID = new SelectList(db.Payments, "PaymentID", "PaymentName");
            ViewBag.Username = new SelectList(db.Users, "Username", "FirtName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,Username,PaymentID,CreationDate,ShippedDate,ShippedAddress,Note,Status")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(orders);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PaymentID = new SelectList(db.Payments, "PaymentID", "PaymentName", orders.PaymentID);
            ViewBag.Username = new SelectList(db.Users, "Username", "FirtName", orders.Username);
            return View(orders);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            ViewBag.PaymentID = new SelectList(db.Payments, "PaymentID", "PaymentName", orders.PaymentID);
            ViewBag.Username = new SelectList(db.Users, "Username", "FirtName", orders.Username);
            return View(orders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,Username,PaymentID,CreationDate,ShippedDate,ShippedAddress,Note,Status")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                var order = db.Orders.Find(orders.OrderID);
                order.ShippedAddress = orders.ShippedAddress;
                order.Note = orders.Note;
                order.PaymentID = orders.PaymentID;
                order.Status = orders.Status;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PaymentID = new SelectList(db.Payments, "PaymentID", "PaymentName", orders.PaymentID);
            ViewBag.Username = new SelectList(db.Users, "Username", "FirtName", orders.Username);
            return View(orders);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orders orders = db.Orders.Find(id);
            db.Orders.Remove(orders);
            db.SaveChanges();
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
