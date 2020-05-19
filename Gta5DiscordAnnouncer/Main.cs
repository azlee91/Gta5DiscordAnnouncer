using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Reflection;

using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Net;

namespace Gta5DiscordAnnouncer {
    public class Config {
        /// <summary>
        /// Holds the resource config that can be changed through config.json
        /// </summary>
        public String WebHookUrl { get; set; }
        public Boolean DebugMode { get; set; }
    }

    public class Main : BaseScript {
        private readonly string configPath = GetResourcePath(GetCurrentResourceName()) + @"/config.json";
        private Config appConfig = null;
        private static readonly HttpClient client = new HttpClient();

        private void DebugLog(string message) {
            if (appConfig.DebugMode) {
                Console.WriteLine("[DiscordAnnouncer] " + message);
            }
        }

        public Main() {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };
            Console.WriteLine("[DiscordAnnouncer] Loading announcer script");

            // Load configs
            LoadSettings();
            EventHandlers["esx:playerLoaded"] += new Action<string, dynamic>(HandleOnPlayerLoaded);
        }

        /// <summary>
        /// Loads the application settings from a configuration file
        /// </summary>
        private void LoadSettings() {
            if (File.Exists(configPath)) { // Read in config file
                Console.WriteLine($"[DiscordAnnouncer] Using config file found for discord_announcer in {configPath}");
                String configString = File.ReadAllText(configPath);
                appConfig = JsonConvert.DeserializeObject<Config>(configString);
            }
            else { // Create the config file if it doesn't exist
                Console.WriteLine($"[DiscordAnnouncer] No config file found for discord_announcer, creating a new one in {configPath}");
                using (StreamWriter outFile = File.CreateText(configPath)) {
                    Config newConfig = new Config {
                        WebHookUrl = "",
                        DebugMode = false
                    };
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(outFile, newConfig);
                }
            }
        }

        private async void SendDiscordWebHook(string message) {
            DebugLog($"Sending message to discord webhook url: {appConfig.WebHookUrl}");
            Dictionary<string, string> data = new Dictionary<string, string> { { "content", message } };
            var response = await client.PostAsync(
                appConfig.WebHookUrl,
                new StringContent(
                    JsonConvert.SerializeObject(data),
                    Encoding.UTF8,
                    "application/json"
                )
            );
            DebugLog($"Post response: {(int)response.StatusCode}");
        }

        private void HandleOnPlayerLoaded(string playerId, dynamic esxPlayer) {
            string message = $"{esxPlayer.getName()} has logged in with ID: {esxPlayer.getIdentifier()}!";
            DebugLog(message);
            SendDiscordWebHook(message);
        }
    }
}
