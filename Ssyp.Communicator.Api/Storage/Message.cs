using System;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Api.Storage
{
    [Serializable]
    internal sealed class Message
    {
        public Message([NotNull] string sender, long timeStamp, [NotNull] string value)
        {
            Sender = sender ?? throw new ArgumentNullException(nameof(sender));
            TimeStamp = timeStamp;
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        [NotNull] public string Sender { get; set; }
        public long TimeStamp { get; set; }
        [NotNull] public string Value { get; set; }

        [NotNull]
        public override string ToString()
        {
            return
                $"{nameof(Message)}({nameof(Sender)}={Sender}, {nameof(TimeStamp)}={TimeStamp}, " +
                $"{nameof(Value)}={Value})";
        }
    }
}