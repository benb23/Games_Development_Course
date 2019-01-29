using Microsoft.Xna.Framework;
using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public class Background : Sprite
    {
        private Game m_Game;

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
            this.m_Scales = new Vector2(this.m_Game.Window.ClientBounds.Width / 800f, this.m_Game.Window.ClientBounds.Height / 600f);
        }
    }
}
