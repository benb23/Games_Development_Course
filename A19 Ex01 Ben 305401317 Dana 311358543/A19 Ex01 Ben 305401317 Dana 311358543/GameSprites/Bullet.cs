using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace A19_Ex01_Ben_305401317_Dana_311358543
{
    public class Bullet : Sprite
    {
        public enum eBulletType
        {
            SpaceShipBullet = -1,
            EnemyBullet = 1
        };

        private eBulletType m_Type;
        private readonly float r_BulletVelocity = 155;

        public eBulletType Type
        {
            get { return m_Type; }
        }

        public override void Update(GameTime gameTime)
        {
            ShootingManager shootingManager = (Game.Services.GetService(typeof(ShootingManager)) as ShootingManager);

            if (isBulletHitTheScreenBorder())
            {
                RemoveComponent();
            }

            Sprite hittenSprite = shootingManager.IsGameObjectWasHitten(this);

            if ( hittenSprite != null)
            {
                shootingManager.OnHit(this,hittenSprite);
            }
            else
            {
                Position = new Vector2(Position.X, Position.Y + (float)m_Type * r_BulletVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        private bool isBulletHitTheScreenBorder()
        {
            bool isBulletHit = m_Position.Y <= 0 || m_Position.Y >= Game.GraphicsDevice.Viewport.Height;

            return isBulletHit;
        }

        public Bullet(Game game, eBulletType bulletType, Sprite shooter ) :base(game)
        {
            m_AssetName = @"Sprites\Bullet";
            m_Type = bulletType;

            if(m_Type == eBulletType.EnemyBullet)
            {
                m_Tint = Color.Blue;
            }
            else
            {
                m_Tint = Color.Red;
            }

            initBulletPosition(shooter);
        }

        public void initBulletPosition(Sprite i_Shooter) //TODO: ??
        {
            Position=new Vector2(i_Shooter.Position.X + i_Shooter.Texture.Width/ 2, i_Shooter.Position.Y +(float)m_Type*(1+i_Shooter.Texture.Height));//TODO: CONST 32 SHOOTER WIDTH
        }

        public override void initPosition() //TODO: ??
        {
           
        }
    }
}
