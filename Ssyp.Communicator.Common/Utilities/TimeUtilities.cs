using System;

namespace Ssyp.Communicator.Common.Utilities
{
    public static class TimeUtilities
    {
        public static long CurrentTimeMillis()
        {
            return (long) (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
    }
}