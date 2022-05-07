using System.Linq;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

[assembly: ModInfo( "HungerGames", Description = "Hunger Games in VintageStory", Version = "0.1.0", Website = "website", Authors = new []{ "treeman0013" } )]

namespace HungerGames
{
	public class HungerGames : ModSystem
	{
		ICoreAPI api;
		ICoreServerAPI sapi;

		// List of Tributes
		List<Tribute> tributes = new List<Tribute>();

		public override void Start(ICoreAPI api)
		{
			base.Start(api);

			this.api = api;
		}

		public override void StartServerSide(ICoreServerAPI sapi)
		{
			// Setup the Server API
			base.StartServerSide(sapi);

			this.sapi = sapi;

			// Setup server events - Join, Disconnect, and Death
			sapi.Event.PlayerJoin += OnPlayerJoin;
			sapi.Event.PlayerDisconnect += OnPlayerDisconnect;
			sapi.Event.PlayerDeath += OnPlayerDeath;
			sapi.Event.SaveGameLoaded += OnSaveGameLoaded;
			sapi.Event.GameWorldSave += OnGameWorldSave;
		}

		private void OnSaveGameLoaded()
		{
			sapi.LoadModConfig<List<Tribute>>("hungergames_config.json");
		}

		private void OnGameWorldSave()
		{
			sapi.StoreModConfig<List<Tribute>>(tributes, "hungergames_config.json");
		}

		private void OnPlayerJoin(IServerPlayer byPlayer)
		{
			Tribute foundTribute = tributes.FirstOrDefault(_ => _.playerUID == byPlayer.PlayerUID);

            if (foundTribute == null)
            {
				// Tribute does not exist, create one
                tributes.Add(new Tribute() { playerUID = byPlayer.PlayerUID, isConnected = true });
            }
            else
            {
				// Tribute Exists, so just update it
                foundTribute.isConnected = true;
            }
		}

		private void OnPlayerDisconnect(IServerPlayer byPlayer)
		{
			Tribute foundTribute = tributes.FirstOrDefault(_ => _.playerUID == byPlayer.PlayerUID);

			if(foundTribute == null)
			{
				// There is an error, you can't connect without being on the server at least once
				api.Logger.Error($"PlayerUID: {byPlayer.PlayerUID} does not exist in hungergames_config.json, but is disconnecting!");
			}
			else
			{
				// Tribute exists, so update the connection
				foundTribute.isConnected = false;
			}
		}

		private void OnPlayerDeath(IServerPlayer byPlayer, DamageSource damageSource)
		{
			Tribute foundTribute = tributes.FirstOrDefault(_ => _.playerUID == byPlayer.PlayerUID);

			foundTribute.isAlive = false;
		}
	}
}