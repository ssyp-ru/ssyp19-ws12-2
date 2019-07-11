using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Ssyp.Communicator.Common.Utilities;
using Ssyp.Communicator.CommonClient;

namespace Ssyp.Communicator.Cli
{
    internal static class Program
    {
        private static void Main()
        {
            var messageSyncing = MessageSyncing.StartMessageSyncing(
                () => { Console.WriteLine("Stopping the syncing thread..."); },
                it =>
                {
                    it.ToList().ForEach(
                        m => Console.WriteLine($"{Requests.RequestUserInfoOwn()?.Result.Name} => Y: {m.Value}"));
                });

            while (true)
            {
                var commandString = Console.ReadLine();

                if (commandString == null)
                {
                    Console.WriteLine("Stopping the console thread...");
                    messageSyncing.Abort();
                    break;
                }

                var commandWords = commandString.Split(' ').ToList();
                var args = commandWords.Drop(0);

                switch (commandWords.GetOrNull(0))
                {
                    case { } c when c.Equals("set-api-key", StringComparison.CurrentCultureIgnoreCase):
                        var newApiKey = args.GetOrNull(0);

                        if (newApiKey == null)
                        {
                            Console.WriteLine("Specify new API key!");
                            break;
                        }

                        if (!Regex.IsMatch(
                            newApiKey,
                            @"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$"))
                            break;

                        Requests.ApiKey = newApiKey;
                        Console.WriteLine("New API key set!");
                        break;


                    case {} c when c.Equals("update-name", StringComparison.CurrentCultureIgnoreCase):
                        var name = args.GetOrNull(0);

                        if (name == null)
                        {
                            Console.WriteLine("Specify your new name!");
                            break;
                        }

                        if (name.Length > 16)
                            Console.WriteLine("The new name can't be too long!");

                        Console.WriteLine(Requests.RequestUserModify(name).Result
                            ? $"You are {name} now!"
                            : "API key is invalid or name can't be parsed");

                        break;

                    case {} c when c.Equals("get-user-name", StringComparison.CurrentCultureIgnoreCase):
                        var response = Requests.RequestUserInfoOwn();

                        Console.WriteLine(
                            response?.Result == null
                                ? "API key is invalid"
                                : $"Your name is {response.Result?.Name}");

                        break;

                    case {} c when c.Equals("send", StringComparison.CurrentCultureIgnoreCase):
                        var receiver = args.GetOrNull(0);
                        var message = args.Drop(0).JoinToString(" ");
                        Console.WriteLine(receiver);
                        Console.WriteLine(message);

                        if (receiver == null || message.Length == 0)
                        {
                            Console.WriteLine("Specify receiver and non-empty message");
                            break;
                        }

                        Console.WriteLine(Requests.RequestConversionSend(receiver, message).Result
                            ? $"Y => {receiver}: {message}"
                            : "Receiver not found or API key is invalid");

                        break;

                    case {} c when c.Equals("conversation", StringComparison.OrdinalIgnoreCase):
                        var interlocutor = args.GetOrNull(0);

                        if (interlocutor == null)
                        {
                            Console.WriteLine("Specify interlocutor");
                            break;
                        }

                        Requests
                            .RequestConversationList()
                            ?.Result
                            .Conversations
                            .Find(it => it.Interlocutor.Equals(interlocutor))
                            .Messages
                            .ForEach(it => Console.WriteLine($"{it.Sender} - {it.Value}"));

                        break;

                    default:
                        Console.WriteLine($"No command \"{commandString}\" available.");
                        break;
                }
            }
        }
    }
}