using System;

namespace Ssyp.Communicator.Common.Utilities
{
    public static class TimeUtilities
    {
        private static readonly DateTime Jan1st1970 = new DateTime
            (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long CurrentTimeMillis()
        {
            return (long) (DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }
    }
}