using System;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common.Responses
{
    [Serializable]
    public sealed class UserInfoOwnResponse
    {
        public UserInfoOwnResponse([NotNull] string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        [NotNull] public string Name { get; set; }
    }
}