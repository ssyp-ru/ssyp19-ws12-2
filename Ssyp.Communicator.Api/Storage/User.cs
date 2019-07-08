using System;
using CuttingEdge.Conditions;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Api.Storage
{
    [Serializable]
    public sealed class User
    {
        public User([NotNull] string name, long userID, Guid apiKey)
        {
            Condition.Requires(name, "name").IsNotNull();
            Name = name;
            UserID = userID;
            ApiKey = apiKey;
        }

        public Guid ApiKey { get; set; }

        public long UserID { get; }

        [NotNull] public string Name { get; set; }

        public override string ToString()
        {
            return $"User(Name={Name}, UserID={UserID}, ApiKey={ApiKey})";
        }
    }
}