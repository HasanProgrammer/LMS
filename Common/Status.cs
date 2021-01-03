using System;
using System.Linq;

namespace Common
{
    public class Status
    {
        public static T ConvertNumberToEnum<T>(int Status = 0 | 1) where T : Enum
        {
            Init<T>(out object Active, out object Inactive);
            
            /*-------------------------------------------------------*/

            switch (Status)
            {
                case 0 : return (T)Inactive;
                case 1 : return (T)Active;
                default: throw new Exception("ورودی Status باید از بین مقادیر 0 یا 1 باشد");
            }
        }
        
        public static (T, string) ConvertToEnumString<T>(int Status = 0 | 1) where T : Enum
        {
            Init<T>(out object Active, out object Inactive);
            
            /*-------------------------------------------------------*/

            switch (Status)
            {
                case 0 : return ((T)Inactive , "غیر فعال");
                case 1 : return ((T)Active   , "فعال");
                default: throw new Exception("ورودی Status باید از بین مقادیر 0 یا 1 باشد");
            }
        }

        private static void Init<T>(out object active, out object inactive)
        {
            /*در این قسمت بررسی می گردد که آیا تایپ مورد نظر Enum می باشد یا خیر*/
            if(!typeof(T).IsEnum) throw new Exception("تایپ مورد نظر باید یک Enum باشد");

            /*در این قسمت مشخص می گردد که آیا Enum مورد نظر دو فیلد دارد یا خیر*/
            Array values = typeof(T).GetEnumValues();
            if(values.Length == 0 || values.Length > 2) throw new Exception("تایپ مورد نظر باید دارای حداکثر و حداقل دو فیلد باشد");
            
            /*در این قسمت باید نام دقیق فیلد Active در تایپ مورد نظر بررسی گردد*/
            active = values.GetValue(1);
            if(typeof(T).GetMember( active.ToString() ).First().Name != "Active") throw new Exception("تایپ مورد نظر باید فیلد Active را داشته باشد");

            /*در این قسمت باید نام دقیق فیلد Inactive در تایپ مورد نظر بررسی گردد*/
            inactive = values.GetValue(0);
            if(typeof(T).GetMember( inactive.ToString() ).First().Name != "Inactive") throw new Exception("تایپ مورد نظر باید فیلد Active را داشته باشد");
        }
    }
}