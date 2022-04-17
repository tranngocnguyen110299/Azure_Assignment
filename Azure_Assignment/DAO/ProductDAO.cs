using Azure_Assignment.EF;
using Azure_Assignment.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Azure_Assignment.Providers;

namespace Azure_Assignment.DAO
{
    public class ProductDAO
    {
        private DataPalkia db = new DataPalkia();
        private FTPServerProvider ftp = new FTPServerProvider();
        private string ftpChild = "imgThumbnailProducts";

        public List<ProductViewModel> GetProduct()
        {
            var list = (from pro in db.Products
                          join cate in db.Categories on pro.CategoryID equals cate.CategoryID
                          where pro.UnitsInStock > 0 
                          select new ProductViewModel()
                          {
                              ProductID = pro.ProductID,
                              ProductName = pro.ProductName,
                              UnitPrice = pro.UnitPrice,
                              OldUnitPrice = pro.OldUnitPrice,
                              CategoryID = cate.CategoryID,
                              CategoryName = cate.CategoryName,
                              Thumbnail = pro.Thumbnail
                          }).ToList();

            foreach (var item in list)
            {
                //item.Thumbnail = ftp.Get(item.Thumbnail, ftpChild);
                item.Thumbnail = new ImageProvider().LoadImage(item.Thumbnail, ftpChild);
            }

            return list;
        }

        public List<ProductViewModel> GetSaleProduct()
        {
            var list = (from pro in db.Products
                          join sale in db.Sale on pro.SaleID equals sale.SaleID
                          join cate in db.Categories on pro.CategoryID equals cate.CategoryID
                          where pro.UnitsInStock > 0 && sale.SaleName != "No Sale"
                          orderby sale.SaleID descending
                          select new ProductViewModel()
                          {
                              ProductID = pro.ProductID,
                              ProductName = pro.ProductName,
                              UnitPrice = pro.UnitPrice,
                              OldUnitPrice = pro.OldUnitPrice,
                              SaleID = sale.SaleID,
                              SaleName = sale.SaleName,
                              CategoryID = cate.CategoryID,
                              CategoryName = cate.CategoryName,
                              Thumbnail = pro.Thumbnail
                          }).ToList();

            foreach (var item in list)
            {
                //item.Thumbnail = ftp.Get(item.Thumbnail, ftpChild);
                item.Thumbnail = new ImageProvider().LoadImage(item.Thumbnail, ftpChild);
            }
            return list;
        }

        public List<ProductViewModel> GetBestSellerProduct()
        {
            var list = (from pro in db.Products
                          join sale in db.Sale on pro.SaleID equals sale.SaleID
                          join cate in db.Categories on pro.CategoryID equals cate.CategoryID
                          where pro.UnitsInStock > 0
                          orderby pro.UnitsOnOrder descending
                          select new ProductViewModel()
                          {
                              ProductID = pro.ProductID,
                              ProductName = pro.ProductName,
                              UnitPrice = pro.UnitPrice,
                              OldUnitPrice = pro.OldUnitPrice,
                              SaleID = sale.SaleID,
                              SaleName = sale.SaleName,
                              CategoryID = cate.CategoryID,
                              CategoryName = cate.CategoryName,
                              Thumbnail = pro.Thumbnail
                          }).ToList();
            foreach (var item in list)
            {
                //item.Thumbnail = ftp.Get(item.Thumbnail, ftpChild);
                item.Thumbnail = new ImageProvider().LoadImage(item.Thumbnail, ftpChild);
            }
            return list;
        }

        public List<ProductViewModel> GetHighlightProducts()
        {
            var list = (from pro in db.Products
                          join sale in db.Sale on pro.SaleID equals sale.SaleID
                          join cate in db.Categories on pro.CategoryID equals cate.CategoryID
                          where pro.UnitsInStock > 0 && sale.SaleName != "No Sale" 
                          orderby pro.UnitsOnOrder, pro.UnitPrice descending
                          select new ProductViewModel()
                          {
                              ProductID = pro.ProductID,
                              ProductName = pro.ProductName,
                              UnitPrice = pro.UnitPrice,
                              OldUnitPrice = pro.OldUnitPrice,
                              SaleID = sale.SaleID,
                              SaleName = sale.SaleName,
                              CategoryID = cate.CategoryID,
                              CategoryName = cate.CategoryName,
                              Thumbnail = pro.Thumbnail
                          }).ToList();
            foreach (var item in list)
            {
                //item.Thumbnail = ftp.Get(item.Thumbnail, ftpChild);
                item.Thumbnail = new ImageProvider().LoadImage(item.Thumbnail, ftpChild);
            }
            return list;
        }

        public List<ProductViewModel> GetProductsByCategory(int? CateID)
        {
            var list = (from pro in db.Products
                        join cate in db.Categories on pro.CategoryID equals cate.CategoryID
                        join sale in db.Sale on pro.SaleID equals sale.SaleID
                        where pro.UnitsInStock > 0 && cate.CategoryID == CateID
                        select new ProductViewModel
                        {
                            ProductID = pro.ProductID,
                            ProductName = pro.ProductName,
                            UnitPrice = pro.UnitPrice,
                            OldUnitPrice = pro.OldUnitPrice,
                            SaleID = sale.SaleID,
                            SaleName = sale.SaleName,
                            CategoryID = cate.CategoryID,
                            CategoryName = cate.CategoryName,
                            Thumbnail = pro.Thumbnail
                        }).ToList();
            foreach (var item in list)
            {
                // item.Thumbnail = ftp.Get(item.Thumbnail, ftpChild);
                item.Thumbnail = new ImageProvider().LoadImage(item.Thumbnail, ftpChild);
            }
            return list;
        }

        public string GetCategoryName(int? id)
        {
            var name = (db.Categories.Find(id).CategoryName).ToString();
            return name;
        }

        public string GetBrandName(int? id)
        {
            var name = (db.Suppliers.Find(id).CompanyName).ToString();
            return name;
        }

        public List<ProductViewModel> GetProductsByCategory_Price(int? CateID,int? min, int? max)
        {
            var list = (from pro in db.Products
                        join cate in db.Categories on pro.CategoryID equals cate.CategoryID
                        join sale in db.Sale on pro.SaleID equals sale.SaleID
                        where pro.UnitsInStock > 0 && cate.CategoryID == CateID && pro.UnitPrice > min && pro.UnitPrice < max
                        select new ProductViewModel
                        {
                            ProductID = pro.ProductID,
                            ProductName = pro.ProductName,
                            UnitPrice = pro.UnitPrice,
                            OldUnitPrice = pro.OldUnitPrice,
                            SaleID = sale.SaleID,
                            SaleName = sale.SaleName,
                            CategoryID = cate.CategoryID,
                            CategoryName = cate.CategoryName,
                            Thumbnail = pro.Thumbnail
                        }).ToList();
            foreach (var item in list)
            {
                //item.Thumbnail = ftp.Get(item.Thumbnail, ftpChild);
                item.Thumbnail = new ImageProvider().LoadImage(item.Thumbnail, ftpChild);
            }
            return list;
        }

        public List<ProductViewModel> GetProductsByCategory_Brand(int? CateID, int? BrandID)
        {
            var list = (from pro in db.Products
                        join cate in db.Categories on pro.CategoryID equals cate.CategoryID
                        join sale in db.Sale on pro.SaleID equals sale.SaleID
                        where pro.UnitsInStock > 0 && cate.CategoryID == CateID && pro.SupplierID == BrandID
                        select new ProductViewModel
                        {
                            ProductID = pro.ProductID,
                            ProductName = pro.ProductName,
                            UnitPrice = pro.UnitPrice,
                            OldUnitPrice = pro.OldUnitPrice,
                            SaleID = sale.SaleID,
                            SaleName = sale.SaleName,
                            CategoryID = cate.CategoryID,
                            CategoryName = cate.CategoryName,
                            Thumbnail = pro.Thumbnail
                        }).ToList();
            foreach (var item in list)
            {
                //item.Thumbnail = ftp.Get(item.Thumbnail, ftpChild);
                item.Thumbnail = new ImageProvider().LoadImage(item.Thumbnail, ftpChild);
            }
            return list;
        }

        public List<ProductViewModel> GetProductsByBrand(int? brand)
        {
            var list = (from pro in db.Products
                        join cate in db.Categories on pro.CategoryID equals cate.CategoryID
                        join sale in db.Sale on pro.SaleID equals sale.SaleID
                        where pro.UnitsInStock > 0 && pro.SupplierID == brand
                        select new ProductViewModel
                        {
                            ProductID = pro.ProductID,
                            ProductName = pro.ProductName,
                            UnitPrice = pro.UnitPrice,
                            OldUnitPrice = pro.OldUnitPrice,
                            SaleID = sale.SaleID,
                            SaleName = sale.SaleName,
                            CategoryID = cate.CategoryID,
                            CategoryName = cate.CategoryName,
                            Thumbnail = pro.Thumbnail
                        }).ToList();
            foreach (var item in list)
            {
                //item.Thumbnail = ftp.Get(item.Thumbnail, ftpChild);
                item.Thumbnail = new ImageProvider().LoadImage(item.Thumbnail, ftpChild);
            }
            return list;
        }

        public List<ProductViewModel> Search(string key)
        {
            var list = (from pro in db.Products
                        join cate in db.Categories on pro.CategoryID equals cate.CategoryID
                        join sale in db.Sale on pro.SaleID equals sale.SaleID
                        where pro.UnitsInStock > 0 && pro.ProductName.Contains(key)
                        select new ProductViewModel
                        {
                            ProductID = pro.ProductID,
                            ProductName = pro.ProductName,
                            UnitPrice = pro.UnitPrice,
                            OldUnitPrice = pro.OldUnitPrice,
                            SaleID = sale.SaleID,
                            SaleName = sale.SaleName,
                            CategoryID = cate.CategoryID,
                            CategoryName = cate.CategoryName,
                            Thumbnail = pro.Thumbnail
                        }).ToList();
            foreach (var item in list)
            {
                //item.Thumbnail = ftp.Get(item.Thumbnail, ftpChild);
                item.Thumbnail = new ImageProvider().LoadImage(item.Thumbnail, ftpChild);
            }
            return list;
        }

        public List<ProductViewModel> GetProductsByBrand_Category(int? supID, int? cateID)
        {
            var list = (from pro in db.Products
                        join cate in db.Categories on pro.CategoryID equals cate.CategoryID
                        join sale in db.Sale on pro.SaleID equals sale.SaleID
                        where pro.UnitsInStock > 0 && pro.SupplierID == supID && cate.CategoryID == cateID
                        select new ProductViewModel
                        {
                            ProductID = pro.ProductID,
                            ProductName = pro.ProductName,
                            UnitPrice = pro.UnitPrice,
                            OldUnitPrice = pro.OldUnitPrice,
                            SaleID = sale.SaleID,
                            SaleName = sale.SaleName,
                            CategoryID = cate.CategoryID,
                            CategoryName = cate.CategoryName,
                            Thumbnail = pro.Thumbnail
                        }).ToList();
            foreach (var item in list)
            {
                //item.Thumbnail = ftp.Get(item.Thumbnail, ftpChild);
                item.Thumbnail = new ImageProvider().LoadImage(item.Thumbnail, ftpChild);
            }
            return list;
        }

        public Products ViewDetail(int id)
        {
            var product = db.Products.SingleOrDefault(p => p.ProductID == id);

            //product.Thumbnail = ftp.Get(product.Thumbnail, ftpChild);
            product.Thumbnail = new ImageProvider().LoadImage(product.Thumbnail, ftpChild);

            return product;
        }

        public bool UpdateUnitOnOder(int productID, int quantity)
        {
            var product = db.Products.Find(productID);
            product.UnitsOnOrder += quantity;
            if (db.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int CountProductByCategoryInStock(int cateID)
        {
            int num = 0;
            foreach(var item in db.Products.Where(pro => pro.CategoryID == cateID))
            {
                num += item.UnitsInStock.GetValueOrDefault(0);
            }
            return num;
        }

        public int CountProductByCategoryOnOrder(int cateID)
        {
            int num = 0;
            foreach (var item in db.Products.Where(pro => pro.CategoryID == cateID))
            {
                num += item.UnitsOnOrder.GetValueOrDefault(0);
            }
            return num;
        }

        public int GetImportationQuantity(int cateID)
        {
            int num = 0;
            foreach (var item in db.Importation.Where(pro => pro.Products.CategoryID == cateID))
            {
                num += item.Quantity.GetValueOrDefault(0);
            }
            return num;
        }

        public int GetExportationQuantity(int cateID)
        {
            int num = 0;
            foreach (var item in db.Exportation.Where(pro => pro.Products.CategoryID == cateID))
            {
                num += item.Quantity.GetValueOrDefault(0);
            }
            return num;
        }

        public bool ValidateCheckOut(int productID, int quantity)
        {
            var inStock = db.Products.Find(productID).UnitsInStock;
            if (inStock >= quantity)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}