using System;
using CuttingEdge.Conditions;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common
{
    [Serializable]
    public sealed class UserModifyRequest : ICommunicatorRequest
    {
        public UserModifyRequest(Guid apiKey, [NotNull] string name)
        {
            Condition.Requires(name, nameof(name)).IsNotNull();
            ApiKey = apiKey;
            Name = name;
        }

        [NotNull] public string Name { get; set; }
        public Guid ApiKey { get; set; }

        [NotNull]
        public override string ToString() =>
            $"{nameof(UserModifyRequest)}({nameof(ApiKey)}={ApiKey}, {nameof(Name)}={Name})";
    }
}