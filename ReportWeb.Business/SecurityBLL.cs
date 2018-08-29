using ReportWeb.Data;
using ReportWeb.Entities;
using ReportWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.BLL
{
    public class SecurityBLL
    {
        private const int TokenValidityInMinutes = 3000;
        private const int BlockFailedCount = 30000;
        private const int PasswordLength = 8;
        private const int TokenLength = 32;
        private const int MinRandomPasswordValue = 65;
        private const int MaxRandomPasswordValue = 90;

        public SecurityBLL()
        {

        }

        public string VerifyUser(string userID, string password, string ipAddress)
        {
            SecurityDS ds = new SecurityDS();
            using (SecurityBusiness bSecurity = new SecurityBusiness())
            {
                bSecurity.FillUsers(ds);


                SecurityDS.USR_USERRow user = ds.USR_USER.Where(x => x.UIDUSER == userID).FirstOrDefault();
                if (user == null) return null;

                if (user.PWDUSER != password) return null;

                using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
                {

                    StringBuilder tokenBuilder = new StringBuilder(TokenLength);
                    Random randomizer = new Random();

                    for (int i = 0; i < TokenLength; i++)
                    {
                        int randomNumber = randomizer.Next(MinRandomPasswordValue, MaxRandomPasswordValue);
                        char ch = Convert.ToChar(randomNumber);
                        tokenBuilder.Append(ch);
                    }

                    string token = tokenBuilder.ToString();
                    bSecurity.SaveToken(userID, token, TokenValidityInMinutes, ipAddress);
                    return token;
                }
            }
        }

        public static TokenModel GetTokenModel(string token)
        {
            using (SecurityBusiness bSecurity = new SecurityBusiness())
            {
                SecurityDS ds = new SecurityDS();

                bSecurity.GetToken(ds, token);
                SecurityDS.RW_TOKENRow t = ds.RW_TOKEN.Where(x => x.TOKEN == token).FirstOrDefault();
                if (t == null) return null;

                TokenModel tok = new TokenModel()
                {
                    IpAddress = t.IsIPADDRESSNull() ? string.Empty : t.IPADDRESS,
                    Token = token,
                    User = t.UIDUSER
                };

                return tok;
            }
        }

        public List<MenuModel> CreateMenuModel(string UIDUSER)
        {
            SecurityDS ds = new SecurityDS();
            using (SecurityBusiness bSecurity = new SecurityBusiness())
            {
                bSecurity.FillMenu(ds);
                bSecurity.FillUserMenu(UIDUSER, ds);
            }

            List<int> idMenuAbilitati = ds.RW_USER_MENU.Where(x => x.UIDUSER == UIDUSER).Select(x => (int)x.IDMENU).ToList();

            List<MenuModel> menu = new List<MenuModel>();
            foreach (SecurityDS.RW_MENURow row in ds.RW_MENU.Where(x => x.IsIDMENUPADRENull()))
            {
                MenuModel elementoMenu = CreaMenu(ds, row.IDMENU, idMenuAbilitati);
                menu.Add(elementoMenu);
            }



            return menu;
        }

        private MenuModel CreaMenu(SecurityDS ds, decimal idMenu, List<int> idMenuAbilitati)
        {
            SecurityDS.RW_MENURow row = ds.RW_MENU.Where(x => x.IDMENU == idMenu).FirstOrDefault();

            MenuModel padre = new MenuModel();
            padre.IdMenu = (int)row.IDMENU;
            padre.Azione = row.AZIONE == 1 ? true : false;
            padre.Etichetta = row.ETICHETTA;
            padre.HRef = row.IsHREFNull() ? string.Empty : row.HREF;
            padre.OnClick = row.IsONCLICKNull() ? string.Empty : row.ONCLICK;
            padre.Font = row.IsFONTNull() ? string.Empty : row.FONT;
            padre.MenuFiglio = new List<MenuModel>();
            padre.Abilitato = idMenuAbilitati.Contains(padre.IdMenu) ? true : false;

            foreach (SecurityDS.RW_MENURow rowFiglio in ds.RW_MENU.Where(x => !x.IsIDMENUPADRENull() && x.IDMENUPADRE == row.IDMENU).OrderBy(x => x.SEQUENZA))
            {
                MenuModel figlio = CreaMenu(ds, rowFiglio.IDMENU, idMenuAbilitati);
                padre.MenuFiglio.Add(figlio);
            }
            return padre;
        }

        public List<RWListItem> FillUtenti()
        {
            SecurityDS ds = new SecurityDS();
            using (SecurityBusiness bSecurity = new SecurityBusiness())
            {
                bSecurity.FillUsers(ds);
            }

            List<RWListItem> utenti = (from tp in ds.USR_USER.OrderBy(x => x.FULLNAMEUSER) select new RWListItem(tp.FULLNAMEUSER, tp.UIDUSER)).ToList();

            utenti = utenti.Distinct(new RWListItemComparer()).ToList();
            utenti.Insert(0, new RWListItem(string.Empty, string.Empty));
            return utenti;
        }

        public void SalvaMenuUtente(string UIDUSER, int[] idMenu)
        {
            SecurityDS ds = new SecurityDS();
            using (SecurityBusiness bSecurity = new SecurityBusiness())
            {
                bSecurity.FillUserMenu(UIDUSER, ds);


                foreach (SecurityDS.RW_USER_MENURow row in ds.RW_USER_MENU.Where(x => !idMenu.Contains((int)x.IDMENU)).ToList())
                    row.Delete();

                foreach (int idm in idMenu)
                {
                    SecurityDS.RW_USER_MENURow row = ds.RW_USER_MENU.Where(x => x.RowState != System.Data.DataRowState.Deleted && x.IDMENU == idm).FirstOrDefault();
                    if (row == null)
                    {
                        SecurityDS.RW_USER_MENURow newrow = ds.RW_USER_MENU.NewRW_USER_MENURow();
                        newrow.IDMENU = idm;
                        newrow.UIDUSER = UIDUSER;
                        ds.RW_USER_MENU.AddRW_USER_MENURow(newrow);
                    }
                }
                bSecurity.SalvaMenuUtente(ds);
            }
        }
    }
}
