using System;
using CuttingEdge.Conditions;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common
{
    [Serializable]
    public struct User
    {
        public User(Guid apiKey, long userId, [NotNull] string name)
        {
            Condition.Requires(name).IsNotNull();
            ApiKey = apiKey;
            UserID = userId;
            Name = name;
        }

        public Guid ApiKey;

        public long UserID;

        [NotNull]
        public string Name;
    }
}