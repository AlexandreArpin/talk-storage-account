using System;

namespace StorageAccount.Chat
{
    public static class DateTimeExtensions
    {
        public static long ToReverseTimestamp(this DateTime value)
        {
            return long.MaxValue - value.ToUniversalTime().Ticks;
        }

        public static DateTime GetReverseTimestamp(this long value)
        {
            return new DateTime(long.MaxValue - value, DateTimeKind.Utc);
        }
    }
}
