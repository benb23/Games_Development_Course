using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public class Wall : CollidableSprite, IPixelsCollidable, IRectangleCollidable
    {
        private const int k_NumOfWalls = 4;
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

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if(!this.m_Initialize)
            {
                this.m_StartingPosition = this.Position;
                this.m_Initialize = true;
            }
            if (m_GameEngine.Level != SpaceInvadersEngine.eLevel.One)
            {
                moveWall();
            }
            base.Update(gameTime);
        }

        private void moveWall()
        {
            if ((Position.X - this.m_StartingPosition.X >= (Texture.Width / 2)) || (Position.X + (Texture.Width / 2) <= this.m_StartingPosition.X))
            {
                this.Velocity *= -1;
            }
        }
        protected override void InitOrigins()
        {
            this.m_PositionOrigin = new Vector2(Texture.Width / 2, Texture.Height);
            base.InitOrigins();
        }

        void ICollidable.Collided(ICollidable i_Collidable)
        {
            if(this.CurrTexture == null)
            {
                this.CurrTexture = new Texture2D(Game.GraphicsDevice, Texture.Width, Texture.Height);
                //Color[] texturePixels = new Color[Texture.Width * Texture.Height];
                this.CurrTexture.SetData(OriginalPixels);
            }

            this.m_GameEngine.HandleHit(this, i_Collidable);
        }

        //public void createNewTexture()
        //{
        //    this.CurrTexture = new Texture2D(Game.GraphicsDevice, Texture.Width, Texture.Height);
        //    //Color[] texturePixels = new Color[Texture.Width * Texture.Height];
        //    this.CurrTexture.SetData(OriginalPixels);
        //}
    }
}
