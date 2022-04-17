using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Azure_Assignment.EF;
using Scrypt;
using Azure_Assignment.Providers;

namespace Azure_Assignment.Areas.Admin.Controllers
{
    [Authorize(Roles = "0")]
    public class UsersController : BaseController
    {
        private DataPalkia db = new DataPalkia();
        private FTPServerProvider ftp = new FTPServerProvider();
        private ImageProvider imgProvider = new ImageProvider();
        private string ftpChild = "imgUsers";

        public ActionResult Index()
        {
            var list = db.Users.ToList();
            foreach (var item in list)
            {
                item.Picture = ftp.Get(item.Picture, ftpChild);
            }
            return View(list);
        }


        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            users.Picture = ftp.Get(users.Picture, ftpChild);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,FirtName,LastName,Password,Gender,Birthday,Phone,Email,Address,Picture,Role,Status,ImageFile")] Users users)
        {
            
            if (ModelState.IsValid)
            {
                
                if (db.Users.Find(users.Username) != null)
                {
                    ViewBag.Error = "Username already exists";
                    return View("Create");
                }

                if (users.Username.Length < 6 || users.Username.Length > 50)
                {
                    ViewBag.Error = "Username must be between 6 to 50 characters.";
                    return View("Create");
                }

                bool isExist = db.Users.ToList().Exists(model => model.Email.Equals(users.Email, StringComparison.OrdinalIgnoreCase));
                if (isExist)
                {
                    ViewBag.Error = "Email already exists";
                    return View("Create");
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
                    ViewBag.Error = "Age must greater than 16 years old";
                    return View("Create");
                }

                if (users.Password.Length < 8 || users.Password.Length > 50)
                {
                    ViewBag.Error = "Password must be between 8 to 50 characters";
                    return View("Create");
                }

                string fileName = Path.GetFileNameWithoutExtension(users.ImageFile.FileName);
                string extension = Path.GetExtension(users.ImageFile.FileName);
                if (imgProvider.Validate(users.ImageFile) != null)
                {
                    ViewBag.Error = imgProvider.Validate(users.ImageFile);
                    return View("Create");
                }

                users.Picture = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                ScryptEncoder encoder = new ScryptEncoder();
                users.Password = encoder.Encode(users.Password);

                db.Users.Add(users);
                
                if (db.SaveChanges() > 0)
                {
                    ftp.Add(users.Picture, ftpChild, users.ImageFile);
                    TempData["Notice_Create_Success"] = true;
                }
                return RedirectToAction("Index");
            }

            return View(users);
        }


        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            Session["OldImage_User"] = users.Picture;
            return View(users);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Username,Password,FirtName,LastName,Gender,Birthday,Phone,Email,Address,Picture,Role,Status,ImageFile")] Users users, String imageOldFile_User)
        {
            if (ModelState.IsValid)
            {
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
                    ViewBag.Error = "Age must greater than 16 years old";
                    return View("Edit");
                }

                string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/userPictures/");
                if (users.ImageFile == null)
                {
                    users.Picture = imageOldFile_User;
                }
                else
                {
                    string fileName = Path.GetFileNameWithoutExtension(users.ImageFile.FileName);
                    string extension = Path.GetExtension(users.ImageFile.FileName);
                    if (imgProvider.Validate(users.ImageFile) != null)
                    {
                        ViewBag.Error = imgProvider.Validate(users.ImageFile);
                        return View("Create");
                    }
                    users.Picture = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    ftp.Update(users.Picture, ftpChild, users.ImageFile, imageOldFile_User);

                }
                db.Entry(users).State = EntityState.Modified;
                db.Entry(users).Property(x => x.Password).IsModified = false;
                
                if (db.SaveChanges() > 0)
                {
                    Session.Remove("OldImage_User");
                    TempData["Notice_Save_Success"] = true;
                }
                return RedirectToAction("Index");
            }
            return View(users);
        }


        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            users.Picture = ftp.Get(users.Picture, ftpChild);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                Users users = db.Users.Find(id);
                db.Users.Remove(users);
                if (db.SaveChanges() > 0)
                {
                    ftp.Delete(users.Picture, ftpChild);
                    TempData["Notice_Delete_Success"] = true;
                }
            }
            catch (Exception)
            {
                TempData["Notice_Delete_Fail"] = true;
            }
            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
