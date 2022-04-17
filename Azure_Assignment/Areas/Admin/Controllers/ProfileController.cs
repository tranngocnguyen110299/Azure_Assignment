using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Azure_Assignment.Areas.Admin.Models;
using Azure_Assignment.EF;
using Azure_Assignment.Providers;
using Scrypt;

namespace Azure_Assignment.Areas.Admin.Controllers
{
    [Authorize(Roles = "0,1")]
    public class ProfileController : Controller
    {
        DataPalkia db = new DataPalkia();
        FTPServerProvider ftp = new FTPServerProvider();
        ImageProvider imgProvider = new ImageProvider();
        string ftpChild = "imgUsers";

        public ActionResult Index()
        {
            var user = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
            var userentity = db.Users.Find(user.Username);
            var model = new Profile();
            model.FirtName = userentity.FirtName;
            model.LastName = userentity.LastName;
            model.Gender = userentity.Gender;
            model.Birthday = userentity.Birthday;
            model.Phone = userentity.Phone;
            model.Email = userentity.Email;
            model.Address = userentity.Address;
            model.Picture = userentity.Picture;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Change(Profile profile, string imgOld_User)
        {
            if (ModelState.IsValid)
            {
                bool isAgeValid = true;
                if ((DateTime.Now.Year - profile.Birthday.Value.Year) == 16)
                {
                    if ((DateTime.Now.Month - profile.Birthday.Value.Month) == 0)
                    {
                        if ((DateTime.Now.Day - profile.Birthday.Value.Day) > 0)
                        {
                            isAgeValid = false;
                        }
                    }
                    else if ((DateTime.Now.Month - profile.Birthday.Value.Month) > 0)
                    {
                        isAgeValid = false;
                    }
                }
                else if ((DateTime.Now.Year - profile.Birthday.Value.Year) < 16)
                {
                    isAgeValid = false;
                }

                if (!isAgeValid)
                {
                    TempData["Error"] = "Age must greater than 16 years old";
                    return RedirectToAction("Index");
                }

                var user = db.Users.Find(profile.Username);

                user.FirtName = profile.FirtName;
                user.LastName = profile.LastName;
                user.Gender = profile.Gender;
                user.Birthday = profile.Birthday;
                user.Phone = profile.Phone;
                user.Email = profile.Email;
                user.Address = profile.Address;
                user.Status = user.Status;

                string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/userPictures/");
                if (profile.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(profile.ImageFile.FileName);
                    string extension = Path.GetExtension(profile.ImageFile.FileName);
                    if (imgProvider.Validate(profile.ImageFile) != null)
                    {
                        TempData["Error"] = imgProvider.Validate(profile.ImageFile);
                        return RedirectToAction("Index");
                    }
                    user.Picture = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    ftp.Update(user.Picture, ftpChild, profile.ImageFile, imgOld_User);

                }

                if (db.SaveChanges() > 0)
                {
                    TempData["Notice_Save_Success"] = true;
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
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ChangePassword(string username, string currentPass, string newPass, string confirmPass)
        {
            if (username == null || currentPass == null || newPass == null || confirmPass == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ScryptEncoder encoder = new ScryptEncoder();
            var user = db.Users.Find(username);
            bool isValidPass = encoder.Compare(currentPass, user.Password);
            if (!isValidPass)
            {
                TempData["Current_Pass_Fail"] = true;
                return RedirectToAction("Index");
            }
            if (newPass != confirmPass)
            {
                TempData["Password_Not_Match"] = true;
                return RedirectToAction("Index");
            }
            
            user.Password = encoder.Encode(newPass);
            if (db.SaveChanges() > 0)
            {
                TempData["Notice_Save_Success"] = true;
            }
            return RedirectToAction("Index");

        }




    }
}