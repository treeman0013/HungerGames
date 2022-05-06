using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

[assembly: ModInfo( "HungerGames", Description = "Hello World", Version = "0.1.0", Website = "website", Authors = new []{ "treeman0013" } )]

namespace HungerGames
{
	public class HungerGames : ModSystem
	{
		ICoreClientAPI capi;
		HudElement hud;

		public override bool ShouldLoad(EnumAppSide forSide)
		{
			return forSide == EnumAppSide.Client;
		}
			
		public override void StartClientSide(ICoreClientAPI api)
		{
			base.StartClientSide(api);

			this.capi = api;
			capi.Input.RegisterHotKey("annoyingtextgui", "Annoys you with annoyingly centered text", GlKeys.U, HotkeyType.GUIOrOtherControls);
			capi.Input.SetHotKeyHandler("annoyingtextgui", ToggleGui);

			hud = new GuiPlayerCount(capi);
		}

		private bool ToggleGui(KeyCombination comb)
		{
			if (hud.IsOpened()) hud.TryClose();
			else hud.TryOpen();

			return true;
		}
	}
}