using System.Linq;
using System.Collections.Generic;
using Vintagestory.API.Server;

namespace HungerGames
{
    public class Server
    {
        public void OnPlayerJoin(ICoreServerAPI sapi, Globals globals, List<Tribute> tributes, IServerPlayer byPlayer)
        {
            Tribute foundTribute = tributes.FirstOrDefault(_ => _.playerUID == byPlayer.PlayerUID);

            if (foundTribute == null)
            {
				// Tribute does not exist, so create one
                tributes.Add(new Tribute() { playerUID = byPlayer.PlayerUID, isConnected = true });
                globals.tributesConnected++;
            }
        	else
            {
				// Tribute Exists, so just update it
                foundTribute.isConnected = true;
                globals.tributesConnected++;
                globals.tributesAlive++;
            }
        }

        public void OnPlayerDisconnect(ICoreServerAPI sapi, Globals globals, List<Tribute> tributes, IServerPlayer byPlayer)
        {
            Tribute foundTribute = tributes.FirstOrDefault(_ => _.playerUID == byPlayer.PlayerUID);

			if(foundTribute == null)
			{
				// There is an error, you can't connect without being on the server at least once
				sapi.Logger.Error($"PlayerUID: {byPlayer.PlayerUID} does not exist in the hungergames_config.json, but is disconnecting!");
			}
			else
			{
				// Tribute exists, so update the connection
				foundTribute.isConnected = false;
                globals.tributesConnected--;
			}
        }

        public void OnPlayerDeath(ICoreServerAPI sapi, Globals globals, List<Tribute> tributes, IServerPlayer byPlayer)
        {
            Tribute foundTribute = tributes.FirstOrDefault(_ => _.playerUID == byPlayer.PlayerUID);

            if(foundTribute == null)
            {
                // There is an error, you can't die without being on the server at least once
                sapi.Logger.Error($"PlayerUID: {byPlayer.PlayerUID} does not exist in the hungergames_config.json, but has died!");
            }
            else
            {
                // tribute exists, log it to Tributes List
                foundTribute.isAlive = false;
                globals.tributesAlive--;
            }
        }

        public int CountTributesAlive(List<Tribute> tributes)
        {
            int count = 0;
			foreach(Tribute t in tributes)
			{
				if(t.isAlive)
                {
                    count++;
                }
			}

            return count;
        }
    }
}