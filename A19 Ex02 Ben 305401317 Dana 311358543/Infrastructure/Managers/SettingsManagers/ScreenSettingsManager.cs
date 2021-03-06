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
        private Game m_Game;
        private GraphicsDeviceManager m_GraphicDeviceManager;
        private Vector2 m_DefaultWindowSize;

        public ScreenSettingsManager(Game i_Game, Vector2 i_DefaultWindowSize) : base(i_Game)
        {
            this.m_DefaultWindowSize = i_DefaultWindowSize;
            this.m_Game = i_Game;
            this.m_GraphicDeviceManager = this.m_Game.Services.GetService(typeof(IGraphicsDeviceManager)) as GraphicsDeviceManager;
        }

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(IScreenSettingsManager), this);
        }

        public void ToggleMouseVisabilityConfig()
        {
            this.m_Game.IsMouseVisible = !this.m_Game.IsMouseVisible;
        }

        public void AllowWindowResizingConfigToggle_Click(object sender, EventArgs args)
        {
            this.m_Game.Window.AllowUserResizing = !this.m_Game.Window.AllowUserResizing;
        }

        public void FullScreenModeConfigToggle_Click(object sender, EventArgs args)
        {
            this.m_GraphicDeviceManager.IsFullScreen = !this.m_GraphicDeviceManager.IsFullScreen;

            if (this.m_GraphicDeviceManager.IsFullScreen)
            {
                this.m_GraphicDeviceManager.PreferredBackBufferWidth = this.m_Game.GraphicsDevice.DisplayMode.Width;
                this.m_GraphicDeviceManager.PreferredBackBufferHeight = this.m_Game.GraphicsDevice.DisplayMode.Height;
            }
            else
            {
                this.m_GraphicDeviceManager.PreferredBackBufferWidth = (int)this.m_DefaultWindowSize.X;
                this.m_GraphicDeviceManager.PreferredBackBufferHeight = (int)this.m_DefaultWindowSize.Y;
            }

            this.m_GraphicDeviceManager.ApplyChanges();
        }
    }
}
