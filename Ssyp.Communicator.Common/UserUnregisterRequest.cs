using System;

namespace Ssyp.Communicator.Common
{
    [Serializable]
    public struct UserUnregisterRequest
    {
        public Guid ApiKey;

        public UserUnregisterRequest(Guid apiKey)
        {
            ApiKey = apiKey;
        }

        public override string ToString()
        {
            return $"UserUnregisterRequest(ApiKey={ApiKey})";
        }
    }
}