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

            this.m_GraphicsMgr.IsFullScreen = false;

            this.m_GraphicsMgr.PreferredBackBufferWidth = 800;
            this.m_GraphicsMgr.PreferredBackBufferHeight = 600;
            this.m_GraphicsMgr.ApplyChanges();
            this.Window.Title = "Space Invaders";

            this.Services.AddService(typeof(Random), new Random());

            new ScreenSettingsManager(this);
            new CollisionsManager(this);
            new SpaceInvadersEngine(this);
            new InputManager(this);
            ScreensMananger screensMananger = new ScreensMananger(this);
            GameScreen welcomeScreen = new WelcomeScreen(this);
            GameScreen gameOverScreen = new GameOverScreen(this);

            screensMananger.AddScreen(welcomeScreen);
            screensMananger.AddScreen(new GameOverScreen(this));
            screensMananger.AddScreen(new PlayScreen(this));
            screensMananger.AddScreen(new MainMenuScreen(this));
            screensMananger.AddScreen(new PauseScreen(this));
            screensMananger.AddScreen(new ScreenSettingsScreen(this));
            screensMananger.AddScreen(new SoundSettingsScreen(this));
            screensMananger.AddScreen(new LevelTransitionScreen(this));

            screensMananger.SetCurrentScreen(gameOverScreen);
            screensMananger.SetCurrentScreen(welcomeScreen);

            Content.RootDirectory = "Content";
        }

        //protected override void Initialize()
        //{
        //    base.Initialize();
            




        //    //this.m_GraphicsMgr.PreferredBackBufferWidth = this.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
        //    //this.m_GraphicsMgr.PreferredBackBufferHeight = this.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
           
        //}

        protected override void Draw(GameTime i_GameTime)
        {
            m_GraphicsMgr.GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(i_GameTime);
        }
    }
}
