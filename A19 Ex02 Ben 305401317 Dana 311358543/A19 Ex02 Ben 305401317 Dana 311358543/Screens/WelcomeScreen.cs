using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public class WelcomeScreen : MenuScreen
    {
        private Background m_Background;
        private MenuHeader m_MenuHeader;

        public WelcomeScreen(Game i_Game) : base(i_Game)
        {
            this.IsUsingKeyboard = false;
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);
            this.m_MenuHeader = new MenuHeader(this, @"Screens\Wellcome\SpaceInvadersLogo", 0.8f);


            int index = 0;
            ClickItem playItem = new ClickItem("PlayScreen", @"Screens\Wellcome\PlayGame", this, index++);
            ClickItem mainMenuItem = new ClickItem("MainMenuScreen", @"Screens\Wellcome\MainMenu", this, index++);
            ClickItem QuitItem = new ClickItem("Quit", @"Screens\Wellcome\QuitGame", this, index++);

            playItem.ItemClicked += new EventHandler<ScreenEventArgs>(handleItemClicked);
            mainMenuItem.ItemClicked += new EventHandler<ScreenEventArgs>(handleItemClicked);
            QuitItem.ItemClicked += new EventHandler<ScreenEventArgs>(quit);

            AddMenuItem(playItem);
            AddMenuItem(mainMenuItem);
            AddMenuItem(QuitItem);
        }

        private void quit(object sender, ScreenEventArgs args)
        {
            Game.Exit();
        }

        private void handleItemClicked(object sender, ScreenEventArgs args)
        {
            MenuUtils.GoToScreen(this, this.m_ScreensManager.GetScreen(args.ScreenName));
        }

        public override void Initialize()
        {
            base.Initialize();
            m_MenuHeader.OffsetX = m_MenuHeader.Texture.Width / 10;

        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.KeyPressed(Keys.Enter))
            {
                handleItemClicked(this, new ScreenEventArgs("PlayScreen"));
            }
            else if(InputManager.KeyPressed(Keys.T))
            {
                handleItemClicked(this, new ScreenEventArgs("MainMenuScreen"));
            }
            else if (InputManager.KeyPressed(Keys.Escape))
            {
                quit(this,null);
            }

            base.Update(gameTime);
        }

        public override string ToString()
        {
            return "WellcomeScreen";
        }
    }
}
