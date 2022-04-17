using Azure_Assignment.EF;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Azure_Assignment.DAO;
using Azure_Assignment.Providers;
using PagedList;
using Azure_Assignment.Models;
using System.Collections.Generic;
using System.IO;
using System;
using Azure_Assignment.ViewModels;
using Scrypt;

namespace Azure_Assignment.Controllers
{
    public class HomeController : Controller
    {
        private DataPalkia db = new DataPalkia();
        private FTPServerProvider ftp = new FTPServerProvider();
        ProductDAO productDAO = new ProductDAO();
        CategoryDAO categoryDAO = new CategoryDAO();
        SaleDAO saleDAO = new SaleDAO();
        SupplierDAO supplierDAO = new SupplierDAO();

        public ActionResult Index()
        {
            ViewBag.One_New_Category = categoryDAO.GetNewCategories().Take(1);
            ViewBag.Four_Categories = categoryDAO.Get().Take(4);
            ViewBag.Five_Category_For_Filter = categoryDAO.Get().Take(5);
            ViewBag.Eight_New_Product = productDAO.GetProduct().Take(8);
            ViewBag.Sale = saleDAO.Get().Take(3);
            ViewBag.Best_Seller = productDAO.GetBestSellerProduct().Take(3);
            ViewBag.Sale_Product = productDAO.GetSaleProduct().Take(3);
            ViewBag.Highlight_Product = productDAO.GetHighlightProducts().Take(3);
            ViewBag.Discount = saleDAO.Get().Take(1);
            ViewBag.StartEndDate = saleDAO.Get().Take(1);

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult Navigation()
        {
            ViewBag.Layout_Menu = categoryDAO.Get().Take(2);
            ViewBag.Categories_List = categoryDAO.Get();
            ViewBag.Suppliers_List = supplierDAO.Get();
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult Header()
        {
            var cart = (List<CartItem>)Session[Common.CommonConstants.CartSession];
            var qty = 0;
            if (cart != null)
            {
                foreach (var item in cart)
                {
                    qty += item.Quantity;
                }
            }
            ViewBag.QuantityInCart = qty;
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult Footer()
        {
            return PartialView();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult UploadTooBig()
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest); 
        }
    }
}