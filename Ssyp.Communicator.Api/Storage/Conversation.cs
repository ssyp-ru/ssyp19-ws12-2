using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Api.Storage
{
    [Serializable]
    internal sealed class Conversation
    {
        internal Conversation(User first, User second, [NotNull] List<Message> messages)
        {
            Condition.Requires(messages, "messages").IsNotNull();
            First = first;
            Second = second;
            Messages = messages;
        }

        internal User First { get; }
        internal User Second { get; }

        [NotNull] internal List<Message> Messages { get; }

        public override string ToString()
        {
            return $"Conversation(First={First}, Second={Second})";
        }

        internal bool ContainsUser(User user)
        {
            return user.ApiKey.Equals(First.ApiKey) || user.ApiKey.Equals(Second.ApiKey);
        }
    }
}