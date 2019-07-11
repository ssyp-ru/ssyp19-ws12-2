using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;
using Ssyp.Communicator.Common.Responses;
using Ssyp.Communicator.Common.Utilities;

namespace Ssyp.Communicator.CommonClient
{
    public static class MessageSyncing
    {
        public static Thread StartMessageSyncing(
            [NotNull] Action onThreadAbort,
            [NotNull] Action<IEnumerable<ConversationListResponse.Conversation.Message>> onReceivedNewMessages,
            int timeout = 2000)
        {
            if (onThreadAbort == null)
                throw new ArgumentNullException(nameof(onThreadAbort));

            if (onReceivedNewMessages == null)
                throw new ArgumentNullException(nameof(onReceivedNewMessages));

            var messageSyncing = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        onReceivedNewMessages(
                            Requests
                                .RequestConversationList()
                                ?.Result
                                ?.Conversations
                                .SelectMany(it => it.Messages)
                                .Where(it => it.TimeStamp >= TimeUtilities.CurrentTimeMillis() - timeout));

                        Thread.Sleep(timeout);
                    }
                    catch (ThreadAbortException)
                    {
                        onThreadAbort();
                        break;
                    }
                }
            });

            messageSyncing.Start();
            return messageSyncing;
        }
    }
}