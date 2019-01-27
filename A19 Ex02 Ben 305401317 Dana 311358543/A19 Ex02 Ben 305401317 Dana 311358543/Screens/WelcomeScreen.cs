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
    public class WelcomeScreen : MenuScreen
    {
        private MainMenuScreen m_MainMenuScreen;
        private PlayScreen m_PlayScreen;
        private Background m_Background;
        private MenuHeader m_MenuHeader;

        public WelcomeScreen(Game i_Game) : base(i_Game, new Vector2(250 ,250), 15f)
        {
            this.IsUsingKeyboard = false;
            this.m_PlayScreen = new PlayScreen(Game);
            this.m_MainMenuScreen = new MainMenuScreen(Game);
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);
            this.m_MenuHeader = new MenuHeader(this, @"Screens\Wellcome\SpaceInvadersLogo");
        }

        public override void Initialize()
        {
            int index = 0;
            AddMenuItem(new ClickItem(@"Screens\Wellcome\PlayGame", this, index++, m_PlayScreen));
            AddMenuItem(new ClickItem(@"Screens\Wellcome\MainMenu", this, index++, m_MainMenuScreen));
            AddMenuItem(new ClickItem(@"Screens\Wellcome\QuitGame", this, index++));

            m_MenuHeader.Scales *= 0.8f;
            m_MenuHeader.Position = new Vector2(GraphicsDevice.Viewport.Width / 10, 20);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.KeyPressed(Keys.Enter))
            {
                ExitScreen();
                this.ScreensManager.SetCurrentScreen(m_PlayScreen);
            }
            else if(InputManager.KeyPressed(Keys.T))
            {
                ExitScreen();
                this.ScreensManager.SetCurrentScreen(m_MainMenuScreen);
            }
            else if (InputManager.KeyPressed(Keys.Escape))
            {
                Game.Exit();
                Game.Exit();
            }

            base.Update(gameTime);
        }
    }
}
