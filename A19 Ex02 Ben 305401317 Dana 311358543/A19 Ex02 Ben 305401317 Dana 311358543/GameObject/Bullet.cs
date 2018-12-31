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
    public class Bullet : Sprite, ICollidable2D
    {
        public enum eBulletType
        {
            SpaceShipBullet = -1,
            EnemyBullet = 1
        }

        private const string k_AssteName = @"Sprites\Bullet";
        private const float k_BulletVelocity = 155;
        private eBulletType m_Type;

        public eBulletType Type
        {
            get { return this.m_Type; }
        }

        public override void Update(GameTime i_GameTime)
        {
            ShootingManager shootingManager = SpaceInvaders.s_GameUtils.ShootingManager;

            if (this.isBulletHitTheScreenBorder())
            {
                this.RemoveComponent();
            }

            Sprite hittenSprite = shootingManager.IsGameObjectWasHitten(this);

            if (hittenSprite != null)
            {
                shootingManager.OnHit(this, hittenSprite);
            }
            else
            {
                this.Position = new Vector2(this.Position.X, this.Position.Y + ((float)this.m_Type * k_BulletVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds));
            }
        }

        private bool isBulletHitTheScreenBorder()
        {
            bool isBulletHit = m_Position.Y <= 0 || m_Position.Y >= Game.GraphicsDevice.Viewport.Height;

            return isBulletHit;
        }

        public Bullet(Game i_Game, eBulletType i_BulletType) : base(k_AssteName ,i_Game)
        {
            this.m_Type = i_BulletType;

            if (this.m_Type == eBulletType.EnemyBullet)
            {
                this.m_TintColor = Color.Blue;
            }
            else
            {
                this.m_TintColor = Color.Red;
            }

            m_Velocity =new Vector2 (0,k_BulletVelocity); // TODO: in initialize???
        }

        void ICollidable.Collided(ICollidable i_Collidable)
        {

        }

        bool ICollidable.CheckCollision(ICollidable i_Source)
        {
            return false;
        }

    }
}
