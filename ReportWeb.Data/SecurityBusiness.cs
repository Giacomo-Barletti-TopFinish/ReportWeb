using ReportWeb.Data.Core;
using ReportWeb.Entities;
using ReportWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data
{
    public class SecurityBusiness : ReportWebBusinessBase
    {
        public SecurityBusiness() : base() { }

        [DataContext]
        public void FillUsers(SecurityDS ds)
        {
            SecurityAdapter a = new SecurityAdapter(DbConnection, DbTransaction);
            a.FillUsers(ds);
        }

        [DataContext]
        public void SaveToken(string UIDUSER, string Token, int Durata, string IpAddress)
        {
            SecurityAdapter a = new SecurityAdapter(DbConnection, DbTransaction);
            a.SaveToken(UIDUSER, Token, Durata, IpAddress);
        }

        [DataContext]
        public void GetToken(SecurityDS ds, string token)
        {
            SecurityAdapter a = new SecurityAdapter(DbConnection, DbTransaction);
            a.GetToken(ds, token);
        }

        [DataContext]
        public void FillMenu(SecurityDS ds)
        {
            SecurityAdapter a = new SecurityAdapter(DbConnection, DbTransaction);
            a.FillMenu(ds);
        }

        [DataContext]
        public void FillUserMenu(string UIDUSER, SecurityDS ds)
        {
            SecurityAdapter a = new SecurityAdapter(DbConnection, DbTransaction);
            a.FillUserMenu(UIDUSER, ds);
        }

        [DataContext(true)]
        public void SalvaMenuUtente(SecurityDS ds)
        {
            SecurityAdapter a = new SecurityAdapter(DbConnection, DbTransaction);
            a.UpdateSecurityDSTable(ds.RW_USER_MENU.TableName, ds);
        }
    }
}