using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Api.Storage
{
    [Serializable]
    internal sealed class DataStorage
    {
        internal DataStorage([NotNull] List<Conversation> conversations, [NotNull] List<User> users)
        {
            Conversations = conversations ?? throw new ArgumentNullException(nameof(conversations));
            Users = users ?? throw new ArgumentNullException(nameof(users));
        }

        [NotNull] internal List<User> Users { get; set; }
        [NotNull] internal List<Conversation> Conversations { get; set; }

        public override string ToString()
        {
            return $"{nameof(DataStorage)}({nameof(Users)}={Users}, {nameof(Conversations)}={Conversations})";
        }
    }
}