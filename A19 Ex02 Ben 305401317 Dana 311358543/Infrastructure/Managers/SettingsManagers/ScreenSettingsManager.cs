﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure
{
    public class ScreenSettingsManager : GameService, IScreenSettingsManager
    {
        Game m_Game;
        GraphicsDeviceManager m_GraphicDeviceManager;

        public ScreenSettingsManager(Game i_Game) : base(i_Game)
        {
            this.m_Game = i_Game;
            this.m_GraphicDeviceManager = m_Game.Services.GetService(typeof(IGraphicsDeviceManager)) as GraphicsDeviceManager;
        }

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(IScreenSettingsManager), this);
        }

        public void ToggleMouseVisabilityConfig()
        {
            this.m_Game.IsMouseVisible = !this.m_Game.IsMouseVisible;
        }

        public void ToggleAllowWindowResizingConfig(object sender, EventArgs args)
        {
            this.m_Game.Window.AllowUserResizing = !this.m_Game.Window.AllowUserResizing;
        }

        public void ToggleFullScreenModeConfig(object sender, EventArgs args)
        {
            this.m_GraphicDeviceManager.IsFullScreen = !this.m_GraphicDeviceManager.IsFullScreen;

            //if (!this.m_GraphicDeviceManager.IsFullScreen)
            //{
            //    this.m_GraphicDeviceManager.PreferredBackBufferWidth = 800;
            //    this.m_GraphicDeviceManager.PreferredBackBufferHeight = 600;
            //}

            if (this.m_GraphicDeviceManager.IsFullScreen)
            {
                this.m_GraphicDeviceManager.PreferredBackBufferWidth = m_Game.GraphicsDevice.DisplayMode.Width;
                this.m_GraphicDeviceManager.PreferredBackBufferHeight = m_Game.GraphicsDevice.DisplayMode.Height;
            }
            else
            {
                this.m_GraphicDeviceManager.PreferredBackBufferWidth = 800;
                this.m_GraphicDeviceManager.PreferredBackBufferHeight = 600;
            }

            this.m_GraphicDeviceManager.ApplyChanges();
        }

    }
}
