using System;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Api.Storage
{
    [Serializable]
    internal sealed class Message
    {
        internal Message([NotNull] User sender, long timeStamp, [NotNull] string value)
        {
            Sender = sender ?? throw new ArgumentNullException(nameof(sender));
            TimeStamp = timeStamp;
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        [NotNull] internal User Sender { get; set; }
        internal long TimeStamp { get; set; }
        [NotNull] internal string Value { get; set; }

        [NotNull]
        public override string ToString()
        {
            return
                $"{nameof(Message)}({nameof(Sender)}={Sender}, {nameof(TimeStamp)}={TimeStamp}, " +
                $"{nameof(Value)}={Value})";
        }
    }
}