using Azure_Assignment.DAO;
using Azure_Assignment.EF;
using Azure_Assignment.Providers;
using Scrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Azure_Assignment.Areas.Admin.Controllers
{
    [Authorize(Roles = "0,1")]
    public class HomeController : BaseController
    {
        private DataPalkia db = new DataPalkia();
        private ProductDAO productDAO = new ProductDAO();
        private ImageProvider imgProvider = new ImageProvider();
        private string ftpChild = "imgBlogs";
        [Authorize]
        public ActionResult Index()
        {
            long productInStock = 0;
            long productOnOrder = 0;
            long total = 0;
            long import = 0;
            long export = 0;
            
            foreach(var item in db.Products.ToList())
            {
                productInStock += item.UnitsInStock.GetValueOrDefault(0);
                productOnOrder += item.UnitsOnOrder.GetValueOrDefault(0);
            }
            var _baseInfo = new long[] { productInStock, (db.Feedbacks.Count() + db.BlogComments.Count()), db.Orders.Count(), db.Users.Count() };

            ViewBag.BaseInfo = _baseInfo;

            foreach (var item in db.OrderDetails.ToList())
            {
                total += (item.Quantity.GetValueOrDefault(0) * item.UnitPrice.GetValueOrDefault(0));
            }
            foreach (var item in db.Importation.ToList())
            {
                import += (item.Quantity.GetValueOrDefault(0) * item.UnitPrice.GetValueOrDefault(0));
            }
            foreach (var item in db.Exportation.ToList())
            {
                export += (item.Quantity.GetValueOrDefault(0) * item.UnitPrice.GetValueOrDefault(0));
            }
            var _total = new long[] { productOnOrder, total, import, export };
            ViewBag.Total = _total;

            List<int> dataCategoryChartInStock = new List<int>();
            List<int> dataCategoryChartOnOrder = new List<int>();
            List<int> dataImportationQuantity = new List<int>();
            List<int> dataExportationQuantity = new List<int>();

            foreach (var item in db.Categories.ToList())
            {
                dataCategoryChartInStock.Add(productDAO.CountProductByCategoryInStock(item.CategoryID));
                dataCategoryChartOnOrder.Add(productDAO.CountProductByCategoryOnOrder(item.CategoryID));
                dataImportationQuantity.Add(productDAO.GetImportationQuantity(item.CategoryID));
                dataExportationQuantity.Add(productDAO.GetExportationQuantity(item.CategoryID));
            }

            var labelChart = db.Categories.Select(x => x.CategoryName);
            ViewBag.LabelChartCategory = labelChart;
            ViewBag.DataCategoryChartInStock = dataCategoryChartInStock;
            ViewBag.DataCategoryChartOnOrder = dataCategoryChartOnOrder;
            ViewBag.DataImportationQuantity = dataImportationQuantity;
            ViewBag.DataExportationQuantity = dataExportationQuantity;

            ViewBag.NewFeedbacks = db.Feedbacks.OrderByDescending(i => i.FeedBackDate).Take(7).ToList();
            var blogs = db.Blogs.OrderByDescending(i => i.WritingDate).Take(4).ToList();

            foreach (var item in blogs)
            {
                item.Thumbnail = imgProvider.LoadImage(item.Thumbnail, ftpChild);
            }
            ViewBag.RecentlyAddedBlogs = blogs;

            var dataSystem = new int[] { db.Suppliers.ToList().Count(), db.Categories.ToList().Count(), db.Products.ToList().Count(), db.Sale.ToList().Count() - 1  };
            ViewBag.DataSystem = dataSystem;

            return View();
        }

        
    }
}