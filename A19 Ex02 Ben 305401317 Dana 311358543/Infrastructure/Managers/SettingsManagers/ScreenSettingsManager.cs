using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure
{
    public class ScreenSettingsManager : GameService
    {
        Game m_Game;
        private bool isMouseVisible = true;
        private bool isWindowResizingAllowed = true;
        private bool isFullScreenMode = true;


        public ScreenSettingsManager(GameScreen i_GameScreen) : base(i_GameScreen.Game)
        {
            m_Game = i_GameScreen.Game;
        }

        public void ToggleMouseVisabilityConfig()
        {
            this.m_Game.IsMouseVisible = !this.m_Game.IsMouseVisible;
        }

        public void ToggleAllowWindowResizingConfig()
        {
            this.m_Game.Window.AllowUserResizing = !this.m_Game.Window.AllowUserResizing;
        }

        public void ToggleFullScreenModeConfig()
        {
        }

    }
}
