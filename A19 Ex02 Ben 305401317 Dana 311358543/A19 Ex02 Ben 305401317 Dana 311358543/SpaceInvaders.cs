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
    public class SpaceInvaders : Game
    {
        GraphicsDeviceManager m_GraphicsMgr;
        public SpaceInvaders()
        {
            m_GraphicsMgr = new GraphicsDeviceManager(this);

            new ScreenSettingsManager(this);
            new CollisionsManager(this);
            new SpaceInvadersEngine(this);
            new InputManager(this);
            this.Services.AddService(typeof(Random), new Random());

            ScreensMananger screensMananger = new ScreensMananger(this);
            GameScreen welcomeScreen = new WelcomeScreen(this);
            GameScreen gameOverScreen = new GameOverScreen(this);

            screensMananger.AddToDictScreens(welcomeScreen);
            screensMananger.AddToDictScreens(new GameOverScreen(this));
            screensMananger.AddToDictScreens(new PlayScreen(this));
            screensMananger.AddToDictScreens(new MainMenuScreen(this));
            screensMananger.AddToDictScreens(new PauseScreen(this));
            screensMananger.AddToDictScreens(new ScreenSettingsScreen(this));
            screensMananger.AddToDictScreens(new SoundSettingsScreen(this));
            screensMananger.AddToDictScreens(new LevelTransitionScreencs(this));

            screensMananger.SetCurrentScreen(gameOverScreen);
            screensMananger.SetCurrentScreen(welcomeScreen);

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.m_GraphicsMgr.IsFullScreen = false;
            
            this.m_GraphicsMgr.PreferredBackBufferWidth = 800;
            this.m_GraphicsMgr.PreferredBackBufferHeight = 600;
            this.m_GraphicsMgr.ApplyChanges();
            this.Window.Title = "Space Invaders";
        }

        protected override void Draw(GameTime i_GameTime)
        {
            m_GraphicsMgr.GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(i_GameTime);
        }
    }
}
