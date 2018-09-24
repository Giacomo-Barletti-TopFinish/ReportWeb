using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Entities
{
    public class IdMAncanti
    {
        public static string MancMaterTornitura = "0000000063";
        public static string MancMaterFresatura = "0000000064";
        public static string MancMaterPiegafilo = "0000000065";
        public static string MancMaterPressofusione = "0000000066";
        public static string MancMaterRiprese = "0000000067";
        public static string MancMaterStampaggio = "0000000112";
        public static string MancMaterTermineLavoro = "0000000115";
        public static string MancMaterInfragruppo = "0000000118";
        public static string MancMaterGiacenza = "0000000122";

        public static List<string> ListaIdMancanti = new List<string>(new string[]
        {
            MancMaterTornitura,
            MancMaterFresatura,
            MancMaterPiegafilo,
            MancMaterPressofusione,
            MancMaterRiprese,
            MancMaterStampaggio,
            MancMaterTermineLavoro,
            MancMaterInfragruppo,
            MancMaterGiacenza
        });
    }
}
