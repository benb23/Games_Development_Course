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
    public class Enemy : Sprite, ICollidable2D
    {
        private List<Bullet> m_Bullets = new List<Bullet>(5);
        private const string k_AssteName = @"Sprites\Enemy01_32x32";
        private Gun m_Gun;
        private const int k_MaxRandomToShoot = 10; //TODO: LOCATION?
        public const int k_MaxRandomNumber = 50000; //TODO: LOCATION?
        private IGameEngine m_GameEngine;

        public Enemy(Game i_Game,Color i_Tint) : base(k_AssteName, i_Game)
        {
            m_TintColor = i_Tint;
            AssetName = k_AssteName;
            m_Gun = new Gun();
        }

        public void LoadAsset()
        {
            this.Texture = this.Game.Content.Load<Texture2D>(this.m_AssetName);
        }

        public override void Update(GameTime i_GameTime)
        {
            int rnd = SpaceInvaders.s_RandomNum.Next(0, k_MaxRandomNumber);

            if (rnd <= k_MaxRandomToShoot)
            {
                Bullet bullet = getBullet();
                bullet.Position = new Vector2(Position.X, Position.Y + Texture.Height / 2 + bullet.Texture.Height/2 + 1);
                this.m_Gun.Shoot(bullet ,Game);
            }
        }

        private Bullet getBullet()
        {
            Bullet bullet = null;
            bool freeBulletFound=false;

            if(m_Bullets.Count > 0)
            {
                foreach (Bullet currBullet in m_Bullets)
                {
                    if(Visible==false)
                    {
                        bullet = currBullet;
                        bullet.AddComponent();
                        freeBulletFound = true;
                        break;
                    }
                }
            }
            
            if(!freeBulletFound)
            {
                bullet = new Bullet(Game, Bullet.eBulletType.EnemyBullet);
                m_Bullets.Add(bullet);
            }
          
            return bullet;
        }

        void ICollidable.Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet && (i_Collidable as Bullet).Type == Bullet.eBulletType.EnemyBullet)
            {
                return;
            }
            else
            {
                if (m_GameEngine == null)
                {
                    m_GameEngine = Game.Services.GetService(typeof(IGameEngine)) as IGameEngine;
                }

                if ((i_Collidable as Bullet).Type != Bullet.eBulletType.EnemyBullet)
                {
                    Visible = false;
                }

                m_GameEngine.HandleHit(this, i_Collidable);
            }
        }

        protected override void InitOrigins()
        {
            m_PositionOrigin = new Vector2(Texture.Width / 2, Texture.Height/2);
            m_RotationOrigin = new Vector2(Texture.Width / 2, Texture.Height/2);
            base.InitOrigins();
        }
    }
}
