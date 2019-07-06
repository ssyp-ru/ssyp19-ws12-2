using System;

namespace Ssyp.Communicator.Common
{
    [Serializable]
    public struct UserInfoRequest
    {
        public Guid ApiKey;
        public long UserID;

        public UserInfoRequest(Guid apiKey, long userId)
        {
            ApiKey = apiKey;
            UserID = userId;
        }

        public override string ToString()
        {
            return $"UserInfoRequest(ApiKey={ApiKey}, UserID={UserID}";
        }
    }
}