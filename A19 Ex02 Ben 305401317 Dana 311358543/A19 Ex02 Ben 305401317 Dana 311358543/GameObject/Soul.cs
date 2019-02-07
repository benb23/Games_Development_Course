using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure;

namespace A19_Ex03_Ben_305401317_Dana_311358543
{
    public class Soul : Sprite
    {
        private const int k_MaxSoulsNumber = 3;
        private int m_CurrSoulsNumber = k_MaxSoulsNumber;
        private int m_SoulIndx;
        private PlayerIndex m_SoulOwner;

        public Soul(GameScreen i_GameScreen, Vector2 i_Scale, float i_Opacity, string i_AssteName, PlayerIndex i_SoulOwner, int i_SoulIndex)
            : base(i_AssteName, i_GameScreen)
        {
            this.Scales = i_Scale;
            this.Opacity = i_Opacity;
            this.m_SoulIndx = i_SoulIndex;
            this.m_SoulOwner = i_SoulOwner;
        }

        public int SoulIndex
        {
            set { this.m_SoulIndx = value; }
        }

        public override void Update(GameTime gameTime)
        {
            if(!this.m_Initialize)
            {
                this.initPosition();
                this.m_Initialize = true;
            }

            base.Update(gameTime);
        }

        private void initPosition()
        {
            this.m_CurrSoulsNumber -= this.m_SoulIndx;
            float soulsGap = 5;
            this.Position = new Vector2(this.Game.GraphicsDevice.Viewport.Width - (this.m_CurrSoulsNumber * (this.Width + soulsGap)), 1 + ((int)this.m_SoulOwner * 20));
        }

        public override void Draw(GameTime gameTime)
        {
            this.m_SpriteBatch.End();
            this.m_SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            this.DrawWithAllParameters();
            this.m_SpriteBatch.End();
            this.m_SpriteBatch.Begin();
        }
    }
}
