using System;

namespace Assets.Scripts.Handlers
{
    public static class TimeHandler
    {
        public static double GetMilliseconds()
        {
            DateTime dt1970 = new DateTime(1970, 1, 1);
            DateTime current = DateTime.Now;
            TimeSpan span = current - dt1970;
            return span.TotalMilliseconds;
        }
    }
}