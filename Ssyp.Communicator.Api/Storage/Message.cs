using System;
using CuttingEdge.Conditions;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Api.Storage
{
    [Serializable]
    internal sealed class Message
    {
        internal Message([NotNull] User sender, long timeStamp, [NotNull] string value)
        {
            Condition.Requires(sender, "sender").IsNotNull();
            Condition.Requires(value, "value").IsNotNull();
            Sender = sender;
            TimeStamp = timeStamp;
            Value = value;
        }

        [NotNull] internal User Sender { get; }
        internal long TimeStamp { get; }
        [NotNull] internal string Value { get; set; }

        [NotNull]
        public override string ToString()
        {
            return $"Message(Sender={Sender}, TimeStamp={TimeStamp}, Value={Value})";
        }
    }
}