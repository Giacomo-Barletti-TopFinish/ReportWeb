using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.Core
{
    public static class DaoExtensor
    {
        private class MappedProperty
        {
            public PropertyInfo Property { get; private set; }
            public ColumnMapAttribute Map { get; private set; }

            public MappedProperty(PropertyInfo pi, ColumnMapAttribute cma)
            {
                Property = pi;
                Map = cma;
            }
        }

        public static List<T> ToEntityList<T>(this IDataReader dr) where T : class, new()
        {
            List<MappedProperty> mappedProperties = new List<MappedProperty>();
            Type t = typeof(T);
            foreach (PropertyInfo pi in t.GetProperties())
            {
                foreach (Attribute a in pi.GetCustomAttributes(true))
                {
                    if (a is ColumnMapAttribute)
                    {
                        ColumnMapAttribute cma = a as ColumnMapAttribute;
                        mappedProperties.Add(new MappedProperty(pi, cma));
                    }
                }
            }

            List<T> entities = new List<T>();
            while (dr.Read())
            {
                T entity = new T();
                foreach (MappedProperty mp in mappedProperties)
                {
                    object value;
                    try
                    {
                        value = dr[mp.Map.FieldName];
                    }
                    catch
                    {
                        throw;
                    }
                    if (value != DBNull.Value)
                    {
                        Type pt = mp.Property.PropertyType;
                        if (pt.IsGenericType && pt.GetGenericTypeDefinition() == typeof(Nullable<>))
                            pt = pt.GetGenericArguments()[0];
                        value = Convert.ChangeType(value, pt, CultureInfo.InvariantCulture);
                    }
                    else
                        value = null;
                    mp.Property.SetValue(entity, value, null);
                }
                entities.Add(entity);
            }
            return entities;
        }
    }
}
