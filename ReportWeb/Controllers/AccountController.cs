using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportWeb.BLL;
using ReportWeb.Data;
using System.Web.Security;

namespace ReportWeb.Controllers
{
    public class AccountController : ControllerBase
    {
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Models.LoginModel model)
        {
            if (ModelState.IsValid)
            {
                SecurityBLL security = new SecurityBLL();
                string token = security.VerifyUser(model.UserId.ToUpper().Trim(), model.Password.ToUpper().Trim(), ClientIPAddress);

                if (string.IsNullOrWhiteSpace(token))
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                    return View(model);
                }

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                           1,
                           token,
                           DateTime.Now,
                           DateTime.Now.AddMinutes(30),
                           false,
                           "User"
                     );

                string formsCookieStr = FormsAuthentication.Encrypt(ticket);
                HttpCookie formsCookie = new HttpCookie(FormsAuthentication.FormsCookieName, formsCookieStr);
                HttpContext.Response.Cookies.Add(formsCookie);
                return RedirectToAction("Index", "Home");

            }
            ModelState.AddModelError(string.Empty, "User or password incorrect.");
            return View(model);
        }
    }
}