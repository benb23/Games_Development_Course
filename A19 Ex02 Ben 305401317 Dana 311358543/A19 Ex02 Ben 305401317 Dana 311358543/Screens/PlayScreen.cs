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
    public class PlayScreen : GameScreen
    {
        private const int k_NumOfPlayers = 2;
        private const int k_NumOfWalls = 4;
        private ISpaceInvadersEngine m_GameEngine;
        public const int k_MaxRandomNumber = 50000;
        private const string k_GameName = "Space Invaders";
        //public Random m_Random = new Random();
        private MotherSpaceShip m_MotherSpaceShip;
        private EnemiesGroup m_EnemysGroup;
        private Background m_Background;
        private List<Player> m_Players;
        private WallsGroup m_WallsGroup;
        private ScoreBoardHeader m_ScoreBoard;
        PauseScreen m_PauseScreenScreen;

        public PlayScreen(Game i_Game)
            : base(i_Game)
        {
            m_ScoreBoard = new ScoreBoardHeader(this);
            this.m_GameEngine = Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
            i_Game.IsMouseVisible = true;
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);
            this.m_MotherSpaceShip = new MotherSpaceShip(this);
            m_Players = m_GameEngine.Players;
            
            this.m_EnemysGroup = new EnemiesGroup(this);
            this.m_WallsGroup = new WallsGroup(this, k_NumOfWalls);
            m_PauseScreenScreen = new PauseScreen(this.Game);
        }

        private void OnGameOver()//TODO: CALL 
        {

            this.ExitScreen();
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            // we want to fade in only uppon first activation:
            this.ActivationLength = TimeSpan.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.m_State == eScreenState.Active)
            {
                if(m_GameEngine.IsGameOver)
                {
                    this.ExitScreen();
                    this.ScreensManager.SetCurrentScreen(new GameOverScreen(this.Game)); 
                }

                if (InputManager.KeyPressed(Keys.P))
                {
                    ScreensManager.SetCurrentScreen(m_PauseScreenScreen);
                }
            }
            
        }

        public override void Initialize()
        {
            this.m_GameEngine.CreatePlayers(this);
            m_Players = m_GameEngine.Players;
            this.Game.Window.Title = k_GameName;
            base.Initialize();
            m_WallsGroup.Position = new Vector2(m_WallsGroup.Position.X, GraphicsDevice.Viewport.Height - (2 * m_Players[(int)PlayerIndex.One].SpaceShip.Texture.Height));
        }
    }
}

