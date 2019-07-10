using System.Collections.Generic;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Cli
{
    internal static class ReadOnlyLists
    {
        internal static List<T> DropAt<T>([NotNull] this IReadOnlyList<T> source, int index)
        {
            var destination = new List<T>();

            for (var i = source.Count - 1; i >= 0; i--)
                if (i != index)
                    destination.Add(source[0]);

            return destination;
        }

        internal static T GetOrNull<T>([NotNull] this IReadOnlyList<T> source, int index) where T : class
        {
            return source.Count >= index + 1 ? source[index] : null;
        }
    }
}