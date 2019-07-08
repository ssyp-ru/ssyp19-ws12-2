using System;
using CuttingEdge.Conditions;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common
{
    [Serializable]
    public sealed class UserInfoResponse
    {
        public UserInfoResponse([NotNull] string name)
        {
            Condition.Requires(name).IsNotNull();
            Name = name;
        }

        [NotNull] public string Name { get; set; }

        [NotNull]
        public override string ToString()
        {
            return $"UserInfoResponse(Name={Name})";
        }
    }
}