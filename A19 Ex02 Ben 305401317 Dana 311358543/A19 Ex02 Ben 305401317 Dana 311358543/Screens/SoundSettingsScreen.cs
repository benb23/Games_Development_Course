using Microsoft.Xna.Framework;
using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public class SoundSettingsScreen : GameScreen
    {

        private Background m_Background;
        private MenuHeader m_MenuHeader;
        private ISoundSettingsManager m_SoundSettingManager;

        public SoundSettingsScreen(Game i_Game) : base(i_Game, 150f, 15f)
        {
            int index = 0;
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);
            this.m_MenuHeader = new MenuHeader(this, @"Screens\Settings\SoundSettingsLogo");
            this.m_SoundSettingManager = i_Game.Services.GetService(typeof(ISoundSettingsManager)) as ISoundSettingsManager;

            ToggleItem toggleGameSound = new ToggleItem(@"Screens/Settings/ToggleSound", @"Screens\Settings\OnOff_53x52", this, index++);
            //ToggleItem windowResizing = new ToggleItem(@"Screens\Settings\AllowResizing", @"Screens\Settings\OnOff_53x52", this, index++, 1);
            //ToggleItem fullScreen = new ToggleItem(@"Screens\Settings\FullScreenMode", @"Screens\Settings\OnOff_53x52", this, index++, 1);
            ClickItem doneItem = new ClickItem("Done", @"Screens\Settings\Done", this, index++);

            toggleGameSound.ToggleValueChanched += new EventHandler<EventArgs>(m_SoundSettingManager.ToggleMouseVisabilityConfig);
            windowResizing.ToggleValueChanched += new EventHandler<EventArgs>(m_SoundSettingManager.ToggleAllowWindowResizingConfig);
            fullScreen.ToggleValueChanched += new EventHandler<EventArgs>(m_SoundSettingManager.ToggleFullScreenModeConfig);
            doneItem.ItemClicked += new EventHandler<ScreenEventArgs>(OnItemClicked);

            AddMenuItem(mouseVisability);
            AddMenuItem(windowResizing);
            AddMenuItem(fullScreen);
            AddMenuItem(doneItem);
        }



        private void OnItemClicked(object sender, ScreenEventArgs args)
        {
            this.ExitScreen();
        }


        public override string ToString()
        {
            return "SoundSettingsScreen";
        }
    }
}
