using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.WebPages;
using Azure_Assignment.EF;
using Azure_Assignment.Providers;

namespace Azure_Assignment.Areas.Admin.Controllers
{
    [Authorize(Roles = "0")]
    public class SalesController : BaseController
    {
        private DataPalkia db = new DataPalkia();
        private FTPServerProvider ftp = new FTPServerProvider();
        private ImageProvider imgProvider = new ImageProvider();
        private string ftpChild = "imgSales";

        public ActionResult Index()
        {
            var list = db.Sale.ToList();
            foreach (var item in list)
            {
                item.Picture = ftp.Get(item.Picture, ftpChild);
            }
            return View(list);
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sale.Find(id);
            sale.Picture = ftp.Get(sale.Picture, ftpChild);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SaleID,SaleName,Content,StartDate,EndDate,Picture,Code,Discount,ImageFile")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                if (db.Sale.FirstOrDefault(s => s.Code == sale.Code) != null)
                {
                    ViewBag.Error = "Code has been exist";
                    return View(sale);
                }

                if (sale.StartDate > sale.EndDate)
                {
                    ViewBag.NotiDate = "The start date must be before the end date.";
                    return View(sale);
                }
                string fileName = Path.GetFileNameWithoutExtension(sale.ImageFile.FileName);
                string extension = Path.GetExtension(sale.ImageFile.FileName);

                if (imgProvider.Validate(sale.ImageFile) != null)
                {
                    ViewBag.Error = imgProvider.Validate(sale.ImageFile);
                    return View(sale);
                }

                sale.Picture = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                db.Sale.Add(sale);
                
                if (db.SaveChanges() > 0)
                {
                    ftp.Add(sale.Picture, ftpChild, sale.ImageFile);
                    TempData["Notice_Create_Success"] = true;
                }
                return RedirectToAction("Index");
            }

            return View(sale);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sale.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            Session["OldImage"] = sale.Picture;
            return View(sale);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SaleID,SaleName,Content,StartDate,EndDate,Picture,Code,Discount,ImageFile")] Sale sale, String imageOldFile)
        {
            if (ModelState.IsValid)
            {
                if (sale.StartDate > sale.EndDate)
                {
                    ViewBag.NotiDate = "The start date must be before the end date.";
                    return View("Edit");
                }
                
                if (sale.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(sale.ImageFile.FileName);
                    string extension = Path.GetExtension(sale.ImageFile.FileName);

                    if (imgProvider.Validate(sale.ImageFile) != null)
                    {
                        ViewBag.Error = imgProvider.Validate(sale.ImageFile);
                        return View("Edit");
                    }
                    sale.Picture = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    ftp.Update(sale.Picture, ftpChild, sale.ImageFile, imageOldFile);
                }
                else
                {
                    sale.Picture = imageOldFile;
                }
                db.Entry(sale).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    Session.Remove("OldImage");
                    TempData["Notice_Save_Success"] = true;
                }
                return RedirectToAction("Index");
            }
            return View(sale);
        } 

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sale.Find(id);
            sale.Picture = ftp.Get(sale.Picture, ftpChild);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Sale sale = db.Sale.Find(id);
                db.Sale.Remove(sale);
                if (db.SaveChanges() > 0 )
                {
                    ftp.Delete(sale.Picture, ftpChild);
                    TempData["Notice_Delete_Success"] = true;
                }
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
