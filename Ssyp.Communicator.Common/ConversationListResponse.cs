using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common
{
    [Serializable]
    public sealed class ConversationListResponse
    {
        public ConversationListResponse([NotNull] List<Conversation> conversations)
        {
            Condition.Requires(conversations, nameof(conversations)).IsNotNull();
            Conversations = conversations;
        }

        [NotNull] public List<Conversation> Conversations { get; }

        [NotNull]
        public override string ToString() =>
            $"${nameof(ConversationListResponse)}({nameof(Conversations)}={Conversations})";

        [Serializable]
        public sealed class Conversation
        {
            public Conversation(Guid interlocutor, [NotNull] List<Message> messages)
            {
                Condition.Requires(messages, nameof(messages)).IsNotNull();
                Interlocutor = interlocutor;
                Messages = messages;
            }

            public Guid Interlocutor { get; }
            [NotNull] public List<Message> Messages { get; }

            [NotNull]
            public override string ToString() =>
                $"{nameof(Conversation)}({nameof(Interlocutor)}={Interlocutor}, {nameof(Messages)}={Messages}";

            [Serializable]
            public sealed class Message
            {
                public Guid Sender;
                public long TimeStamp;
                [NotNull] public string Value;

                public Message(Guid sender, string value, long timeStamp)
                {
                    Condition.Requires(value, nameof(value)).IsNotNull();
                    Sender = sender;
                    Value = value;
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