using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.Core
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DataContextAttribute : Attribute
    {
        public DataContextAttribute()
            : this(string.Empty, false)
        {
        }

        public DataContextAttribute(bool transactional)
            : this(string.Empty, transactional)
        {
        }

        public DataContextAttribute(string contextName)
            : this(contextName, false)
        {
        }

        public DataContextAttribute(string contextName, bool transactional)
        {
            ContextName = contextName;
            Transactional = transactional;
        }

        public string ContextName { get; protected set; }
        public bool Transactional { get; protected set; }
    }
}
