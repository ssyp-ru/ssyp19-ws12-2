using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace Ssyp.Communicator.CommonClient
{
    public static class ReadOnlyListExtensions
    {
        public static string JoinToString<T>([NotNull] this IReadOnlyList<T> source, [NotNull] string separator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (separator == null)
                throw new ArgumentNullException(nameof(separator));

            var sb = new StringBuilder();

            source.ForEachIndexed((it, i) =>
            {
                sb.Append(it);

                if (i != source.ToList().Count - 1)
                    sb.Append(separator);
            });

            return sb.ToString();
        }

        public static List<T> Drop<T>([NotNull] this IReadOnlyList<T> source, int index)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var destination = new List<T>();

            source.ForEachIndexed((it, i) =>
            {
                if (i != index)
                    destination.Add(it);
            });

            return destination;
        }

        public static T GetOrNull<T>([NotNull] this IReadOnlyList<T> source, int index) where T : class
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Count >= index + 1 ? source[index] : null;
        }

        private static void ForEachIndexed<T>([NotNull] this IReadOnlyList<T> source, [NotNull] Action<T, int> action)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (action == null)
                throw new ArgumentNullException(nameof(action));
            
            for (var i = 0; i < source.Count; i++)
            {
                var element = source[i];
                action(element, i);   
            }
        }
    }
}