using System;

namespace Common
{
    public static class DigitCode
    {
        public static int Generate8()
        {
            const int min = 10000000;
            const int max = 99999999;

            return new Random().Next(min, max);
        }
    }
}