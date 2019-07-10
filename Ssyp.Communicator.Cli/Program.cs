using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Ssyp.Communicator.Common.Utilities;

namespace Ssyp.Communicator.Cli
{
    internal static class Program
    {
        private static void Main()
        {
            StartMethodSync(out var messageSyncing);

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
                var args = commandWords.DropAt(0);

                switch (commandWords.GetOrNull(0))
                {
                    case { } c when c.Equals("set-api-key", StringComparison.CurrentCultureIgnoreCase):
                        Console.WriteLine("Set");
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


                    case {} c when c.Equals("update-name"):
                        var name = args.GetOrNull(0);

                        if (name == null)
                        {
                            Console.WriteLine("Specify your new name!");
                            break;
                        }

                        if (name.Length > 16)
                            Console.WriteLine("The new name can't be too long!");

                        Requests.RequestUserModify(name);
                        break;

                    case {} c when c.Equals("get-user-name", StringComparison.CurrentCultureIgnoreCase):
                        Console.WriteLine($"Your name is {Requests.RequestUserInfoOwn()?.Result.Name}");
                        break;

                    case {} c when c.Equals("send", StringComparison.CurrentCultureIgnoreCase):
                        var receiver = args.GetOrNull(0);
                        var message = args.DropAt(0);

                        if (receiver == null || message.IsEmpty())
                        {
                            Console.WriteLine("Specify receiver and non-empty message");
                            break;
                        }

                        if (Requests.RequestConversionSend(receiver, message.JoinToString("")).Result)
                            Console.WriteLine($"Y => {receiver}: {message}");
                        else
                            Console.WriteLine("Receiver not found or API key is invalid");
                        break;

                    default:
                        Console.WriteLine($"No command \"{commandString}\" available.");
                        break;
                }
            }
        }


        private static void StartMethodSync(out Thread messageSyncing)
        {
            messageSyncing = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        Requests.RequestConversationList()
                            ?.Result
                            .Conversations
                            .SelectMany(it => it.Messages)
                            .Where(it => it.TimeStamp >= TimeUtilities.CurrentTimeMillis() - 2000)
                            .ToList()
                            .ForEach(it =>
                                Console.WriteLine($"{Requests.RequestUserInfoOwn()} => Y: {it.Value}"));

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
        }
    }
}