using System;
using CuttingEdge.Conditions;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common
{
    [Serializable]
    public sealed class ConversationSendRequest : ICommunicatorRequest
    {
        public ConversationSendRequest(Guid apiKey, [NotNull] string message, long receiver)
        {
            Condition.Requires(message, nameof(message)).IsNotNull();
            ApiKey = apiKey;
            Message = message;
            Receiver = receiver;
        }

        [NotNull] public string Message { get; set; }
        public long Receiver { get; set; }
        public Guid ApiKey { get; set; }

        [NotNull]
        public override string ToString() =>
            $"{nameof(ConversationSendRequest)}({nameof(ApiKey)}={ApiKey}, {nameof(Message)}={Message}, " +
            $"{nameof(Receiver)}={Receiver})";
    }
}