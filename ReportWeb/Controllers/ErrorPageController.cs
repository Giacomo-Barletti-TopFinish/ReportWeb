using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportWeb.Controllers
{
    public class ErrorPageController : Controller
    {
        public ActionResult Error(WebReportException ex)
        {
            if (ex == null) return null;
            var model = new HandleErrorInfo(ex.InnerException, ex.Controller, ex.Action);
            Response.Clear();
            Response.StatusCode = 200;
            return View(model);
        }
       
    }
}