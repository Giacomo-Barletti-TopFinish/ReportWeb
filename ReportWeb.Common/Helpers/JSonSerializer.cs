using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Common.Helpers
{
    public class JSonSerializer
    {
        public static string Serialize<T>(object obj)
        {
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            js.WriteObject(ms, obj);

            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            string json = sr.ReadToEnd();
            sr.Close();
            ms.Close();
            return json;
        }

        public static T Deserialize<T>(string jsonData)
        {
            MemoryStream stream = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(jsonData));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            stream.Position = 0;
            T obj = (T)ser.ReadObject(stream);
            stream.Close();
            return obj;
        }
    }
}
