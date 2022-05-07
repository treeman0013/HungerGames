using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Client;
using Vintagestory.API.Server;

// Mod information
[assembly: ModInfo( "HungerGames",
Description = "Hunger Games in VintageStory",
Version = "0.1.0",
Website = "https://github.com/treeman0013/HungerGames.git",
Authors = new []{ "treeman0013" } )]

namespace HungerGames
{
	public class HungerGames : ModSystem
	{
		// API references
		ICoreAPI api;
		ICoreClientAPI capi;
		ICoreServerAPI sapi;

		// Data structures
		List<Tribute> tributes = new List<Tribute>();
		Globals globals = new Globals();

		// Server and client
		Client client = new Client();
		Server server = new Server();

		// Once the mod Starts
		public override void Start(ICoreAPI api)
		{
			base.Start(api);
			this.api = api;
		}

		// Once the mod starts client-side
		public override void StartClientSide(ICoreClientAPI capi)
		{
			base.StartClientSide(capi);
			this.capi = capi;

			client.SetupGUI(capi);
		}

		// Once the mod starts server-side
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

		// Defined event functions
		private void OnSaveGameLoaded()
		{
			sapi.LoadModConfig<Globals>("hungergames_globals.json");
			sapi.LoadModConfig<List<Tribute>>("hungergames_config.json");
		}

		private void OnGameWorldSave()
		{
			sapi.StoreModConfig<Globals>(globals, "hungergames_globals.json");
			sapi.StoreModConfig<List<Tribute>>(tributes, "hungergames_config.json");
		}

		private void OnPlayerJoin(IServerPlayer byPlayer)
		{
			server.OnPlayerJoin(sapi, globals, tributes, byPlayer);
		}

		private void OnPlayerDisconnect(IServerPlayer byPlayer)
		{
			server.OnPlayerDisconnect(sapi, globals, tributes, byPlayer);
		}

		private void OnPlayerDeath(IServerPlayer byPlayer, DamageSource damageSource)
		{
			server.OnPlayerDeath(sapi, globals, tributes, byPlayer);
		}
	}
}