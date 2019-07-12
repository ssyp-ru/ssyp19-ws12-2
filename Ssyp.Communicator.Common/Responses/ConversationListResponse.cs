using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common.Responses
{
    [Serializable]
    public sealed class ConversationListResponse
    {
        public ConversationListResponse([NotNull] List<Conversation> conversations) => Conversations =
            conversations ?? throw new ArgumentNullException(nameof(conversations));

        [NotNull] public List<Conversation> Conversations { get; }

        [NotNull]
        public override string ToString() =>
            $"{nameof(ConversationListResponse)}({nameof(Conversations)}={Conversations})";

        [Serializable]
        public sealed class Conversation
        {
            public Conversation(string interlocutor, [NotNull] List<Message> messages)
            {
                Interlocutor = interlocutor;
                Messages = messages ?? throw new ArgumentNullException(nameof(messages));
            }

            public string Interlocutor { get; set; }
            [NotNull] public List<Message> Messages { get; set; }

            [NotNull]
            public override string ToString() =>
                $"{nameof(Conversation)}({nameof(Interlocutor)}={Interlocutor}, {nameof(Messages)}={Messages}";

            [Serializable]
            public sealed class Message
            {
                public string Sender;
                public long TimeStamp;
                [NotNull] public string Value;

                public Message(string sender, [NotNull] string value, long timeStamp)
                {
                    Sender = sender;
                    Value = value ?? throw new ArgumentNullException(nameof(value));
                    TimeStamp = timeStamp;
                }

                [NotNull]
                public override string ToString() =>
                    $"{nameof(Message)}({nameof(Sender)}={Sender}, {nameof(Value)}={Value}, " +
                    $"{nameof(TimeStamp)}={TimeStamp}";
            }
        }
    }
}