using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Ganss.XSS;

namespace Common
{
    public class String
    {
        public static string Sluger(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            
            text = Regex.Replace(text, @"\s+", " ").Trim(); /*تبدیل تمام فضاهای خالی دو یا چند فاصله ای ( Space ) به 1 فضای خالی*/
            text = Regex.Replace(text, @"\s", "-");         /*تبدیل فضای های خالی ( 1 Space ) ای به عبارت ( - )*/
            return text;
        }
        
        public static string DeSluger(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            
            return Regex.Replace(text, "-", " ").Trim();
        }

        public static string Sanitize(string text)
        {
            string one = text.Replace(@"\", "").Replace(@"/", "");
            string two = Regex.Replace(text, @"<[^>]*(>|$)", "");
            return new HtmlSanitizer().Sanitize(text.Trim());
        }

        public static string EnNumberToPersian(string text)
        {
            string[] persian = { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };

            for (int i= 0; i< persian.Length; i++)
                text = text.Replace(i.ToString(), persian[i]);

            return text;
        }
    }
}