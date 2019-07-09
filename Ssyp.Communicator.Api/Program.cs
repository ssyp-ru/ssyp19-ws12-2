using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ssyp.Communicator.Api.Storage;

namespace Ssyp.Communicator.Api
{
    internal class Program
    {
        [CanBeNull] internal static DataStorage DataStorage { get; private set; }

        [CanBeNull] private static IWebHost Host { get; set; }

        [NotNull]
        internal static ILogger Logger => Host?.Services.GetRequiredService<ILogger<Program>>() ??
                                          throw new Exception("Logger accessed before Host was initialized");

        private static string DataPath { get; } = Path.GetFullPath("C:/Users/Commander Tvis/Data.json");

        [CanBeNull]
        internal static User GetUserByApiKey(Guid apiKey)
        {
            Debug.Assert(DataStorage != null, nameof(DataStorage) + " != null");
            return DataStorage.Users.Find(it => it.ApiKey.Equals(apiKey));
        }

        [CanBeNull]
        internal static User GetUserByUserID(Guid userID)
        {
            Debug.Assert(DataStorage != null, nameof(DataStorage) + " != null");
            return DataStorage.Users.Find(it => it.UserID.Equals(userID));
        }

        internal static bool HasUserWithUsedID(Guid userID)
        {
            return GetUserByUserID(userID) == null;
        }

        internal static bool HasUserWithApiKey(Guid apiKey)
        {
            return GetUserByApiKey(apiKey) == null;
        }

        internal static void SaveData()
        {
            File.WriteAllText(DataPath, JsonConvert.SerializeObject(DataStorage), Encoding.UTF8);
            Logger.LogDebug($"Updated data in {DataPath}");
        }

        private static void SaveDefaultData()
        {
            DataStorage = new DataStorage(
                new List<Conversation>(),
                new List<User>
                {
                    new User("Haimuke", Guid.NewGuid(), Guid.NewGuid())
                });

            SaveData();
            Logger.LogDebug($"Saved default data. DataStorage: {DataStorage}");
        }

        private static void PullData()
        {
            try
            {
                DataStorage =
                    JsonConvert.DeserializeObject<DataStorage>(File.ReadAllText(DataPath, Encoding.UTF8));

                Logger.LogDebug($"Pulled data from {DataPath}");
            }
            catch (JsonSerializationException)
            {
                SaveDefaultData();
            }
        }

        internal static void Main([NotNull] string[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            Host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile(
                            $"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                            true,
                            true)
                        .AddEnvironmentVariables();
                })
                .ConfigureLogging((context, builder) =>
                    builder
                        .AddConfiguration(context.Configuration.GetSection("Logging"))
                        .AddConsole()
                        .AddDebug()
                        .AddEventSourceLogger())
                .UseStartup<Startup>()
                .Build();

            if (!File.Exists(DataPath))
                SaveDefaultData();

            PullData();
            DataStorage = new DataStorage(new List<Conversation>(), new List<User>());

            Host.Run();
        }
    }
}