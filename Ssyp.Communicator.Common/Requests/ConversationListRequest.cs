using System;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common.Requests
{
    [Serializable]
    public sealed class ConversationListRequest : ICommunicatorRequest
    {
        public ConversationListRequest(string apiKey) => ApiKey = apiKey;

        public string ApiKey { get; set; }

        [NotNull]
        public override string ToString() => $"{nameof(ConversationListRequest)}({nameof(ApiKey)}={ApiKey}";
    }
}