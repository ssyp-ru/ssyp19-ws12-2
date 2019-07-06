using System;
using CuttingEdge.Conditions;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common
{
    [Serializable]
    public struct UserInfoResponse
    {
        [NotNull] public string Name;

        public UserInfoResponse([NotNull] string name)
        {
            Condition.Requires(name).IsNotNull();
            Name = name;
        }

        public override string ToString()
        {
            return $"UserInfoResponse(Name={Name})";
        }
    }
}