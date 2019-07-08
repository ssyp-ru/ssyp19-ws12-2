using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Api.Storage
{
    [Serializable]
    internal sealed class Conversation
    {
        internal Conversation([NotNull] User first, [NotNull] User second, [NotNull] List<Message> messages)
        {
            Condition.Requires(messages, nameof(messages)).IsNotNull();
            First = first;
            Second = second;
            Messages = messages;
        }

        [NotNull] internal User First { get; }
        [NotNull] internal User Second { get; }

        [NotNull] internal List<Message> Messages { get; }

        [NotNull]
        public override string ToString() =>
            $"{nameof(Conversation)}({nameof(First)}={First}, {nameof(Second)}={Second})";

        internal bool ContainsUser([NotNull] User user)
        {
            Condition.Requires(user, nameof(user)).IsNotNull();
            return user.Equals(First) || user.Equals(Second);
        }
    }
}