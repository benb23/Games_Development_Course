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
    public class Bullet : CollidableSprite, IPixelsCollidable, IRectangleCollidable
    {
        public enum eBulletType 
        {
            PlayerOneBullet,
            PlayerTwoBullet,
            EnemyBullet 
        }

        private const string k_AssteName = @"Sprites\Bullet";
        private ISpaceInvadersEngine m_GameEngine;
        private Vector2 k_BulletVelocity = new Vector2(0, 155);
        private eBulletType m_Type;

        public Bullet(Game i_Game, eBulletType i_BulletType) : base(k_AssteName, i_Game)
        {
            this.m_Type = i_BulletType;

            if (this.m_Type == eBulletType.EnemyBullet)
            {
                this.Velocity = this.k_BulletVelocity;
                this.m_TintColor = Color.Blue;
            }
            else
            {
                this.Velocity = this.k_BulletVelocity * new Vector2(0, -1);
                this.m_TintColor = Color.Red;
                this.k_BulletVelocity *= -1;
            }

            this.InitOrigins();
        }

        public eBulletType Type
        {
            get { return this.m_Type; }
        }

        public override void Update(GameTime i_GameTime)
        {
            if (this.isBulletHitTheScreenBorder())
            {
                this.Visible = false;
                this.Enabled = false;
            }

            base.Update(i_GameTime);
        }

        private bool isBulletHitTheScreenBorder()
        {
            bool isBulletHit = this.m_Position.Y + (this.Texture.Height / 2) <= 0 || this.m_Position.Y - (this.Texture.Height / 2) >= this.Game.GraphicsDevice.Viewport.Height;

            return isBulletHit;
        }

        void ICollidable.Collided(ICollidable i_Collidable)
        {
            if(this.m_GameEngine == null)
            {
                this.m_GameEngine = Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
            }

            this.m_GameEngine.HandleHit(this, i_Collidable);
        }

        protected override void InitOrigins()
        {
            this.m_PositionOrigin = new Vector2(this.Texture.Width / 2, this.Texture.Height / 2);
            base.InitOrigins();
        }
    }
}
