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
        [NotNull] internal static DataStorage DataStorage { get; private set; } = SaveDefaultData();

        [CanBeNull] private static IWebHost Host { get; set; }

        [NotNull]
        internal static ILogger Logger => Host?.Services.GetRequiredService<ILogger<Program>>() ??
                                          throw new Exception("Logger accessed before Host was initialized");

        [NotNull] private static string DataPath => "C:/Users/Commander Tvis/Data.json";

        [CanBeNull]
        internal static User GetUserByApiKey(Guid apiKey)
        {
            Debug.Assert(DataStorage != null, nameof(DataStorage) + " != null");
            return DataStorage.Users.Find(it => it.ApiKey == apiKey);
        }

        [CanBeNull]
        internal static User GetUserByName([NotNull] string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            Debug.Assert(DataStorage != null, nameof(DataStorage) + " != null");
            return DataStorage.Users.Find(it => it.Name == name);
        }

        private static bool HasUserWithName([NotNull] string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return GetUserByName(name) != null;
        }

        internal static bool HasUserWithApiKey(Guid apiKey)
        {
            return GetUserByApiKey(apiKey) != null;
        }

        internal static bool AddUser([NotNull] string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (!IsUserNameValid(name))
                return false;

            DataStorage.Users.Add(new User(name, Guid.NewGuid()));
            return true;
        }

        internal static bool DeleteUser([NotNull] User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            DataStorage.Users.Remove(user);
            return true;
        }

        internal static bool RenameUser([NotNull] User user, [NotNull] string newName)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (newName == null)
                throw new ArgumentNullException(nameof(newName));

            if (!IsUserNameValid(newName))
                return false;

            user.Name = newName;
            return true;
        }

        private static bool IsUserNameValid(string name)
        {
            return !HasUserWithName(name) && name.Length != 0 && name.Length <= 16 && !name.Contains(" ");
        }

        internal static void SaveData()
        {
            File.WriteAllText(DataPath, JsonConvert.SerializeObject(DataStorage, Formatting.Indented), Encoding.UTF8);
        }

        private static DataStorage SaveDefaultData()
        {
            DataStorage = new DataStorage(
                new List<Conversation>(),
                new List<User>
                {
                    new User(
                        "Haimuke",
                        Guid.Parse("12D5A4A4-F225-4245-A2E5-EA76AB042712"))
                });

            SaveData();
            return DataStorage;
        }

        private static void PullData()
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
            catch (ArgumentNullException)
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

            PullData();
            Host.Run();
        }
    }
}