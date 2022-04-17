using Azure_Assignment.EF;
using Scrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Azure_Assignment.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string pass)
        {
            DataPalkia db = new DataPalkia();
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
                userSession.Picture = user.Picture;
                Session.Add(Common.CommonConstants.CLIENT_SESSION, userSession);
                TempData["Notice_Login_Success"] = true;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorLogin = "Username or password incorrect";
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session[Common.CommonConstants.CLIENT_SESSION] = null;
            return RedirectToAction("Index", "Home");
        }

    }
}