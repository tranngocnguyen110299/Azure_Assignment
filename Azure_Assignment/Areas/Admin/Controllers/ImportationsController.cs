using System;
using System.Collections;
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
    public class ImportationsController : BaseController
    {
        private DataPalkia db = new DataPalkia();


        public ActionResult Index()
        {
            var importation = db.Importation.Include(i => i.Products).Include(i => i.Users);
            ViewBag.ProductList = new List<Products>(db.Products.Include(i => i.ProductImage).ToList());
            return View(importation.ToList());
        }

       
        public ActionResult Create(int? id)
        {
            var product = db.Products.FirstOrDefault(p => p.ProductID == id);
            ViewBag.Product = product;
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Import([Bind(Include = "ImportationID,ProductID,Username,ImportDate,UnitPrice,Quantity")] Importation importation)
        {
            if (ModelState.IsValid)
            {
                var product = db.Products.Single(p => p.ProductID == importation.ProductID);
                product.UnitsInStock += importation.Quantity;
                db.Importation.Add(importation);
                db.SaveChanges();
                TempData["Notice_Save_Success"] = true;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Create", new { id = importation.ProductID });
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
