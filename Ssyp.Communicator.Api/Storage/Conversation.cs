using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Api.Storage
{
    [Serializable]
    internal sealed class Conversation
    {
        public Conversation([NotNull] string first, [NotNull] string second, [NotNull] List<Message> messages)
        {
            First = first ?? throw new ArgumentNullException(nameof(first));
            Second = second ?? throw new ArgumentNullException(nameof(second));
            Messages = messages ?? throw new ArgumentNullException(nameof(messages));
        }

        [NotNull] public string First { get; set; }
        [NotNull] public string Second { get; set; }
        [NotNull] public IList<Message> Messages { get; set; }

        private bool Equals([CanBeNull] Conversation other)
        {
            return other != null && string.Equals(First, other.First) && string.Equals(Second, other.Second);
        }

        public override bool Equals([CanBeNull] object obj)
        {
            return obj is Conversation other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (First.GetHashCode() * 397) ^ Second.GetHashCode();
            }
        }

        [NotNull]
        public override string ToString()
        {
            return $"{nameof(Conversation)}({nameof(First)}={First}, {nameof(Second)}={Second})";
        }

        internal bool ContainsUser([NotNull] User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));


            return user.Name.Equals(First) || user.Name.Equals(Second);
        }
    }
}