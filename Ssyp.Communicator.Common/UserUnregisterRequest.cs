using System;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common
{
    [Serializable]
    public sealed class UserUnregisterRequest : ICommunicatorRequest
    {
        public UserUnregisterRequest(Guid apiKey) => ApiKey = apiKey;

        public Guid ApiKey { get; set; }

        [NotNull]
        public override string ToString() => $"{nameof(UserUnregisterRequest)}({nameof(ApiKey)}={ApiKey})";
    }
}