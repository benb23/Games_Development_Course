using Microsoft.Xna.Framework;
using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public class Background : Sprite
    {
        public Background(string i_AssetName, Game i_Game, int i_UpdateOrder, int i_DrawOrder)
            : base(i_AssetName, i_Game, i_UpdateOrder, i_DrawOrder)
        {
        }

        protected override void InitBounds()
        {
            base.InitBounds();

            this.DrawOrder = int.MinValue;
        }
    }
}
