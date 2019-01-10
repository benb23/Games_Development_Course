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
using Microsoft.Xna.Framework;
using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public class Bullet : CollidableSprite, IPixelsCollidable, IRectangleCollidable
    {
        private IGameEngine m_GameEngine;
        public enum eBulletType 
        {
            PlayerOneBullet,
            PlayerTwoBullet,
            EnemyBullet 
        }
        

        private const string k_AssteName = @"Sprites\Bullet";
        private Vector2 k_BulletVelocity = new Vector2(0,155);
        private eBulletType m_Type;

        public Bullet(Game i_Game, eBulletType i_BulletType) : base(k_AssteName, i_Game)
        {
            this.m_Type = i_BulletType;

            if (this.m_Type == eBulletType.EnemyBullet)
            {
                Velocity = k_BulletVelocity;
                this.m_TintColor = Color.Blue;
            }
            else
            {
                Velocity = k_BulletVelocity *new Vector2(0,-1);
                this.m_TintColor = Color.Red;
                k_BulletVelocity *= -1;
            }
            InitOrigins();
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
            bool isBulletHit = m_Position.Y + Texture.Height/2 <= 0 || m_Position.Y - Texture.Height / 2 >= Game.GraphicsDevice.Viewport.Height;

            return isBulletHit;
        }

        void ICollidable.Collided(ICollidable i_Collidable)
        {
            if(m_GameEngine==null)
            {
                m_GameEngine = Game.Services.GetService(typeof(IGameEngine)) as IGameEngine;
            }

            m_GameEngine.HandleHit(this, i_Collidable);
        }

        protected override void InitOrigins()
        {
            m_PositionOrigin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            base.InitOrigins();
        }

        public override void Draw(GameTime gameTime)
        {
            m_SpriteBatch.Begin();
            DrawWithAllParameters();
            m_SpriteBatch.End();
        }
    }
}
