using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using JetBrains.Annotations;
using Ssyp.Communicator.Common.Utilities;
using Ssyp.Communicator.CommonClient;

namespace Ssyp.Communicator.Cli
{
    internal static class Program
    {
        private static bool Syncing { get; set; } = true;

        private static void Main()
        {
            var messageSyncing = InitializeMessageSyncing();

            while (true)
            {
                var commandString = Console.ReadLine();

                if (commandString == null)
                {
                    Console.WriteLine("Stopping the console thread...");
                    Syncing = false;
                    break;
                }

                var commandWords = commandString.Split(' ').ToList();
                var args = commandWords.Drop(0);

                switch (commandWords.GetOrNull(0))
                {
                    case {} c when c.Equals("set-api-key", StringComparison.CurrentCultureIgnoreCase):
                        ChangeApiKey(args);
                        break;


                    case {} c when c.Equals("update-name", StringComparison.CurrentCultureIgnoreCase):
                        UpdateNickname(args);
                        break;

                    case {} c when c.Equals("get-user-name", StringComparison.CurrentCultureIgnoreCase):
                        GetNickname();
                        break;

                    case {} c when c.Equals("send", StringComparison.CurrentCultureIgnoreCase):
                        SendMessage(args);
                        break;

                    case {} c when c.Equals("conversation", StringComparison.OrdinalIgnoreCase):
                        ShowConversation(args);
                        break;

                    default:
                        Console.WriteLine($"No command \"{commandString}\" available.");
                        break;
                }
            }
        }

        private static Thread InitializeMessageSyncing()
        {
            var messageSyncing = new Thread(() =>
            {
                while (Syncing)
                {
                    try
                    {
                        Requests
                            .RequestConversationList()
                            ?.Result
                            ?.Conversations
                            .SelectMany(it1 => it1.Messages)
                            .Where(it2 => it2.TimeStamp >= TimeUtilities.CurrentTimeMillis() - 2000)
                            .ToList()
                            .ForEach(m =>
                                Console.WriteLine($"{Requests.RequestUserInfoOwn()?.Result.Name} => Y: {m.Value}"));

                        Thread.Sleep(2000);
                    }
                    catch (ThreadAbortException)
                    {
                        Console.WriteLine("Stopping the syncing thread...");
                        break;
                    }
                }
            });

            messageSyncing.Start();
            return messageSyncing;
        }

        private static void ShowConversation([NotNull] IReadOnlyList<string> args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            var interlocutor = args.GetOrNull(0);

            if (interlocutor == null)
            {
                Console.WriteLine("Specify interlocutor");
                return;
            }

            Requests
                .RequestConversationList()
                ?.Result
                .Conversations
                .Find(it => it.Interlocutor.Equals(interlocutor))
                .Messages
                .ForEach(it => Console.WriteLine($"{it.Sender} - {it.Value}"));
        }

        private static void SendMessage([NotNull] IReadOnlyList<string> args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            var receiver = args.GetOrNull(0);
            var message = args.Drop(0).JoinToString(" ");

            if (receiver == null || message.Length == 0)
            {
                Console.WriteLine("Specify receiver and non-empty message");
                return;
            }

            Console.WriteLine(Requests.RequestConversationSend(receiver, message).Result
                ? $"Y => {receiver}: {message}"
                : "Receiver not found or API key is invalid");
        }

        private static void GetNickname()
        {
            var response = Requests.RequestUserInfoOwn();

            Console.WriteLine(
                response?.Result == null
                    ? "API key is invalid"
                    : $"Your name is {response.Result?.Name}");
        }

        private static void UpdateNickname([NotNull] IReadOnlyList<string> args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            var name = args.GetOrNull(0);

            if (name == null)
            {
                Console.WriteLine("Specify your new name!");
                return;
            }

            if (name.Length > 16)
                Console.WriteLine("The new name can't be too long!");

            Console.WriteLine(Requests.RequestUserModify(name).Result
                ? $"You are {name} now!"
                : "API key is invalid or name can't be parsed");
        }

        private static void ChangeApiKey([NotNull] IReadOnlyList<string> args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            var newApiKey = args.GetOrNull(0);

            if (newApiKey == null)
            {
                Console.WriteLine("Specify new API key!");
                return;
            }

            if (!Regex.IsMatch(
                newApiKey,
                @"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$"))
                return;

            Requests.ApiKey = newApiKey;
            Console.WriteLine("New API key set!");
        }
    }
}