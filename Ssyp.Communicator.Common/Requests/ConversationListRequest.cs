using System;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common.Requests
{
    [Serializable]
    public sealed class ConversationListRequest : ICommunicatorRequest
    {
        public ConversationListRequest([NotNull] string apiKey) =>
            ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));

        public string ApiKey { get; set; }

        [NotNull]
        public override string ToString() => $"{nameof(ConversationListRequest)}({nameof(ApiKey)}={ApiKey}";
    }
}