using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.Core
{
    public class UnsupportedDatabaseEngineException : Exception
    {
        public string ConnectionString { get; private set; }

        public UnsupportedDatabaseEngineException(string connectionString) : this(connectionString, string.Format("Unsupported connection '{0}'", connectionString), null)
        {
        }

        public UnsupportedDatabaseEngineException(string connectionString, string message)
            : this(connectionString, message, null)
        {
        }

        public UnsupportedDatabaseEngineException(string connectionString, string message, Exception innerException)
            : base(message, innerException)
        {
            ConnectionString = connectionString;
        }
    }
}
