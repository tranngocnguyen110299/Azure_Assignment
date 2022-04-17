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
    public class ExportationsController : BaseController
    {
        private DataPalkia db = new DataPalkia();

        
        public ActionResult Index()
        {
            var exportation = db.Exportation.Include(e => e.Products).Include(e => e.Users);
            ViewBag.ProductList = new List<Products>(db.Products.Include(i => i.ProductImage).ToList());
            return View(exportation.ToList());
        }

        
        public ActionResult Create(int? id)
        {
            var product = db.Products.FirstOrDefault(p => p.ProductID == id);
            ViewBag.Product = product;
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Export([Bind(Include = "ExportationID,ProductID,Username,ExportDate,UnitPrice,Quantity")] Exportation exportation)
        {
            if (ModelState.IsValid)
            {
                var product = db.Products.Single(p => p.ProductID == exportation.ProductID);
                if (exportation.Quantity > product.UnitsInStock)
                {
                    TempData["Error"] = "Quantity must be less than unit in stock";
                    return RedirectToAction("Create", new { id = product.ProductID });
                }
                else
                {
                    product.UnitsInStock -= exportation.Quantity;
                    db.Exportation.Add(exportation);
                    db.SaveChanges();
                    TempData["Notice_Save_Success"] = true;

                    return RedirectToAction("Index");
                }
                
            }

            return RedirectToAction("Create", new { id = exportation.ProductID });
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
