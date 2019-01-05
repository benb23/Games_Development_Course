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

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public class Gun
    {
        private List<Bullet> m_Bullets;
        private int k_MaxNumOfBullets;
        private Game m_Game;
        private Bullet.eBulletType m_BulletsType;
        private int m_ShootingDirection;
        public Gun(Game i_Game,int i_MaxNumOfBullets, Bullet.eBulletType i_BulletType,int i_ShootingDirection)
        {
            m_Game = i_Game;
            k_MaxNumOfBullets = i_MaxNumOfBullets;
            m_BulletsType = i_BulletType;
            m_Bullets = new List<Bullet>(k_MaxNumOfBullets);
            m_ShootingDirection = i_ShootingDirection;
        }

        public void Shoot(Vector2 i_ShooterPosition)
        {
            Bullet bullet = getBullet();
            bullet.Position = i_ShooterPosition + new Vector2(0, m_ShootingDirection*(bullet.Texture.Height/2+1));
        }

        private Bullet getBullet()
        {
            Bullet bullet = null;
            bool foundBullet = false;

            foreach (Bullet currBullet in m_Bullets)
            {
                if(!currBullet.Visible)
                {
                    bullet = currBullet;
                    foundBullet = true;
                    bullet.Enabled = true;
                    bullet.Visible = true;
                    break;
                }
            }

            if(!foundBullet)
            {
                if(m_Bullets.Count < k_MaxNumOfBullets)
                {
                    bullet = new Bullet(m_Game, m_BulletsType);
                    m_Bullets.Add(bullet);
                }
            }

            if(bullet==null)
            {
                bullet = null;
            }
            return bullet;
        }
        public bool PermitionToShoot()
        {
            bool PermitionToShoot = false;

            if (m_Bullets.Count < k_MaxNumOfBullets)
            {
                PermitionToShoot = true;
            }

            if(!PermitionToShoot)
            {
                foreach(Bullet currBullet in m_Bullets)
                {
                    if(!currBullet.Visible)
                    {
                        PermitionToShoot = true;
                    }
                }
            }
            return PermitionToShoot;
        }
    }
}
