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
        }

        private readonly float r_BulletVelocity = 155;
        private eBulletType m_Type;


        public eBulletType Type
        {
            get { return this.m_Type; }
        }

        public override void Update(GameTime gameTime)
        {
            ShootingManager shootingManager = Game.Services.GetService(typeof(ShootingManager)) as ShootingManager;

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
                this.Position = new Vector2(this.Position.X, this.Position.Y + ((float)this.m_Type * this.r_BulletVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds));
            }
        }

        private bool isBulletHitTheScreenBorder()
        {
            bool isBulletHit = m_Position.Y <= 0 || m_Position.Y >= Game.GraphicsDevice.Viewport.Height;

            return isBulletHit;
        }

        public Bullet(Game game, eBulletType bulletType, Sprite shooter ) : base(game)
        {
            this.m_AssetName = @"Sprites\Bullet";
            this.m_Type = bulletType;

            if(this.m_Type == eBulletType.EnemyBullet)
            {
                this.m_Tint = Color.Blue;
            }
            else
            {
                this.m_Tint = Color.Red;
            }

            this.initBulletPosition(shooter);
        }

        // TODO: ??
        public void initBulletPosition(Sprite i_Shooter) 
        {
            this.Position = new Vector2(i_Shooter.Position.X + (i_Shooter.Texture.Width / 2), i_Shooter.Position.Y + ((float)this.m_Type * (1 + i_Shooter.Texture.Height))); 
        }

        // TODO: ??
        public override void initPosition() 
        {  
        }
    }
}
