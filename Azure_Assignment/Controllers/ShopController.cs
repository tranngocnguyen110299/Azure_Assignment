using Azure_Assignment.DAO;
using PagedList;
using System.Net;
using System.Web.Mvc;

namespace Azure_Assignment.Controllers
{
    public class ShopController : Controller
    {
        
        ProductDAO productDAO = new ProductDAO();
        CategoryDAO categoryDAO = new CategoryDAO();
        SaleDAO saleDAO = new SaleDAO();
        SupplierDAO supplierDAO = new SupplierDAO();

        public ActionResult Index(int? id, int? page, int? brand)
        {
            TempData["CurrentCategory"] = id;
            return MainView(id, page, brand);
        }

        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            ViewBag.Categories_List = categoryDAO.Get();
            ViewBag.Suppliers_List = supplierDAO.Get();
            return PartialView();
        }

        public ActionResult ShopByBrand(int? id, int? cate, int? page)
        {
            ViewBag.Cate = null;
            if (cate != null)
            {
                ViewBag.Cate = productDAO.GetCategoryName(cate);
            }

            ViewBag.Brand = null;
            if (id != null)
            {
                ViewBag.Brand = productDAO.GetBrandName(id);
            }
            if (page == null) page = 1;
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            TempData["CurrentBrand"] = id;

            if (id != null && cate != null)
            {
                var model = new ProductDAO().GetProductsByBrand_Category(id, cate).ToPagedList(pageNumber, pageSize);
                return View(model);
            }
            if (id != null)
            {
                var model = new ProductDAO().GetProductsByBrand(id).ToPagedList(pageNumber, pageSize);
                
                return View(model);
            }

            return View();
        }

        public ActionResult MainView(int? id, int? page, int? brand)
        {
            ViewBag.Cate = null;
            if (id != null)
            {
                ViewBag.Cate = productDAO.GetCategoryName(id);
            }
            
            ViewBag.Brand = null;
            if (brand != null)
            {
                ViewBag.Brand = productDAO.GetBrandName(brand);
            }
            if (page == null) page = 1;
            int pageSize = 9;
            int pageNumber = (page ?? 1);

            if (brand != null && id != null)
            {
                var model = new ProductDAO().GetProductsByCategory_Brand(id, brand).ToPagedList(pageNumber, pageSize);
                return PartialView(model);
            }

            if (id != null)
            {
                var model = new ProductDAO().GetProductsByCategory(id).ToPagedList(pageNumber, pageSize);
                return PartialView(model);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public ActionResult Search(string txtSearch, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 9;
            int pageNumber = (page ?? 1);

            if (txtSearch != null)
            {
                var model = new ProductDAO().Search(txtSearch).ToPagedList(pageNumber, pageSize);
                return View(model);
            }

            return View();
        }
    }
}