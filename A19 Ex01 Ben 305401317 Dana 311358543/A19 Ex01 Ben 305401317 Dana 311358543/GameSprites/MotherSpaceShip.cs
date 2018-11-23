using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace A19_Ex01_Ben_305401317_Dana_311358543
{
    public class MotherSpaceShip : Sprite
    {
        private const float k_MotherShipVelocity = 40;
        private const int k_MaxRandomNumToDrawMotherShip = 70;
        private int m_CurrRandom = 100;

        public MotherSpaceShip(Game i_Game) : base(i_Game)
        {
            this.m_AssetName = @"Sprites\MotherShip_32x120";
            this.m_Tint = Color.Red;
            this.Visible = false;
        }

        public override void InitPosition()
        {
            this.Position = new Vector2(-m_Texture.Width, m_Texture.Height);
        }

        public override void Update(GameTime i_GameTime)
        {
            if (!this.Visible)
            { 
                this.m_CurrRandom = SpaceInvaders.s_RandomNum.Next(0, SpaceInvaders.k_MaxRandomNumber);
            }

            if (this.m_CurrRandom <= k_MaxRandomNumToDrawMotherShip)
            {
                this.Visible = true;
                this.m_Position.X += k_MotherShipVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
                if (m_Position.X >= GraphicsDevice.Viewport.Width)
                {
                    this.RemoveComponent();   
                }
            }
        }

        public override void RemoveComponent()
        {
            this.Visible = false;

            // init position for the next time the Mothership will appear
            this.InitPosition();
        }
    }
}
