using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Common.Helpers
{
    public class DateTimeHelper
    {
        public static int SettimanaAnno(DateTime dt)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            return cal.GetWeekOfYear(dt, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }

        public static DateTime PrimoGiornoSettimana(int year, int weekNum)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;

            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Monday - jan1.DayOfWeek;
            DateTime firstMonday = jan1.AddDays(daysOffset);

            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstMonday, dfi.CalendarWeekRule, DayOfWeek.Monday);

            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }

            DateTime result = firstMonday.AddDays(weekNum * 7);
            return result;
        }

        public static TimeSpan CalcolaDurata(string inizio, string fine)
        {
            DateTime startTime = Convert.ToDateTime(inizio);
            DateTime endtime = Convert.ToDateTime(fine);
            return endtime - startTime;
        }

        public static TimeSpan ConvertiTimespan(string durata)
        {
            string[] str = durata.Split(':');

            if (str.Length == 0) return new TimeSpan();
            if (str.Length > 2) return new TimeSpan();

            int ora = int.Parse(str[0]);
            int minuti = int.Parse(str[1]);

            return new TimeSpan(ora, minuti, 0);

        }


        public static TimeSpan CalcolaDurata(DateTime inizio, DateTime fine)
        {
            return fine - inizio;
        }
    }
}
