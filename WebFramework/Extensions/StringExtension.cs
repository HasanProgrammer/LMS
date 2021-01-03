using System.Text.RegularExpressions;

namespace WebFramework.Extensions
{
    public static class StringExtension
    {
        public static string Slug(this string text)
        {
            text = Regex.Replace(text, @"\s+", " ").Trim(); /*تبدیل تمام فضاهای خالی دو یا چند فاصله ای ( Space ) به 1 فضای خالی*/
            text = Regex.Replace(text, @"\s", "-");         /*تبدیل فضای های خالی ( 1 Space ) ای به عبارت ( - )*/
            return text;
        }

        public static string DeSlug(this string text)
        {
            return Regex.Replace(text, "-", " ").Trim();
        }
    }
}