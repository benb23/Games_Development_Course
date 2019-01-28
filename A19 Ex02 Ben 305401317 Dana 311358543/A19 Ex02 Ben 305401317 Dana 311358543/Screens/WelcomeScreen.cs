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
        private Background m_Background;
        private MenuHeader m_MenuHeader;

        public WelcomeScreen(Game i_Game) : base(i_Game, new Vector2(250 ,250), 15f)
        {
            this.IsUsingKeyboard = false;
            m_screens.Add("PlayScreen", new PlayScreen(Game));
            m_screens.Add("MainMenuScreen", new MainMenuScreen(Game));
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);
            this.m_MenuHeader = new MenuHeader(this, @"Screens\Wellcome\SpaceInvadersLogo", new Vector2(0.8f));
        }

        public override void Initialize()
        {
            int index = 0;
            ClickItem playItem = new ClickItem("PlayScreen",@"Screens\Wellcome\PlayGame", this, index++);
            ClickItem mainMenuItem = new ClickItem("MainMenuScreen", @"Screens\Wellcome\MainMenu", this, index++);
            ClickItem QuitItem = new ClickItem("Quit", @"Screens\Wellcome\QuitGame", this, index++);

            playItem.ItemClicked += new EventHandler<ScreenEventArgs>(OnItemClicked);
            mainMenuItem.ItemClicked += new EventHandler<ScreenEventArgs>(OnItemClicked);
            QuitItem.ItemClicked += new EventHandler<ScreenEventArgs>(OnQuitItemClicked);

            AddMenuItem(playItem);
            AddMenuItem(mainMenuItem);
            AddMenuItem(QuitItem);

            base.Initialize();
        }

        private void OnQuitItemClicked(object sender, ScreenEventArgs args)
        {
            Game.Exit();
        }

        private void OnItemClicked(object sender, ScreenEventArgs args)
        {
            //MenuUtils.GoToScreen(this, m_screens[args.ScreenName]);
            MenuUtils.GoToScreen(this, this.m_ScreensManager.GetScreen(args.ScreenName));
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.KeyPressed(Keys.Enter))
            {
                OnItemClicked(this, new ScreenEventArgs("PlayScreen"));
            }
            else if(InputManager.KeyPressed(Keys.T))
            {
                OnItemClicked(this, new ScreenEventArgs("MainMenuScreen"));
            }
            else if (InputManager.KeyPressed(Keys.Escape))
            {
                OnItemClicked(this, new ScreenEventArgs("Quit"));
            }

            base.Update(gameTime);
        }

        public override string ToString()
        {
            return "WellcomeScreen";
        }
    }
}
