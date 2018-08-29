using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ReportWeb.Helpers
{
    public class LogManager
    {
        private const string ApplicationName = "SPC";
        private static readonly ILog _logger = log4net.LogManager.GetLogger("SPC");

        public static void WriteException(Exception ex)
        {
            _logger.Fatal("------------------------------- ECCEZIONE ---------------------------------------------");
            _logger.Fatal(ApplicationName, ex);
        }

        public static void WriteException(string Controller, string Method, Exception ex)
        {
            _logger.Fatal("------------------------------- ECCEZIONE ---------------------------------------------");
            _logger.Fatal(string.Format("CONTROLLER:{0}    ACTION:{1}",Controller,Method));
            _logger.Fatal(ApplicationName, ex);
        }

        public static void WriteException(ExceptionContext ex, string userName)
        {
            string actionName = ex.RouteData.Values["action"].ToString();
            string controllerName = ex.RouteData.Values["controller"].ToString();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("EXCEPTION INFORMATION");
            sb.AppendLine(string.Format("     CONTROLLER: {0}", controllerName));
            sb.AppendLine(string.Format("     ACTION: {0}", actionName));
            sb.AppendLine(string.Format("     USER: {0}", userName));

            foreach (string param in ex.RequestContext.HttpContext.Request.Params.AllKeys)
            {
                sb.AppendLine(string.Format("     {0}: {1}", param, ex.RequestContext.HttpContext.Request.Params[param]));
            }

            _logger.Fatal(sb.ToString());
            _logger.Fatal(ex.Controller, ex.Exception);
            if (ex.Exception.InnerException != null)
                _logger.Fatal("Inner exception", ex.Exception.InnerException);

        }

        public static void WriteMessage(string message)
        {
            _logger.Info(message);
        }

        public static void WriteWarning(string message)
        {
            _logger.Warn(message);
        }

        public static void WriteError(string message)
        {
            _logger.Error(message);
        }
    }
}