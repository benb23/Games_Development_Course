using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Infrastructure;


namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public class PlayScreen : GameScreen
    {
        private const int k_NumOfPlayers = 2;
        private const int k_NumOfWalls = 4;
        private ISpaceInvadersEngine m_GameEngine;
        private ISoundMananger m_SoundManager;

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
            this.m_SoundManager = Game.Services.GetService(typeof(ISoundMananger)) as ISoundMananger;

            i_Game.IsMouseVisible = true;
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);
            this.m_MotherSpaceShip = new MotherSpaceShip(this);
            this.m_EnemysGroup = new EnemiesGroup(this);
            this.m_WallsGroup = new WallsGroup(this, k_NumOfWalls);
            m_PauseScreenScreen = new PauseScreen(this.Game);
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

            if(m_GameEngine.IsGameOver)
            {
                OnGameOver();
            }

            if (this.m_State == eScreenState.Active)
            {
                if (m_GameEngine.IsGameOver)
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
            m_EnemysGroup.AllEnemiesDied += new EventHandler<EventArgs>(OnLevelEnded);

        }

        private void OnLevelEnded(object sender, EventArgs args)
        {
            this.m_SoundManager.PlaySoundEffect("LevelWin");
            MenuUtils.GoToScreen(this,this.ScreensManager.GetScreen("LevelTransitionScreen"));
            m_GameEngine.InitGameEngineForNextLevel();
            initSpritesForNewLevel();
        }

        private void OnGameOver()//TODO: CALL 
        {
            //ExitScreen();
            this.m_SoundManager.PlaySoundEffect("GameOver");
            this.ScreensManager.SetCurrentScreen(this.ScreensManager.GetScreen("GameOverScreen"));
            this.m_GameEngine.InitGameEngineForNewGame();
            this.initSpritesForNewGame();
            //this.ScreensManager.SetCurrentScreen(new GameOverScreen(this.Game));
        }

        private void initSpritesForNewGame()
        {
            initSpritesForNewLevel();
        }

        private void initSpritesForNewLevel()
        {
            m_EnemysGroup.InitEnemyGroupForNextLevel();
            m_WallsGroup.InitWallsForNextLevel();
            m_MotherSpaceShip.InitMotherShipForNextLevel();
            //this.ScreensManager.SetCurrentScreen(new LevelTransitionScreen(this.Game));

        }

        //private void initSoulsForNewGame()
        //{
        //    foreach (Player player in m_GameEngine.Players)
        //    {
        //        player.CreateSouls();
        //    }
        //}

        public override string ToString()
        {
            return "PlayScreen";
        }
    }
}

