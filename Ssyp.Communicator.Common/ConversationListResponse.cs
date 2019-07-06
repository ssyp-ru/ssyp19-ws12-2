using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using JetBrains.Annotations;

namespace Ssyp.Communicator.Common
{
    [Serializable]
    public struct ConversationListResponse
    {
        [NotNull] public List<Conversation> Conversations;

        public ConversationListResponse([NotNull] List<Conversation> conversations)
        {
            Condition.Requires(conversations, "conversations").IsNotNull();
            Conversations = conversations;
        }

        public override string ToString()
        {
            return $"ConversationListResponse(Conversations={Conversations})";
        }

        [Serializable]
        public struct Conversation
        {
            public long Interlocutor;
            [NotNull] public List<Message> Messages;

            public Conversation(long interlocutor, [NotNull] List<Message> messages)
            {
                Condition.Requires(messages, "messages").IsNotNull();
                Interlocutor = interlocutor;
                Messages = messages;
            }

            public override string ToString()
            {
                return $"Conversation(Interlocutor={Interlocutor}, Messages={Messages}";
            }

            [Serializable]
            public struct Message
            {
                public long Sender;
                [NotNull] public string Value;
                public long TimeStamp;

                public override string ToString()
                {
                    return $"Message(Sender={Sender}, Value={Value}, TimeStamp={TimeStamp}";
                }

                public Message(long sender, string value, long timeStamp)
                {
                    Condition.Requires(value, "value").IsNotNull();
                    Sender = sender;
                    Value = value;
                    TimeStamp = timeStamp;
                }
            }
        }
    }
}