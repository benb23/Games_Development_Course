using Microsoft.Xna.Framework;
using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public class Background : Sprite
    {
        public Background(GameScreen i_GameScreen, string i_AssetName, float i_Opacity)
            : base(i_AssetName, i_GameScreen)
        {
            this.DrawOrder = int.MinValue;
            this.Opacity = i_Opacity;
            
        }

        public override void Initialize()
        {
            base.Initialize();
            this.m_Scales = new Vector2((float)this.GraphicsDevice.Adapter.CurrentDisplayMode.Width / 1024f, (float)this.GraphicsDevice.Adapter.CurrentDisplayMode.Height / 768f);
        }
    }
}
