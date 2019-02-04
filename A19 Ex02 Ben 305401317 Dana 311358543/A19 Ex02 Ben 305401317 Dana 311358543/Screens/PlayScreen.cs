using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure;

namespace A19_Ex03_Ben_305401317_Dana_311358543
{
    public class PlayScreen : GameScreen
    {
        private const string k_GameName = "Space Invaders";
        private ISpaceInvadersEngine m_GameEngine;
        private ISoundMananger m_SoundManager;
        private MotherSpaceShip m_MotherSpaceShip;
        private EnemiesGroup m_EnemysGroup;
        private Background m_Background;
        private List<Player> m_Players;
        private WallsGroup m_WallsGroup;
        private ScoreBoardHeader m_ScoreBoard;
        private PauseScreen m_PauseScreenScreen;

        public PlayScreen(Game i_Game)
            : base(i_Game)
        {
            this.m_ScoreBoard = new ScoreBoardHeader(this);
            this.m_GameEngine = Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
            this.m_SoundManager = Game.Services.GetService(typeof(ISoundMananger)) as ISoundMananger;

            i_Game.IsMouseVisible = true;
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);
            this.m_MotherSpaceShip = new MotherSpaceShip(this);
            this.m_EnemysGroup = new EnemiesGroup(this);
            this.m_WallsGroup = new WallsGroup(this, SpaceInvadersConfig.k_NumOfWalls);
            this.m_PauseScreenScreen = new PauseScreen(this.Game);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            //// we want to fade in only uppon first activation:
            this.ActivationLength = TimeSpan.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!m_initialized)
            {
                this.m_GameEngine.InitGameEngineForNewGame();
                this.initSpritesForNewGame();
                m_initialized = true;
            }

            if (this.m_GameEngine.IsGameOver)
            {
                this.OnGameOver();
            }


            if (this.InputManager.KeyPressed(Keys.P))
            {
                this.ScreensManager.SetCurrentScreen(this.m_PauseScreenScreen);
            }
        }

        public override void Initialize()
        {
            this.m_GameEngine.CreatePlayers(this);
            this.m_Players = this.m_GameEngine.Players;
            this.Game.Window.Title = k_GameName;
            base.Initialize();
            this.m_WallsGroup.Position = new Vector2(this.m_WallsGroup.Position.X, this.GraphicsDevice.Viewport.Height - (2 * this.m_Players[(int)PlayerIndex.One].SpaceShip.Texture.Height));
            this.m_EnemysGroup.AllEnemiesDied += new EventHandler<EventArgs>(this.levelChanged);
        }

        private void levelChanged(object sender, EventArgs args)
        {
            this.m_SoundManager.PlaySoundEffect("LevelWin");
            MenuUtils.GoToScreen(this, this.ScreensManager.GetScreen("LevelTransitionScreen"));
            this.m_GameEngine.InitGameEngineForNextLevel();
            this.initSpritesForNewLevel();
        }

        private void OnGameOver()
        {
            this.m_SoundManager.PlaySoundEffect("GameOver");
            this.ScreensManager.SetCurrentScreen(this.ScreensManager.GetScreen("GameOverScreen"));
            //this.m_GameEngine.InitGameEngineForNewGame();
            this.m_initialized = false;
            //this.initSpritesForNewGame();
        }

        private void initSpritesForNewGame()
        {
            this.initSpritesForNewLevel();
        }

        private void initSpritesForNewLevel()
        {
            this.m_EnemysGroup.InitEnemyGroupForNextLevel();
            this.m_WallsGroup.InitWallsForNextLevel();
            this.m_MotherSpaceShip.InitMotherShipForNextLevel();
        }

        public override string ToString()
        {
            return "PlayScreen";
        }
    }
}