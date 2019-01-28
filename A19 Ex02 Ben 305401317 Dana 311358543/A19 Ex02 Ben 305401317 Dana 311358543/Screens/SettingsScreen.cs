using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public class SettingsScreen : MenuScreen
    {
        private Background m_Background;
        private MenuHeader m_MenuHeader;
        private IScreenSettingsManager m_ScreenSettingMng;


        public SettingsScreen(Game i_Game) : base(i_Game, new Vector2(250, 250), 15f)
        {
            m_screens.Add("MainMenu", new MainMenuScreen(Game));
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);
            this.m_MenuHeader = new MenuHeader(this, @"Screens\Settings\ScreenSettingsLogo");
            this.m_ScreenSettingMng = i_Game.Services.GetService(typeof(IScreenSettingsManager)) as IScreenSettingsManager;
        }



        public override void Initialize()
        {
            int index = 0;
            ToggleItem mouseVisability = new ToggleItem(@"Screens\Settings\MouseVisability", @"Screens\Settings\VisibleInvisible_128x50", this, index++);
            ToggleItem windowResizing = new ToggleItem(@"Screens\Settings\AllowResizing", @"Screens\Settings\OnOff_53x52", this, index++);
            ToggleItem fullScreen = new ToggleItem(@"Screens\Settings\FullScreenMode", @"Screens\Settings\OnOff_53x52", this, index++);
            ClickItem doneItem = new ClickItem("Quit", @"Screens\Settings\Done", this, index++);


            mouseVisability.ToggleValueChanched += new EventHandler<EventArgs>(m_ScreenSettingMng.ToggleMouseVisabilityConfig);
            windowResizing.ToggleValueChanched += new EventHandler<EventArgs>(m_ScreenSettingMng.ToggleAllowWindowResizingConfig);
            fullScreen.ToggleValueChanched += new EventHandler<EventArgs>(m_ScreenSettingMng.ToggleFullScreenModeConfig);
            doneItem.ItemClicked += new EventHandler<ScreenEventArgs>(OnItemClicked);

            AddMenuItem(mouseVisability);
            AddMenuItem(windowResizing);
            AddMenuItem(fullScreen);
            AddMenuItem(doneItem);


            base.Initialize();
        }

        private void OnItemClicked(object sender, ScreenEventArgs args)
        {
            MenuUtils.GoToScreen(this, m_screens[args.ScreenName]);
        }

    }


}
