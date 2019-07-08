using System;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common
{
    [Serializable]
    public sealed class ConversationSendRequest : ICommunicatorRequest
    {
        public ConversationSendRequest(Guid apiKey, [NotNull] string message, Guid receiver)
        {
            ApiKey = apiKey;
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Receiver = receiver;
        }

        [NotNull] public string Message { get; set; }
        public Guid Receiver { get; set; }
        public Guid ApiKey { get; set; }

        [NotNull]
        public override string ToString() =>
            $"{nameof(ConversationSendRequest)}({nameof(ApiKey)}={ApiKey}, {nameof(Message)}={Message}, " +
            $"{nameof(Receiver)}={Receiver})";
    }
}