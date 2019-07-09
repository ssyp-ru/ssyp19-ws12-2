using System;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Api
{
    internal static class Strings
    {
        [CanBeNull]
        internal static Guid? ToGuidOrNull([NotNull] this string s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));

            try
            {
                return Guid.Parse(s);
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}