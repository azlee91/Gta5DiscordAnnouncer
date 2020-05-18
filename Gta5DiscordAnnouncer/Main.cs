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

namespace Gta5DiscordAnnouncer {
    public class Config {
        public String WebHookUrl { get; set; }
    }

    public class Main : BaseScript {
        // Webhook address = https://discordapp.com/api/webhooks/711312546413412363/aTU6dfSsQvn8rHp452UcgF9zxW3hMTnPJbJ_mnqAHjY8kQaxOeLUmEUvmePl2VKlU_S-
        // readonly String assemblyPath = AppDomain.CurrentDomain.BaseDirectory;
        readonly String configPath = GetResourcePath(GetCurrentResourceName()) + @"/config.json";
        private Config appConfig = null;

        public Main() {
            // Load configs
            LoadSettings();

            Console.WriteLine("Loading announcer script");
            EventHandlers["esx:playerLoaded"] += new Action<string, dynamic>(HandleOnPlayerLoaded);
        }

        private void LoadSettings() {
            if (File.Exists(configPath)) { // Read in config file
                Console.WriteLine($"Config file found for discord_announcer in {configPath}. Using it.");
                String configString = File.ReadAllText(configPath);
                appConfig = JsonConvert.DeserializeObject<Config>(configString);
            }
            else { // Create the config file if it doesn't exist
                Console.WriteLine($"No config file found for discord_announcer, creating a new one in {configPath}");
                using (StreamWriter outFile = File.CreateText(configPath)) {
                    Config newConfig = new Config {
                        WebHookUrl = ""
                    };
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(outFile, newConfig);
                }
            }
        }

        private void SendDiscordWebHook() {

        }

        private void HandleOnPlayerLoaded(string playerId, dynamic esxPlayer) {
            Console.WriteLine($"An esx player [{esxPlayer.getName()}] has logged in with ID: {esxPlayer.getIdentifier()}!");
        }
    }
}
