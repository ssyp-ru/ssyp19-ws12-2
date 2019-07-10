using System;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common.Responses
{
    [Serializable]
    public sealed class UserInfoOwnResponse
    {
        public UserInfoOwnResponse([NotNull] string userID, [NotNull] string name)
        {
            UserID = userID ?? throw new ArgumentNullException(nameof(userID));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        [NotNull] public string UserID { get; set; }
        [NotNull] public string Name { get; set; }
    }
}