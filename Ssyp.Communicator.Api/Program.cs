using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ssyp.Communicator.Api.Storage;

namespace Ssyp.Communicator.Api
{
    internal static class Program
    {
        [NotNull] internal static DataStorage DataStorage { get; private set; }

        [NotNull] internal static ILogger Logger { get; } = LoggerFactory.Create(it => { }).CreateLogger("common");

        private static string DataPath { get; } = Path.GetFullPath("C:/Users/Commander Tvis/Data.json");

        [CanBeNull]
        internal static User GetUserByApiKey(Guid apiKey)
        {
            return DataStorage.Users.Find(it => it.ApiKey.Equals(apiKey));
        }

        [CanBeNull]
        internal static User GetUserByUserID(Guid userID)
        {
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
        }

        internal static void SaveDefaultData()
        {
            DataStorage = new DataStorage(new List<Conversation>(), new List<User>
            {
                new User("Haimuke", Guid.NewGuid(), Guid.NewGuid())
            });

            SaveData();
        }

        internal static void PullData()
        {
            try
            {
                DataStorage =
                    JsonConvert.DeserializeObject<DataStorage>(File.ReadAllText(DataPath, Encoding.UTF8));
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

            if (!File.Exists(DataPath))
                SaveDefaultData();

            PullData();
            DataStorage = new DataStorage(new List<Conversation>(), new List<User>());
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder([NotNull] string[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}