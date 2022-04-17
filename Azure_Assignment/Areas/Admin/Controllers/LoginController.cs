using Scrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Azure_Assignment.EF;
using Azure_Assignment.Providers;

namespace Azure_Assignment.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        DataPalkia db = new DataPalkia();
        private FTPServerProvider ftp = new FTPServerProvider();
        private string ftpChild = "imgUsers";
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string pass)
        {
            ScryptEncoder encoder = new ScryptEncoder();
            var user = db.Users.SingleOrDefault(model => model.Username == username);
            if (user == null)
            {
                ViewBag.ErrorLogin = "Username or password incorrect";
                return View();
            }

            bool isValidPass = encoder.Compare(pass, user.Password);

            if (isValidPass)
            {
                if (user.Status == false)
                {
                    ViewBag.ErrorLogin = "Your account has been locked";
                    return View();
                }
                if (user.Role == 2)
                {
                    ViewBag.ErrorLogin = "Username or password incorrect";
                    return View();
                }
                FormsAuthentication.SetAuthCookie(user.Username, false);
                
                var userSession = new Models.UserLogin();
                userSession.Username = user.Username;
                userSession.FirtName = user.FirtName;
                userSession.LastName = user.LastName;
                userSession.Gender = user.Gender;
                userSession.Birthday = user.Birthday;
                userSession.Phone = user.Phone;
                userSession.Email = user.Email;
                userSession.Address = user.Address;
                userSession.Role = user.Role;
                userSession.Status = user.Status;
                userSession.Picture = ftp.Get(user.Picture, ftpChild);
                Session.Add(Common.CommonConstants.USER_SESSION, userSession);
                TempData["Notice_Login_Success"] = true;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorLogin = "Username or password incorrect";
                return View();
            }
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Remove(Common.CommonConstants.USER_SESSION);
            return Redirect("Index");
        }
    }
}