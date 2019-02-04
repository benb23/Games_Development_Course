using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure;

namespace A19_Ex03_Ben_305401317_Dana_311358543
{
    public class Wall : CollidableSprite, IPixelsCollidable, IRectangleCollidable
    {
        private const string k_AssteName = @"Sprites\Barrier_44x32";
        private ISpaceInvadersEngine m_GameEngine;
        private Vector2 m_StartingPosition;
        
        public Wall(GameScreen i_GameScreen)
            : base(k_AssteName, i_GameScreen)
        {
            if (this.m_GameEngine == null)
            {
                this.m_GameEngine = Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if(!this.m_Initialize)
            {
                this.m_StartingPosition = this.Position;
                this.m_Initialize = true;
            }

            if (SpaceInvadersConfig.s_LogicLevel != SpaceInvadersConfig.eLevel.One)
            {
                this.moveWall();
            }

            base.Update(gameTime);
        }

        private void moveWall()
        {
            if ((this.Position.X - this.m_StartingPosition.X >= (this.Texture.Width / 2)) || (this.Position.X + (this.Texture.Width / 2) <= this.m_StartingPosition.X))
            {
                this.Velocity *= -1;
            }
        }

        protected override void InitOrigins()
        {
            this.m_PositionOrigin = new Vector2(this.Texture.Width / 2, this.Texture.Height);
            base.InitOrigins();
        }

        void ICollidable.Collided(ICollidable i_Collidable)
        {
            if(this.CurrTexture == null)
            {
                this.CurrTexture = new Texture2D(this.Game.GraphicsDevice, this.Texture.Width, this.Texture.Height);
                this.CurrTexture.SetData(this.OriginalPixels);
            }

            this.m_GameEngine.HandleHit(this, i_Collidable);
        }
    }
}
