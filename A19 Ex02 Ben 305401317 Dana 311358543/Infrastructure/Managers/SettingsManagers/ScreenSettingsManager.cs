using System;
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

        private bool isMouseVisible = true;
        private bool isWindowResizingAllowed = true;
        private bool isFullScreenMode = true;


        public ScreenSettingsManager(Game i_Game) : base(i_Game)
        {
            this.m_Game = i_Game;
            this.m_GraphicDeviceManager = m_Game.Services.GetService(typeof(GraphicsDeviceManager)) as GraphicsDeviceManager;
        }


        public void ToggleMouseVisabilityConfig(object sender, EventArgs args)
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
        }

    }
}
