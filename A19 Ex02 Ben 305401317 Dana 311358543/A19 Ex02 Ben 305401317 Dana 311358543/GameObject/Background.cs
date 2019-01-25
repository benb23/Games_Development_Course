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
    }
}
