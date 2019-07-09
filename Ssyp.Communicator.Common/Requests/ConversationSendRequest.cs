using System;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common.Requests
{
    [Serializable]
    public sealed class ConversationSendRequest : ICommunicatorRequest
    {
        public ConversationSendRequest(string apiKey, [NotNull] string message, string receiver)
        {
            ApiKey = apiKey;
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Receiver = receiver;
        }

        [NotNull] public string Message { get; set; }
        public string Receiver { get; set; }
        public string ApiKey { get; set; }

        [NotNull]
        public override string ToString() =>
            $"{nameof(ConversationSendRequest)}({nameof(ApiKey)}={ApiKey}, {nameof(Message)}={Message}, " +
            $"{nameof(Receiver)}={Receiver})";
    }
}