using System;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common
{
    [Serializable]
    public sealed class UserModifyRequest : ICommunicatorRequest
    {
        public UserModifyRequest(Guid apiKey, [NotNull] string name)
        {
            ApiKey = apiKey;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        [NotNull] public string Name { get; set; }
        public Guid ApiKey { get; set; }

        [NotNull]
        public override string ToString()
        {
            return $"{nameof(UserModifyRequest)}({nameof(ApiKey)}={ApiKey}, {nameof(Name)}={Name})";
        }
    }
}