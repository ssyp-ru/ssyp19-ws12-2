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
                var command = commandWords.GetOrNull(0);
                var args = commandWords.DropAt(0);

                switch (commandString)
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

                    case {} c when c.Equals("get-user-name"):
                        break;

                    default:
                        Console.WriteLine($"No command \"{commandString}\" available.");
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
                            .Where(it => it.TimeStamp >= TimeUtilities.CurrentTimeMillis() + 2000)
                            .ToList()
                            .ForEach(it =>
                                Console.WriteLine($"{Requests.RequestUserName(it.Sender)?.Result} => Y: {it.Value}"));

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