using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Infrastructure;

namespace A19_Ex03_Ben_305401317_Dana_311358543
{
    public class SpaceInvaders : Game
    {
        private GraphicsDeviceManager m_GraphicsMgr;
        private InputManager m_InputManager;
        private SoundManager m_SoundManager;
        private SoundSettingsManager m_SoundSettingsManager; 

        public SpaceInvaders()
        {
            this.m_GraphicsMgr = new GraphicsDeviceManager(this);
            this.m_GraphicsMgr.IsFullScreen = false;
            this.m_GraphicsMgr.PreferredBackBufferWidth = (int)SpaceInvadersConfig.k_DefaultWindowSize.X;
            this.m_GraphicsMgr.PreferredBackBufferHeight = (int)SpaceInvadersConfig.k_DefaultWindowSize.Y;
            this.m_GraphicsMgr.ApplyChanges();
            this.Window.Title = "Space Invaders";

            this.Services.AddService(typeof(Random), new Random());

            this.m_SoundManager = new SoundManager(this);
            new ScreenSettingsManager(this, SpaceInvadersConfig.k_DefaultWindowSize);
            this.m_SoundSettingsManager = new SoundSettingsManager(this); 
            new CollisionsManager(this);
            this.m_InputManager = new InputManager(this);
            
            ScreensMananger screensMananger = new ScreensMananger(this);
            new SpaceInvadersEngine(this);
            GameScreen welcomeScreen = new WelcomeScreen(this);
            GameScreen gameOverScreen = new GameOverScreen(this);

            screensMananger.AddScreen(welcomeScreen);
            screensMananger.AddScreen(gameOverScreen);
            screensMananger.AddScreen(new PlayScreen(this));
            screensMananger.AddScreen(new MainMenuScreen(this));
            screensMananger.AddScreen(new PauseScreen(this));
            screensMananger.AddScreen(new ScreenSettingsScreen(this));
            screensMananger.AddScreen(new SoundSettingsScreen(this));
            screensMananger.AddScreen(new LevelTransitionScreen(this));

            screensMananger.SetCurrentScreen(welcomeScreen);

            Content.RootDirectory = "Content";
        }

        protected override void Update(GameTime gameTime)
        {
            if (this.m_InputManager.KeyPressed(Keys.M))
            {
                this.m_SoundSettingsManager.ToggleGameSound_Clicked(this, null);
            }

            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            
            this.m_SoundManager.AddSoundEffect(this.Content.Load<SoundEffect>(@"c:/temp/XNA_Assets/Ex03/Sounds/SSGunShot"), "SSGunShot");
            this.m_SoundManager.AddSoundEffect(this.Content.Load<SoundEffect>(@"c:/temp/XNA_Assets/Ex03/Sounds/MotherShipKill"), "MotherShipKill");
            this.m_SoundManager.AddSoundEffect(this.Content.Load<SoundEffect>(@"c:/temp/XNA_Assets/Ex03/Sounds/MenuMove"), "MenuMove");
            this.m_SoundManager.AddSoundEffect(this.Content.Load<SoundEffect>(@"c:/temp/XNA_Assets/Ex03/Sounds/LifeDie"), "LifeDie");
            this.m_SoundManager.AddSoundEffect(this.Content.Load<SoundEffect>(@"c:/temp/XNA_Assets/Ex03/Sounds/LevelWin"), "LevelWin");
            this.m_SoundManager.AddSoundEffect(this.Content.Load<SoundEffect>(@"c:/temp/XNA_Assets/Ex03/Sounds/GameOver"), "GameOver");
            this.m_SoundManager.AddSoundEffect(this.Content.Load<SoundEffect>(@"c:/temp/XNA_Assets/Ex03/Sounds/EnemyKill"), "EnemyKill");
            this.m_SoundManager.AddSoundEffect(this.Content.Load<SoundEffect>(@"c:/temp/XNA_Assets/Ex03/Sounds/EnemyGunShot"), "EnemyGunShot");
            this.m_SoundManager.AddSoundEffect(this.Content.Load<SoundEffect>(@"c:/temp/XNA_Assets/Ex03/Sounds/BarrierHit"), "BarrierHit");
            this.m_SoundManager.AddSong(this.Content.Load<Song>(@"c:/temp/XNA_Assets/Ex03/Sounds/BGMusic"), "BGMusic");

            MediaPlayer.Play(this.m_SoundManager.GetSong("BGMusic"));
            MediaPlayer.IsRepeating = true;

            base.LoadContent();
        }

        protected override void Draw(GameTime i_GameTime)
        {
            this.m_GraphicsMgr.GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(i_GameTime);
        }
    }
}
