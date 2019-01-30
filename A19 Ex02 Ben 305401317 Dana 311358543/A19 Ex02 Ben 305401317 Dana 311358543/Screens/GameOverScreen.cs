using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    class GameOverScreen : MenuScreen
    {
        private ISpaceInvadersEngine m_GameEngine;
        private Background m_Background;
        private MenuHeader m_GameOverHeader;
        SpriteFont m_FontCalibri;
        private string m_Result;

        public GameOverScreen(Game i_Game) : base(i_Game, 0f,50f, 15f)
        {
            this.IsUsingKeyboard = false;
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);
            this.m_Background.TintColor = Color.Red;
            this.m_GameOverHeader = new MenuHeader(this, @"Screens\GameOver\GameOverLogo");

            int index = 0;

            ClickItem QuitItem = new ClickItem("Quit", @"Screens\Wellcome\QuitGame", this, index++);
            ClickItem playItem = new ClickItem("PlayScreen", @"Screens\GameOver\Restart", this, index++);
            ClickItem mainMenuItem = new ClickItem("MainMenuScreen", @"Screens\Wellcome\MainMenu", this, index++);

            QuitItem.ItemClicked += new EventHandler<ScreenEventArgs>(OnQuitItemClicked);
            playItem.ItemClicked += new EventHandler<ScreenEventArgs>(OnItemClicked);
            mainMenuItem.ItemClicked += new EventHandler<ScreenEventArgs>(OnItemClicked);

            AddMenuItem(QuitItem);
            AddMenuItem(playItem);
            AddMenuItem(mainMenuItem);
        }

        public override void Initialize()
        {
            m_GameEngine = Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;

            if (SpaceInvadersConfig.m_NumOfPlayers == SpaceInvadersConfig.eNumOfPlayers.TwoPlayers)
            {
                string winner = setWinnerString();

m_Result = string.Format(
@"player 1 score is : {0}
Player 2 score is : {1}
The winner is : {2} !",
m_GameEngine.Players[0].Score.ToString(),
m_GameEngine.Players[1].Score.ToString(),
winner);
            }
            else
            {
m_Result = string.Format(
@"player score is : {0}",
m_GameEngine.Players[0].Score.ToString());
            }

            base.Initialize();
        }
        private string setWinnerString()
        {
            string winner;

            if (m_GameEngine.Winner == null)
            {
                winner = "Tie";
            }
            else
            {
                winner = "Player " + m_GameEngine.Winner.ToString();
            }

            return winner;
        }
        private void OnQuitItemClicked(object sender, ScreenEventArgs args)
        {
            Game.Exit();
        }

        private void OnItemClicked(object sender, ScreenEventArgs args)
        {
            MenuUtils.GoToScreenAndExitCurrent(this, this.m_ScreensManager.GetScreen(args.ScreenName));
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.KeyPressed(Keys.Escape))
            {
                OnQuitItemClicked(this, null);
            }
            else if(InputManager.KeyPressed(Keys.Home))
            {
                OnItemClicked(this, new ScreenEventArgs("PlayScreen"));
            }
            else if (InputManager.KeyPressed(Keys.T))
            {
                OnItemClicked(this, new ScreenEventArgs("MainMenuScreen"));
            }
            
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_FontCalibri = ContentManager.Load<SpriteFont>(@"Fonts\Calibri");
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SpriteBatch.Begin();
            SpriteBatch.DrawString(m_FontCalibri, m_Result, new Vector2(200), Color.White);
            SpriteBatch.End();
        }

        public override string ToString()
        {
            return "GameOverScreen";
        }


    }
}
