using System;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common.Requests
{
    [Serializable]
    public sealed class ConversationSendRequest : ICommunicatorRequest
    {
        public ConversationSendRequest([NotNull] string apiKey, [NotNull] string message, [NotNull] string receiver)
        {
            ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
        }

        [NotNull] public string Message { get; set; }
        [NotNull] public string Receiver { get; set; }
        [NotNull] public string ApiKey { get; set; }

        [NotNull]
        public override string ToString() =>
            $"{nameof(ConversationSendRequest)}({nameof(ApiKey)}={ApiKey}, {nameof(Message)}={Message}, " +
            $"{nameof(Receiver)}={Receiver})";
    }
}