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
            Condition.Requires(conversations, "conversations").IsNotNull();
            Conversations = conversations;
        }

        [NotNull] public List<Conversation> Conversations { get; }

        [NotNull]
        public override string ToString()
        {
            return $"ConversationListResponse(Conversations={Conversations})";
        }

        [Serializable]
        public sealed class Conversation
        {
            public Conversation(long interlocutor, [NotNull] List<Message> messages)
            {
                Condition.Requires(messages, "messages").IsNotNull();
                Interlocutor = interlocutor;
                Messages = messages;
            }

            public long Interlocutor { get; }
            [NotNull] public List<Message> Messages { get; }

            [NotNull]
            public override string ToString()
            {
                return $"Conversation(Interlocutor={Interlocutor}, Messages={Messages}";
            }

            [Serializable]
            public sealed class Message
            {
                public long Sender;
                public long TimeStamp;
                [NotNull] public string Value;

                public Message(long sender, string value, long timeStamp)
                {
                    Condition.Requires(value, "value").IsNotNull();
                    Sender = sender;
                    Value = value;
                    TimeStamp = timeStamp;
                }

                [NotNull]
                public override string ToString()
                {
                    return $"Message(Sender={Sender}, Value={Value}, TimeStamp={TimeStamp}";
                }
            }
        }
    }
}