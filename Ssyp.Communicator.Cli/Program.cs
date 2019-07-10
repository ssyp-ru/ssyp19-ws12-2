using System;
using System.Threading;

namespace Ssyp.Communicator.Cli
{
    internal static class Program
    {
        private static void Main()
        {
            StartMethodSync(out var messageSyncing);

            while (true)
            {
                var command = Console.ReadLine();

                if (command == null)
                    break;
            }
        }

        private static void StartMethodSync(out Thread messageSyncing)
        {
            messageSyncing = new Thread(() => { });

            messageSyncing.Start();
        }
    }
}