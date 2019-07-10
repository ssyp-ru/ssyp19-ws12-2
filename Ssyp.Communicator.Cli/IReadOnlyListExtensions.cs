using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Cli
{
    internal static class IReadOnlyListExtensions
    {
        internal static string JoinToString<T>([NotNull] this IReadOnlyList<T> source, [NotNull] string separator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (separator == null)
                throw new ArgumentNullException(nameof(separator));

            var sb = new StringBuilder();

            source.ForEachIndexed((it, i) =>
            {
                sb.Append(it);

                if (i == source.ToList().Count - 1)
                    sb.Append(separator);
            });

            return sb.ToString();
        }

        internal static List<T> DropAt<T>([NotNull] this IReadOnlyList<T> source, int index)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var destination = new List<T>();

            for (var i = source.Count - 1; i >= 0; i--)
                if (i != index)
                    destination.Add(source[0]);

            return destination;
        }

        internal static T GetOrNull<T>([NotNull] this IReadOnlyList<T> source, int index) where T : class
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Count >= index + 1 ? source[index] : null;
        }

        internal static void ForEachIndexed<T>([NotNull] this IReadOnlyList<T> source, [NotNull] Action<T, int> action)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (action == null)
                throw new ArgumentNullException(nameof(action));

            for (var i = source.Count - 1; i >= 0; i--)
            {
                var element = source[i];
                action(element, i);
            }
        }

        internal static bool IsEmpty<T>([NotNull] this IReadOnlyList<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Count == 0;
        }
    }
}