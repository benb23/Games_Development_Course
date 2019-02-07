using System;
using Microsoft.Xna.Framework;
using Infrastructure;

namespace A19_Ex03_Ben_305401317_Dana_311358543
{
    public class SoundSettingsScreen : MenuScreen
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

            ToggleItem toggleGameSound = new ToggleItem(@"Screens\Settings\ToggleSound", @"Screens\Settings\OnOff_53x52", this, index++);
            VolumeItem bgMusicVolume = new VolumeItem(@"Screens\Settings\BgMusic", this, index++);
            VolumeItem soundEffectsVolume = new VolumeItem(@"Screens\Settings\SoundEffects", this, index++);
            ClickItem doneItem = new ClickItem("Done", @"Screens\Settings\Done", this, index++);

            toggleGameSound.ToggleValueChanched += new EventHandler<EventArgs>(this.m_SoundSettingManager.ToggleGameSound_Clicked);
            bgMusicVolume.IncreaseVolumeButtonClicked += new EventHandler<EventArgs>(this.m_SoundSettingManager.IncreaseBackgroundMusic_Click);
            bgMusicVolume.DecreaseVolumeButtonClicked += new EventHandler<EventArgs>(this.m_SoundSettingManager.DecreaseBackgroundMusic_Click);
            soundEffectsVolume.IncreaseVolumeButtonClicked += new EventHandler<EventArgs>(this.m_SoundSettingManager.IncreaseSoundEffects_Click);
            soundEffectsVolume.DecreaseVolumeButtonClicked += new EventHandler<EventArgs>(this.m_SoundSettingManager.DecreaseSoundEffects_Click);

            doneItem.ItemClicked += this.menuItem_Click;

            this.AddMenuItem(toggleGameSound);
            this.AddMenuItem(bgMusicVolume);
            this.AddMenuItem(soundEffectsVolume);
            this.AddMenuItem(doneItem);
        }

        private void menuItem_Click(object sender, ScreenEventArgs args)
        {
            this.ExitScreen();
        }

        public override string ToString()
        {
            return "SoundSettingsScreen";
        }
    }
}
