using Azure_Assignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azure_Assignment.EF;
using Azure_Assignment.Providers;
using Scrypt;
using System.IO;

namespace Azure_Assignment.Controllers
{
    public class RegisterController : Controller
    {
        DataPalkia db = new DataPalkia();
        ImageProvider imgProvider = new ImageProvider();
        FTPServerProvider ftp = new FTPServerProvider();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserRegister users)
        {
            if (db.Users.Find(users.Username) != null)
            {
                TempData["ErrorMess"] = "Username already exists";
                return View(users);
            }

            if (users.Username.Length < 6 || users.Username.Length > 50)
            {
                TempData["ErrorMess"] = "Username must be between 6 to 50 characters.";
                return View(users);
            }

            bool isExist = db.Users.ToList().Exists(model => model.Email.Equals(users.Email, StringComparison.OrdinalIgnoreCase));
            if (isExist)
            {
                TempData["ErrorMess"] = "Email already exists";
                return View(users);
            }

            bool isAgeValid = true;
            if ((DateTime.Now.Year - users.Birthday.Value.Year) == 16)
            {
                if ((DateTime.Now.Month - users.Birthday.Value.Month) == 0)
                {
                    if ((DateTime.Now.Day - users.Birthday.Value.Day) > 0)
                    {
                        isAgeValid = false;
                    }
                }
                else if ((DateTime.Now.Month - users.Birthday.Value.Month) > 0)
                {
                    isAgeValid = false;
                }
            }
            else if ((DateTime.Now.Year - users.Birthday.Value.Year) < 16)
            {
                isAgeValid = false;
            }

            if (!isAgeValid)
            {
                TempData["ErrorMess"] = "Age must greater than 16 years old";
                return View(users);
            }

            if (users.Password.Length < 8 || users.Password.Length > 50)
            {
                TempData["ErrorMess"] = "Password must be between 8 to 50 characters";
                return View(users);
            }

            string fileName = Path.GetFileNameWithoutExtension(users.ImageFile.FileName);
            string extension = Path.GetExtension(users.ImageFile.FileName);
            if (imgProvider.Validate(users.ImageFile) != null)
            {
                TempData["ErrorMess"] = imgProvider.Validate(users.ImageFile);
                return View(users);
            }

            users.Picture = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            ScryptEncoder encoder = new ScryptEncoder();
            users.Password = encoder.Encode(users.Password);


            var custumner = new Users();
            custumner.Username = users.Username;
            custumner.Password = users.Password;
            custumner.FirtName = users.FirtName;
            custumner.LastName = users.LastName;
            custumner.Gender = users.Gender;
            custumner.Birthday = users.Birthday;
            custumner.Phone = users.Phone;
            custumner.Email = users.Email;
            custumner.Address = users.Address;
            custumner.Picture = users.Picture;
            custumner.Role = 2;
            custumner.Status = true;

            db.Users.Add(custumner);

            if (db.SaveChanges() > 0)
            {
                ftp.Add(users.Picture, "imgUsers", users.ImageFile);
                TempData["SuccessMess"] = "Register successfully";
            }
            return View(); ;


        }
    }
}