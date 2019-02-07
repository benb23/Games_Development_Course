using System;
using Microsoft.Xna.Framework;
using Infrastructure;

namespace A19_Ex03_Ben_305401317_Dana_311358543
{
    public class ScreenSettingsScreen : MenuScreen
    {
        private Background m_Background;
        private MenuHeader m_MenuHeader;
        private IScreenSettingsManager m_ScreenSettingMng;

        public ScreenSettingsScreen(Game i_Game) : base(i_Game, 150f, 15f)
        {
            int index = 0;
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);
            this.m_MenuHeader = new MenuHeader(this, @"Screens\Settings\ScreenSettingsLogo");
            this.m_ScreenSettingMng = i_Game.Services.GetService(typeof(IScreenSettingsManager)) as IScreenSettingsManager;

            ToggleItem mouseVisability = new ToggleItem(@"Screens\Settings\MouseVisability", @"Screens\Settings\VisibleInvisible_128x50", this, index++);
            ToggleItem windowResizing = new ToggleItem(@"Screens\Settings\AllowResizing", @"Screens\Settings\OnOff_53x52", this, index++, 1);
            ToggleItem fullScreen = new ToggleItem(@"Screens\Settings\FullScreenMode", @"Screens\Settings\OnOff_53x52", this, index++, 1);
            ClickItem doneItem = new ClickItem("Done", @"Screens\Settings\Done", this, index++);

            mouseVisability.ToggleValueChanched += new EventHandler<EventArgs>(this.mouseVisability_Changed);
            windowResizing.ToggleValueChanched += new EventHandler<EventArgs>(this.m_ScreenSettingMng.AllowWindowResizingConfigToggle_Click);
            fullScreen.ToggleValueChanched += new EventHandler<EventArgs>(this.m_ScreenSettingMng.FullScreenModeConfigToggle_Click);
            doneItem.ItemClicked += new EventHandler<ScreenEventArgs>(this.menuItem_Click);

            this.AddMenuItem(mouseVisability);
            this.AddMenuItem(windowResizing);
            this.AddMenuItem(fullScreen);
            this.AddMenuItem(doneItem);
        }

        private void mouseVisability_Changed(object sender, EventArgs args)
        {
            this.m_ScreenSettingMng.ToggleMouseVisabilityConfig();
            this.IsUsingMouse = !this.IsUsingMouse;
        }

        private void menuItem_Click(object sender, ScreenEventArgs args)
        {
            this.ExitScreen();
        }

        public override string ToString()
        {
            return "ScreenSettingsScreen";
        }
    }
}
