using Azure_Assignment.EF;
using Azure_Assignment.Providers;
using Azure_Assignment.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure_Assignment.DAO
{
    public class ProductDetailDAO
    {
        private DataPalkia db = new DataPalkia();
        private FTPServerProvider ftp = new FTPServerProvider();
        private string ftpChild = "imgProducts";

        public List<ProductDetailViewModel> geAllImagesOfProduct(int? id)
        {
            var productImage = (from pro in db.Products
                                from ProImg in pro.ProductImage
                                where pro.UnitsInStock > 0 && pro.ProductID == id
                                select new ProductDetailViewModel()
                                {
                                    ImgID = ProImg.ImgID,
                                    ImgFileName = ProImg.ImgFileName
                                }).ToList();
            foreach (var item in productImage)
            {
                //item.ImgFileName = ftp.Get(item.ImgFileName, ftpChild);
                item.ImgFileName = new ImageProvider().LoadImage(item.ImgFileName, ftpChild);
            }
            return productImage;
        }
    }
}