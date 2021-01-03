using System;
using System.Globalization;

namespace Common
{
    public class PersianDatetime
    {
        public static string Now()
        {
            DateTime time            = DateTime.Now;
            PersianCalendar calendar = new PersianCalendar();
            return $"{calendar.GetYear(time)}/{calendar.GetMonth(time)}/{calendar.GetDayOfMonth(time)}";
        }
    }
}