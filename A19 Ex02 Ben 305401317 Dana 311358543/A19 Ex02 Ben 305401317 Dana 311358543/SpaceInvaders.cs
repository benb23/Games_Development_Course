﻿using System;
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

            new ScreenSettingsManager(this);
            new CollisionsManager(this);
            new SpaceInvadersEngine(this);
            new InputManager(this);

            ScreensMananger screensMananger = new ScreensMananger(this);
            screensMananger.Push(new GameOverScreen(this)); //todo: delete
            screensMananger.SetCurrentScreen(new WelcomeScreen(this));

            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            base.Initialize();
            this.Services.AddService(typeof(GraphicsDeviceManager), m_GraphicsMgr);
            this.Window.Title = "Space Invaders";
        }

        protected override void Draw(GameTime i_GameTime)
        {
            m_GraphicsMgr.GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(i_GameTime);
        }
    }
}
