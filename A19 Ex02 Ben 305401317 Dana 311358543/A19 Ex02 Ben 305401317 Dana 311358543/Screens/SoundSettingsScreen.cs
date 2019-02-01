using System;
using Microsoft.Xna.Framework;
using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
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

            toggleGameSound.ToggleValueChanched += new EventHandler<EventArgs>(m_SoundSettingManager.ToggleGameSound);
            bgMusicVolume.VolumeIncrease += new EventHandler<EventArgs>(m_SoundSettingManager.IncreaseBackgroundMusicVolume);
            bgMusicVolume.VolumeDecrease += new EventHandler<EventArgs>(m_SoundSettingManager.DecreaseBackgroundMusicVolume);
            soundEffectsVolume.VolumeIncrease += new EventHandler<EventArgs>(m_SoundSettingManager.IncreaseSoundEffectsVolume);
            soundEffectsVolume.VolumeDecrease += new EventHandler<EventArgs>(m_SoundSettingManager.DecreaseSoundEffectsVolume);

            doneItem.ItemClicked += OnItemClicked;

            AddMenuItem(toggleGameSound);
            AddMenuItem(bgMusicVolume);
            AddMenuItem(soundEffectsVolume);
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
