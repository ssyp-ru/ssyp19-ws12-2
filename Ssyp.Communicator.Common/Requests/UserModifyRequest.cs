using System;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common.Requests
{
    [Serializable]
    public sealed class UserModifyRequest : ICommunicatorRequest
    {
        public UserModifyRequest([NotNull] string apiKey, [NotNull] string name)
        {
            ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        [NotNull] public string Name { get; set; }
        [NotNull] public string ApiKey { get; set; }

        [NotNull]
        public override string ToString()
        {
            return $"{nameof(UserModifyRequest)}({nameof(ApiKey)}={ApiKey}, {nameof(Name)}={Name})";
        }
    }
}