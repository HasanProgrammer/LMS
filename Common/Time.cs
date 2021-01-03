using System;

namespace Common
{
    public class Time
    {
        public static long TimeStampNow()
        {
            return new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
        }
    }
}