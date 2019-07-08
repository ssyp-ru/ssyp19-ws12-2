using System;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Api.Storage
{
    [Serializable]
    public sealed class User
    {
        private bool Equals([CanBeNull] User other) => other != null && ApiKey.Equals(other.ApiKey);

        public override bool Equals([CanBeNull] object obj) => obj is User other && Equals(other);

        public override int GetHashCode() => ApiKey.GetHashCode();

        public User([NotNull] string name, Guid userID, Guid apiKey)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            UserID = userID;
            ApiKey = apiKey;
        }

        public Guid ApiKey { get; }

        public Guid UserID { get; }

        [NotNull] public string Name { get; set; }

        [NotNull]
        public override string ToString() =>
            $"{nameof(User)}({nameof(Name)}={Name}, {nameof(UserID)}={UserID}, {nameof(ApiKey)}={ApiKey})";
    }
}