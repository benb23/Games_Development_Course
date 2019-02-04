using Microsoft.Xna.Framework;
using Infrastructure;

namespace A19_Ex03_Ben_305401317_Dana_311358543
{
    public class Background : Sprite
    {
        private const float k_DestHeight = 768f;
        private const float k_DestWidth = 1024f;
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
            this.m_Game.Window.ClientSizeChanged += this.Window_ClientSizeChanged1;
        }

        private void Window_ClientSizeChanged1(object sender, System.EventArgs e)
        {
            if (this.m_GraphicDeviceMng == null)
            {
                this.m_GraphicDeviceMng = this.m_Game.Services.GetService(typeof(IGraphicsDeviceManager)) as GraphicsDeviceManager;
            }

            if (this.m_GraphicDeviceMng.IsFullScreen)
            {
                this.m_Scales = new Vector2(this.m_Game.GraphicsDevice.DisplayMode.Width / k_DestWidth, this.m_Game.GraphicsDevice.DisplayMode.Height / k_DestHeight);
            }
            else
            {
                this.m_Scales = new Vector2(this.m_Game.Window.ClientBounds.Width / SpaceInvadersConfig.k_DefaultWindowSize.X, this.m_Game.Window.ClientBounds.Height / SpaceInvadersConfig.k_DefaultWindowSize.Y);
            }
        }
    }
}
