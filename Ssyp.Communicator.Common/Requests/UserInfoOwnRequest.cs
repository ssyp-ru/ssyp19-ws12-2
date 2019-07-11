using System;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common.Requests
{
    [Serializable]
    public sealed class UserInfoOwnRequest : ICommunicatorRequest
    {
        public UserInfoOwnRequest([NotNull] string apiKey)
        {
            ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }

        public string ApiKey { get; set; }
    }
}