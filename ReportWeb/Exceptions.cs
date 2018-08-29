using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportWeb
{
    public class WebReportException : Exception
    {
        private string _controller;
        private string _action;

        public WebReportException(string controller, string action, Exception innerException) : base(string.Empty, innerException)
        {
            _controller = controller;
            _action = action;
        }

        public WebReportException() : this(string.Empty,  string.Empty, new Exception()) { }

        public string Controller
        {
            get { return _controller; }
            set { _controller = value; }
        }

        public string Action
        {
            get { return _action; }
            set { _action = value; }
        }

    }
}