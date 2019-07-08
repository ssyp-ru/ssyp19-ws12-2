using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Api.Storage
{
    [Serializable]
    internal sealed class Conversation
    {
        internal Conversation([NotNull] User first, [NotNull] User second, [NotNull] List<Message> messages)
        {
            First = first ?? throw new ArgumentNullException(nameof(first));
            Second = second ?? throw new ArgumentNullException(nameof(second));
            Messages = messages ?? throw new ArgumentNullException(nameof(messages));
        }

        [NotNull] internal User First { get; }
        [NotNull] internal User Second { get; }

        [NotNull] internal List<Message> Messages { get; }

        [NotNull]
        public override string ToString()
        {
            return $"{nameof(Conversation)}({nameof(First)}={First}, {nameof(Second)}={Second})";
        }

        internal bool ContainsUser([NotNull] User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return user.Equals(First) || user.Equals(Second);
        }
    }
}