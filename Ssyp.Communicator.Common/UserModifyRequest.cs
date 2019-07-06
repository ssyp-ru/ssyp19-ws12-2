using System;
using CuttingEdge.Conditions;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common
{
    [Serializable]
    public struct UserModifyRequest
    {
        public Guid ApiKey;
        [NotNull] public string Name;

        public UserModifyRequest(Guid apiKey, [NotNull] string name)
        {
            Condition.Requires(name, "name").IsNotNull();
            ApiKey = apiKey;
            Name = name;
        }

        public override string ToString()
        {
            return $"UserModifyRequest(ApiKey={ApiKey}, Name={Name})";
        }
    }
}