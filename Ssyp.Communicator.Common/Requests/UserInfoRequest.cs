using System;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common.Requests
{
    [Serializable]
    public sealed class UserInfoRequest : ICommunicatorRequest
    {
        public UserInfoRequest([NotNull] string apiKey, [NotNull] string userID)
        {
            ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            UserID = userID ?? throw new ArgumentNullException(nameof(userID));
        }

        [NotNull] public string UserID { get; set; }
        [NotNull] public string ApiKey { get; set; }

        [NotNull]
        public override string ToString() =>
            $"{nameof(UserInfoRequest)}({nameof(ApiKey)}={ApiKey}, {nameof(UserID)}={UserID}";
    }
}