using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CitizenFX.Core;
using static CitizenFX.Core.Native.API;


namespace Gta5DiscordAnnouncer {
    public class Main : BaseScript {
        public Main() {
            Console.WriteLine("Loading announcer script");
            EventHandlers["esx:playerLoaded"] += new Action<string, dynamic>(HandleOnPlayerLoaded);
        }

        private void HandleOnPlayerLoaded(string playerId, dynamic esxPlayer) {
            Console.WriteLine($"An esx player [{esxPlayer.getName()}] has loaded with ID: {esxPlayer.getIdentifier()}!");
        }
    }
}
