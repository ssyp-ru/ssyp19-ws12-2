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
            Condition.Requires(sender, nameof(sender)).IsNotNull();
            Condition.Requires(value, nameof(value)).IsNotNull();
            Sender = sender;
            TimeStamp = timeStamp;
            Value = value;
        }

        [NotNull] internal User Sender { get; }
        internal long TimeStamp { get; }
        [NotNull] internal string Value { get; set; }

        [NotNull]
        public override string ToString() =>
            $"{nameof(Message)}({nameof(Sender)}={Sender}, {nameof(TimeStamp)}={TimeStamp}, {nameof(Value)}={Value})";
    }
}