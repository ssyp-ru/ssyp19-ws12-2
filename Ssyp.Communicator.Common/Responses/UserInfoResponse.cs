using System;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common.Responses
{
    [Serializable]
    public sealed class UserInfoResponse
    {
        public UserInfoResponse([NotNull] string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        [NotNull] public string Name { get; set; }

        [NotNull]
        public override string ToString()
        {
            return $"{nameof(UserInfoResponse)}({nameof(Name)}={Name})";
        }
    }
}