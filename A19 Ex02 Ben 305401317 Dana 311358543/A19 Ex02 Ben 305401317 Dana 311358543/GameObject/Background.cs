using Microsoft.Xna.Framework;
using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public class Background : Sprite
    {
        private Game m_Game;
        private GraphicsDeviceManager m_GraphicDeviceMng;

        public Background(GameScreen i_GameScreen, string i_AssetName, float i_Opacity)
            : base(i_AssetName, i_GameScreen)
        {
            this.m_Game = i_GameScreen.Game;
            this.DrawOrder = int.MinValue;
            this.Opacity = i_Opacity;
            
        }

        public override void Initialize()
        {
            base.Initialize();
            this.m_Scales = new Vector2(this.m_Game.Window.ClientBounds.Width / SpaceInvadersConfig.k_DefaultWindowSize.X, this.m_Game.Window.ClientBounds.Height / SpaceInvadersConfig.k_DefaultWindowSize.Y);
            this.m_Game.Window.ClientSizeChanged += Window_ClientSizeChanged1;
        }

        private void Window_ClientSizeChanged1(object sender, System.EventArgs e)
        {
            if (m_GraphicDeviceMng == null)
            {
                m_GraphicDeviceMng = m_Game.Services.GetService(typeof(IGraphicsDeviceManager)) as GraphicsDeviceManager;
            }

            if (m_GraphicDeviceMng.IsFullScreen)
            {
                //todo : const?
                this.m_Scales = new Vector2(m_Game.GraphicsDevice.DisplayMode.Width / 1024f, m_Game.GraphicsDevice.DisplayMode.Height / 768f);
            }
            else
            {
                this.m_Scales = new Vector2(this.m_Game.Window.ClientBounds.Width / SpaceInvadersConfig.k_DefaultWindowSize.X, this.m_Game.Window.ClientBounds.Height / SpaceInvadersConfig.k_DefaultWindowSize.Y);
            }
            
        }


    }
}
