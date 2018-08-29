using ReportWeb.BLL;
using ReportWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportWeb.Controllers
{
    public class MenuController : ControllerBase
    {
        public ActionResult LeftMenu()
        {
            SecurityBLL sec = new SecurityBLL();
            List<MenuModel>  menu = sec.CreateMenuModel(ConnectedUser);
            return PartialView(menu);
        }
    }
}