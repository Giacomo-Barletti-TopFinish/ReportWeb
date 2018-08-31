using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace ReportWeb.Common.Helpers
{
    public class LogManagerHelper
    {
        private const string ApplicationName = "ReportWeb";
        private static readonly ILog _logger = log4net.LogManager.GetLogger("ReportWeb");

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