using System.Text.RegularExpressions;

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
    }
}