using System;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common.Requests
{
    [Serializable]
    public sealed class UserUnregisterRequest : ICommunicatorRequest
    {
        public UserUnregisterRequest([NotNull] string apiKey) =>
            ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));

        [NotNull] public string ApiKey { get; set; }

        [NotNull]
        public override string ToString() => $"{nameof(UserUnregisterRequest)}({nameof(ApiKey)}={ApiKey})";
    }
}