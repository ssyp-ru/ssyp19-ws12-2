using System;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common
{
    [Serializable]
    public sealed class ConversationListRequest : ICommunicatorRequest
    {
        public ConversationListRequest(Guid apiKey) => ApiKey = apiKey;

        public Guid ApiKey { get; set; }

        [NotNull]
        public override string ToString() => $"{nameof(ConversationListRequest)}({nameof(ApiKey)}={ApiKey}";
    }
}