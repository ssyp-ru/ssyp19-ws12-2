using System;
using CuttingEdge.Conditions;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common
{
    public struct ConversationSendRequest
    {
        public Guid ApiKey;
        [NotNull] public string Message;
        public long Receiver;

        public ConversationSendRequest(Guid apiKey, [NotNull] string message, long receiver)
        {
            Condition.Requires(message, "message").IsNotNull();
            ApiKey = apiKey;
            Message = message;
            Receiver = receiver;
        }

        public override string ToString()
        {
            return $"ConversationSendRequest(ApiKey={ApiKey}, Message={Message}, Receuver={Receiver})";
        }
    }
}