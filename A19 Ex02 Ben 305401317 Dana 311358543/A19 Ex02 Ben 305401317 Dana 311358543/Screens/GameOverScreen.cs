using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Infrastructure;

namespace A19_Ex03_Ben_305401317_Dana_311358543
{
    public class GameOverScreen : MenuScreen
    {
        private ISpaceInvadersEngine m_GameEngine;
        private Background m_Background;
        private MenuHeader m_GameOverHeader;
        private SpriteFont m_Font;
        private string m_Result;


        public GameOverScreen(Game i_Game) : base(i_Game, 70f, 30f, 15f)
        {
            this.IsUsingKeyboard = false;
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);
            this.m_Background.TintColor = Color.Red;
            this.m_GameOverHeader = new MenuHeader(this, @"Screens\GameOver\GameOverLogo");

            int index = 0;

            ClickItem QuitItem = new ClickItem("Quit", @"Screens\Wellcome\QuitGame", this, index++);
            ClickItem playItem = new ClickItem("PlayScreen", @"Screens\GameOver\Restart", this, index++);
            ClickItem mainMenuItem = new ClickItem("MainMenuScreen", @"Screens\Wellcome\MainMenu", this, index++);

            QuitItem.ItemClicked += new EventHandler<ScreenEventArgs>(this.quit);
            playItem.ItemClicked += new EventHandler<ScreenEventArgs>(this.handleItemClicked);
            mainMenuItem.ItemClicked += new EventHandler<ScreenEventArgs>(this.handleItemClicked);

            this.AddMenuItem(QuitItem);
            this.AddMenuItem(playItem);
            this.AddMenuItem(mainMenuItem);
        }

        public override void Initialize()
        {
            this.m_GameEngine = this.Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;

            base.Initialize();
        }

        private string setWinnerString()
        {
            PlayerIndex? winnerIndex = this.m_GameEngine.getWinner();
            string winner;

            if (winnerIndex == null)
            {
                winner = "Tie";
            }
            else
            {
                winner = "Player " + winnerIndex.ToString();
            }

            return winner;
        }

        private void quit(object sender, ScreenEventArgs args)
        {
            Game.Exit();
        }

        private void handleItemClicked(object sender, ScreenEventArgs args)
        {
            MenuUtils.GoToScreen(this, this.m_ScreensManager.GetScreen(args.ScreenName));
        }

        public override void Update(GameTime gameTime)
        {
            setResultString();

            if (InputManager.KeyPressed(Keys.Escape))
            {
                this.quit(this, null);
            }
            else if(InputManager.KeyPressed(Keys.Home))
            {
                this.handleItemClicked(this, new ScreenEventArgs("PlayScreen"));
            }
            else if (InputManager.KeyPressed(Keys.T))
            {
                this.handleItemClicked(this, new ScreenEventArgs("MainMenuScreen"));
            }
            
            base.Update(gameTime);
        }

        private void setResultString()
        {

            if (SpaceInvadersConfig.s_NumOfPlayers == SpaceInvadersConfig.eNumOfPlayers.TwoPlayers)
            {
                string winner = this.setWinnerString();

                this.m_Result = string.Format(
@"player 1 score is : {0}
Player 2 score is : {1}
The winner is : {2} !",
                this.m_GameEngine.Players[0].Score.ToString(),
                this.m_GameEngine.Players[1].Score.ToString(),
                winner);
            }
            else
            {
                this.m_Result = string.Format(
@"player score is : {0}",
                this.m_GameEngine.Players[0].Score.ToString());
            }
        }
        protected override void LoadContent()
        {
            base.LoadContent();
            this.m_Font = ContentManager.Load<SpriteFont>(@"Fonts\ERASDEMI");
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            this.SpriteBatch.Begin();
            this.SpriteBatch.DrawString(this.m_Font, this.m_Result, new Vector2(200), Color.Silver);
            this.SpriteBatch.End();
        }

        public override string ToString()
        {
            return "GameOverScreen";
        }
    }
}
