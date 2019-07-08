using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ssyp.Communicator.Api.Storage;

namespace Ssyp.Communicator.Api
{
    internal static class Program
    {
        [NotNull] internal static DataStorage DataStorage { get; private set; }

        [NotNull] internal static ILogger Logger { get; } = LoggerFactory.Create(it => { }).CreateLogger("common");

        [CanBeNull]
        internal static User GetUserByApiKey(Guid apiKey)
        {
            return DataStorage.Users.Find(it => it.ApiKey.Equals(apiKey));
        }

        [CanBeNull]
        internal static User GetUserByUserID(long userID) => DataStorage.Users.Find(it => it.UserID == userID);

        internal static bool HasUserWithUsedID(long userID) => GetUserByUserID(userID) == null;

        internal static bool HasUserWithApiKey(Guid apiKey)
        {
            return GetUserByApiKey(apiKey) == null;
        }

        internal static void Main(string[] args)
        {
            DataStorage = new DataStorage(new List<Conversation>(), new List<User>());
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}