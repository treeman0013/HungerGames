using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace HungerGames
{
	public class GuiPlayerCount : HudElement
    {
        public override string ToggleKeyCombinationCode => "annoyingtextgui";

        public GuiPlayerCount(ICoreClientAPI capi) : base(capi)
        {
            SetupDialog();
        }

        private void SetupDialog()
        {
            // Auto-sized dialog at the center of the screen
            ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(EnumDialogArea.CenterMiddle);

            // Just a simple 300x300 pixel box
            ElementBounds textBounds = ElementBounds.Fixed(0, 40, 300, 100);

            // Background boundaries. Again, just make it fit it's child elements, then add the text as a child element
            ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
            bgBounds.BothSizing = ElementSizing.FitToChildren;
            bgBounds.WithChildren(textBounds);

            // Lastly, create the dialog
            SingleComposer = capi.Gui.CreateCompo("myAwesomeDialog", dialogBounds)
                .AddShadedDialogBG(bgBounds)
                .AddDialogTitleBar("Heck yeah!", OnTitleBarCloseClicked)
                .AddStaticText("This is a piece of text at the center of your screen - Enjoy!", CairoFont.WhiteDetailText(), textBounds)
                .Compose()
            ;
        }

        private void OnTitleBarCloseClicked()
        {
            TryClose();
        }
    }
}