using System;

namespace Ssyp.Communicator.Common
{
    [Serializable]
    public struct ConversationListRequest
    {
        public Guid ApiKey;

        public ConversationListRequest(Guid apiKey)
        {
            ApiKey = apiKey;
        }

        public override string ToString()
        {
            return $"ConversationListRequest(ApiKey={ApiKey}";
        }
    }
}