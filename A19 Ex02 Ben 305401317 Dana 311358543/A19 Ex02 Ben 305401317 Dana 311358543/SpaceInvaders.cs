using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public class SpaceInvaders : Game
    {
        GraphicsDeviceManager m_GraphicsMgr;
        InputManager m_InputManager;
        SoundManager m_SoundManager;
        SoundSettingsManager m_SoundSettingsManager; 

        public SpaceInvaders()
        {
            m_GraphicsMgr = new GraphicsDeviceManager(this);

            this.m_GraphicsMgr.IsFullScreen = false;

            this.m_GraphicsMgr.PreferredBackBufferWidth = 800;
            this.m_GraphicsMgr.PreferredBackBufferHeight = 600;
            this.m_GraphicsMgr.ApplyChanges();
            this.Window.Title = "Space Invaders";

            this.Services.AddService(typeof(Random), new Random());

            m_SoundManager = new SoundManager(this);
            new ScreenSettingsManager(this, SpaceInvadersConfig.k_DefaultWindowSize);
            m_SoundSettingsManager = new SoundSettingsManager(this); 
            new CollisionsManager(this);
            m_InputManager = new InputManager(this);
            
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
            if (m_InputManager.KeyPressed(Keys.M))
            {
                this.m_SoundSettingsManager.ToggleGameSound(this, null);
            }

            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            m_SoundManager.AddSoundEffect(this.Content.Load<SoundEffect>(@"Sounds/SSGunShot"), "SSGunShot");
            m_SoundManager.AddSoundEffect(this.Content.Load<SoundEffect>(@"Sounds/MotherShipKill"), "MotherShipKill");
            m_SoundManager.AddSoundEffect(this.Content.Load<SoundEffect>(@"Sounds/MenuMove"), "MenuMove");
            m_SoundManager.AddSoundEffect(this.Content.Load<SoundEffect>(@"Sounds/LifeDie"), "LifeDie");
            m_SoundManager.AddSoundEffect(this.Content.Load<SoundEffect>(@"Sounds/LevelWin"), "LevelWin");
            m_SoundManager.AddSoundEffect(this.Content.Load<SoundEffect>(@"Sounds/GameOver"), "GameOver");
            m_SoundManager.AddSoundEffect(this.Content.Load<SoundEffect>(@"Sounds/EnemyKill"), "EnemyKill");
            m_SoundManager.AddSoundEffect(this.Content.Load<SoundEffect>(@"Sounds/EnemyGunShot"), "EnemyGunShot");
            m_SoundManager.AddSoundEffect(this.Content.Load<SoundEffect>(@"Sounds/BarrierHit"), "BarrierHit");

            m_SoundManager.AddSong(this.Content.Load<Song>(@"Sounds/BGMusic"), "BGMusic");

            MediaPlayer.Play(m_SoundManager.GetSong("BGMusic"));
            MediaPlayer.IsRepeating = true;

            base.LoadContent();
        }

        protected override void Draw(GameTime i_GameTime)
        {
            m_GraphicsMgr.GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(i_GameTime);
        }
    }
}
