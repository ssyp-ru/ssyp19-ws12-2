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
            Condition.Requires(message, "message").IsNotNull();
            ApiKey = apiKey;
            Message = message;
            Receiver = receiver;
        }

        [NotNull] public string Message { get; set; }
        public long Receiver { get; set; }
        public Guid ApiKey { get; set; }

        [NotNull]
        public override string ToString()
        {
            return $"ConversationSendRequest(ApiKey={ApiKey}, Message={Message}, Receuver={Receiver})";
        }
    }
}