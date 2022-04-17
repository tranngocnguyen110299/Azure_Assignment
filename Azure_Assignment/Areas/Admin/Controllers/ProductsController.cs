using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Azure_Assignment.EF;
using Azure_Assignment.Providers;

namespace Azure_Assignment.Areas.Admin.Controllers
{
    [Authorize(Roles = "0,1")]
    public class ProductsController : BaseController
    {
        private DataPalkia db = new DataPalkia();
        private FTPServerProvider ftp = new FTPServerProvider();
        private ImageProvider imgProvider = new ImageProvider();
        private string ftpChild = "imgThumbnailProducts";

        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Categories).Include(p => p.Sale).Include(p => p.Suppliers);
            
            foreach (var item in products)
            {
                //item.Thumbnail = ftp.Get(item.Thumbnail, ftpChild);
                item.Thumbnail = imgProvider.LoadImage(item.Thumbnail, ftpChild);
            }
            return View(products.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            //products.Thumbnail = ftp.Get(products.Thumbnail, ftpChild);
            products.Thumbnail = imgProvider.LoadImage(products.Thumbnail, ftpChild);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.SaleID = new SelectList(db.Sale, "SaleID", "SaleName");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,UnitPrice,OldUnitPrice,Thumbnail,ShortDescription,Description,Specification,UnitsInStock,UnitsOnOrder,SupplierID,CategoryID,SaleID,Discontinued,ImageFile")] Products products)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(products.ImageFile.FileName);
                string extension = Path.GetExtension(products.ImageFile.FileName);

                if (imgProvider.Validate(products.ImageFile) != null)
                {
                    ViewBag.Error = imgProvider.Validate(products.ImageFile);
                    return View("Create");
                }
                products.Thumbnail = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                
                db.Products.Add(products);
                products.UnitsInStock = 0;
                products.UnitsOnOrder = 0;
                if (db.SaveChanges() > 0)
                {
                    SaveImage(products.ImageFile, products.Thumbnail, ftpChild);
                    ftp.Add(products.Thumbnail, ftpChild, products.ImageFile);
                    TempData["Notice_Create_Success"] = true;
                }
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            ViewBag.SaleID = new SelectList(db.Sale, "SaleID", "SaleName");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", products.SupplierID);
            
            return View(products);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            Session["old_Thumbnail"] = products.Thumbnail;
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            ViewBag.SaleID = new SelectList(db.Sale, "SaleID", "SaleName");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", products.SupplierID);
            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,UnitPrice,OldUnitPrice,Thumbnail,ShortDescription,Description,Specification,UnitsInStock,UnitsOnOrder,SupplierID,CategoryID,SaleID,Discontinued,ImageFile")] Products products, String old_Thumbnail)
        {
            if (ModelState.IsValid)
            {
                if (products.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(products.ImageFile.FileName);
                    string extension = Path.GetExtension(products.ImageFile.FileName);

                    if (imgProvider.Validate(products.ImageFile) != null)
                    {
                        ViewBag.Error = imgProvider.Validate(products.ImageFile);
                        return View("Edit");
                    }
                    products.Thumbnail = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    DeleteImage(imgProvider.LoadImage(old_Thumbnail, ftpChild));
                    SaveImage(products.ImageFile, products.Thumbnail, ftpChild);
                    ftp.Update(products.Thumbnail, ftpChild, products.ImageFile, old_Thumbnail);
                }
                else
                {
                    products.Thumbnail = old_Thumbnail;
                }                

                db.Entry(products).State = EntityState.Modified;
                db.Entry(products).Property(x => x.UnitsInStock).IsModified = false;
                db.Entry(products).Property(x => x.UnitsOnOrder).IsModified = false;
                
                if (db.SaveChanges() > 0)
                {
                    TempData["Notice_Save_Success"] = true;
                    Session.Remove("old_Thumbnail");
                }
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            ViewBag.SaleID = new SelectList(db.Sale, "SaleID", "SaleName");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", products.SupplierID);
            return View(products);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            //products.Thumbnail = ftp.Get(products.Thumbnail, ftpChild);
            products.Thumbnail = imgProvider.LoadImage(products.Thumbnail, ftpChild);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Products products = db.Products.Find(id);
                db.Products.Remove(products);
                if (db.SaveChanges() > 0)
                {
                    DeleteImage(imgProvider.LoadImage(products.Thumbnail, ftpChild));
                    ftp.Delete(products.Thumbnail, ftpChild);
                    TempData["Notice_Delete_Success"] = true;
                }                
            }
            catch (System.Data.SqlClient.SqlException )
            {
                TempData["Notice_Delete_Fail"] = true;
            }
            catch (Exception)
            {
                TempData["Notice_Delete_Fail"] = true;
            }
            return RedirectToAction("Index");

        }

        public ActionResult ProductImage(int? id)
        {
            if (id == null || db.Products.Find(id) == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var productImage = db.ProductImage.Include(p => p.Products).Where(m => m.ProductID == id);
            foreach (var item in productImage)
            {
                //item.ImgFileName = ftp.Get(item.ImgFileName, "imgProducts");
                item.ImgFileName = imgProvider.LoadImage(item.ImgFileName, "imgProducts");
            }
            string[] proinfo = new string[] { id.ToString() , db.Products.Find(id).ProductName.ToString() };
            ViewBag.ProInfo = proinfo;

            return View(productImage.ToList());
        }

        [HttpPost]
        public ActionResult AddImage(int ProductID, HttpPostedFileBase ImageFile)
        {
            ProductImage proImage = new EF.ProductImage();
            proImage.ProductID = ProductID;

            string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
            string extension = Path.GetExtension(ImageFile.FileName);
            string ftpChild = "imgProducts";
            
            if (imgProvider.Validate(ImageFile) != null)
            {
                TempData["Error"] = imgProvider.Validate(ImageFile);
                return RedirectToAction("ProductImage", "Products", new { @id = proImage.ProductID });
            }
            proImage.ImgFileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            db.ProductImage.Add(proImage);
            if (db.SaveChanges() > 0)
            {
                SaveImage(ImageFile, proImage.ImgFileName, ftpChild);
                ftp.Add(proImage.ImgFileName, ftpChild, ImageFile);
                TempData["Notice_Save_Success"] = true;
            }
            return RedirectToAction("ProductImage", "Products", new { @id = proImage.ProductID });
        }

        [HttpPost, ActionName("DeleteImage")]
        public ActionResult DeleteImage(int proImage, int proID)
        {
            try
            {
                ProductImage productImage = db.ProductImage.Find(proImage);
                db.ProductImage.Remove(productImage);
                if (db.SaveChanges() > 0)
                {
                    DeleteImage(imgProvider.LoadImage(productImage.ImgFileName, "imgProducts"));
                    ftp.Delete(productImage.ImgFileName, "imgProducts");
                    TempData["Notice_Delete_Success"] = true;
                }
            }
            catch (Exception)
            {
                TempData["Notice_Delete_Fail"] = true;
            }

            return RedirectToAction("ProductImage", "Products", new { @id = proID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public void SaveImage(HttpPostedFileBase imageFile, string fileName, string childNode)
        {
            string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/" + childNode + "/");

            if (Directory.Exists(uploadFolderPath) == false)
            {
                Directory.CreateDirectory(uploadFolderPath);
            }

            fileName = Path.Combine(uploadFolderPath, fileName);

            imageFile.SaveAs(fileName);
        }

        public void DeleteImage(string path)
        {
            try
            {
                System.IO.File.Delete(Server.MapPath(path));
            }
            catch (Exception) { }

        }
    }
}
