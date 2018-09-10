using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.Core
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ColumnMapAttribute : Attribute
    {
        public ColumnMapAttribute(string fieldName)
        {
            FieldName = fieldName;
        }

        public string FieldName { get; protected set; }
    }
}
