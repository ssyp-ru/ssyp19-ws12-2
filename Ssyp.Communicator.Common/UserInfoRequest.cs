using System;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common
{
    [Serializable]
    public sealed class UserInfoRequest : ICommunicatorRequest
    {
        public UserInfoRequest(Guid apiKey, long userId)
        {
            ApiKey = apiKey;
            UserID = userId;
        }

        public long UserID { get; set; }
        public Guid ApiKey { get; set; }

        [NotNull]
        public override string ToString()
        {
            return $"UserInfoRequest(ApiKey={ApiKey}, UserID={UserID}";
        }
    }
}