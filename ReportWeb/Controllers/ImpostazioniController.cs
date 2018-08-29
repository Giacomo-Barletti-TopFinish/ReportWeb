using ReportWeb.BLL;
using ReportWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportWeb.Controllers
{
    public class ImpostazioniController : ControllerBase
    {
        // GET: Impostazioni
        public ActionResult AbilitaMenu()
        {
            SecurityBLL sec = new SecurityBLL();
            List<RWListItem> utenti = sec.FillUtenti();
           
            ViewData.Add("ListaUtenti", utenti);
            return View();
        }

        public ActionResult GetMenuUtente(string UIDUSER)
        {
            SecurityBLL sec = new SecurityBLL();
            List<MenuModel> menu = sec.CreateMenuModel(UIDUSER);

            return PartialView("GetMenuUtentePartial",menu);
        }

        public ActionResult SalvaMenuUtente(string UIDUSER, int[] idMenu)
        {
            SecurityBLL sec = new SecurityBLL();
            sec.SalvaMenuUtente(UIDUSER, idMenu);
            return null;
        }
    }
}