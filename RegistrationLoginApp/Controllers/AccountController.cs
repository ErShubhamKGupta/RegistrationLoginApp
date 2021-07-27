using RegistrationLoginApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RegistrationLoginApp.Controllers
{
    public class AccountController : Controller
    {
        private WebAppDBEntities dBEntities = new WebAppDBEntities();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVM model)
        {
            bool isValid = dBEntities.Users.Any(u => u.Username == model.Username && u.Password == model.Password);
            if (isValid)
            {
                FormsAuthentication.SetAuthCookie(model.Username, false);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid Username/Password!");
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public JsonResult IsUserNameAvailable(string UserName)
        {
            return Json(!dBEntities.Users.Any(u => u.Username == UserName), JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsEmailAvailable(string Email)
        {
            return Json(!dBEntities.Users.Any(u => u.Email == Email), JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsMobileAvailable(string MobileNumber)
        {
            return Json(!dBEntities.Users.Any(u => u.MobileNumber == MobileNumber), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UsersViewModel userVM)
        {
            User model = new User();
            if (ModelState.IsValid)
            {
                model.Username = userVM.Username;
                model.FirstName = userVM.FirstName;
                model.LastName = userVM.LastName;
                model.MobileNumber = userVM.MobileNumber;
                model.Password = userVM.Password;
                model.Email = userVM.Email;
                model.Gender = userVM.Gender;

                dBEntities.Users.Add(model);
                dBEntities.SaveChanges();
            }
            return RedirectToAction("Login");
        }
    }
}