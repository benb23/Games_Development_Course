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
    public class MainMenuScreen : MenuScreen
    {
        private SettingsScreen m_SettingsScreen;
        private SoundSettingsScreen m_SoundSettingsScreen;
        private PlayScreen m_PlayScreen;
        private Background m_Background;
        private MenuHeader m_MenuHeader;

        public MainMenuScreen(Game i_Game) : base(i_Game, new Vector2(250, 250), 15f)
        {
            m_SettingsScreen = new SettingsScreen(Game);
            m_SoundSettingsScreen = new SoundSettingsScreen(Game);
            m_PlayScreen = new PlayScreen(Game);
            IsUsingKeyboard = true;
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);
            this.m_MenuHeader = new MenuHeader(this, @"Screens\MainMenu\MainMenuLogo");
        }

        public override void Initialize()
        {
            int index = 0;
            AddMenuItem(new ToggleItem(@"Screens\MainMenu\PlayersWhite", @"Screens\MainMenu\PlayersOptions_70x50", this, index++, 2));
            AddMenuItem(new ClickItem(@"Screens\MainMenu\ScreenSettings", this, index++, this.m_SettingsScreen));
            AddMenuItem(new ClickItem(@"Screens\MainMenu\SoundSettings", this, index++, this.m_SoundSettingsScreen));
            AddMenuItem(new ClickItem(@"Screens\MainMenu\PlayGameWhite", this, index++, this.m_PlayScreen));//todo: new game??
            AddMenuItem(new ClickItem(@"Screens\MainMenu\QuitWhite", this, index++));
            m_MenuHeader.Scales *= 0.8f;
            m_MenuHeader.Position = new Vector2(GraphicsDevice.Viewport.Width / 10, 20);

            base.Initialize();
        }
    }
}
