using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Infrastructure;

namespace A19_Ex03_Ben_305401317_Dana_311358543
{
    public class Gun
    {
        private readonly int r_MaxNumOfBullets;
        private List<Bullet> m_Bullets;
        private GameScreen m_GameScreen;
        private Bullet.eBulletType m_BulletsType;
        private int m_ShootingDirection;
        private string m_ShotSound;
        private ISoundMananger m_SoundManager;

        public Gun(GameScreen i_GameScreen, int i_MaxNumOfBullets, Bullet.eBulletType i_BulletType, int i_ShootingDirection, string i_ShotSound)
        {
            this.m_GameScreen = i_GameScreen;
            this.r_MaxNumOfBullets = 100; // i_MaxNumOfBullets;
            this.m_BulletsType = i_BulletType;
            this.m_Bullets = new List<Bullet>(this.r_MaxNumOfBullets);
            this.m_ShootingDirection = i_ShootingDirection;
            this.m_ShotSound = i_ShotSound; 
        }

        public void InitGunForNextLevel()
        {
            foreach(Bullet bullet in this.m_Bullets)
            {
                bullet.Enabled = false;
                bullet.Visible = false;
            }
        }

        public void Shoot(Vector2 i_ShooterPosition)
        {
            Bullet bullet = this.getBullet(i_ShooterPosition);

            this.m_SoundManager = this.m_GameScreen.Game.Services.GetService(typeof(ISoundMananger)) as ISoundMananger;

            if (this.m_ShotSound != string.Empty && this.m_SoundManager != null)
            {
                this.m_SoundManager.PlaySoundEffect(this.m_ShotSound);
            }
        }

        private Bullet getBullet(Vector2 i_ShooterPosition)
        {
            Bullet bullet = null;
            bool foundBullet = false;

            foreach (Bullet currBullet in this.m_Bullets)
            {
                if(!currBullet.Visible)
                {
                    bullet = currBullet;
                    foundBullet = true;
                    bullet.Position = i_ShooterPosition + new Vector2(0, this.m_ShootingDirection * ((bullet.Texture.Height / 2) + 1));
                    bullet.Enabled = true;
                    bullet.Visible = true;
                }

                if(foundBullet)
                {
                    break;
                }
            }

            if(!foundBullet)
            {
                if(this.m_Bullets.Count < this.r_MaxNumOfBullets)
                {
                    bullet = new Bullet(this.m_GameScreen, this.m_BulletsType);
                    bullet.Position = i_ShooterPosition + new Vector2(0, this.m_ShootingDirection * ((bullet.Texture.Height / 2) + 1));
                    this.m_Bullets.Add(bullet);
                }
            }

            return bullet;
        }

        public bool PermitionToShoot()
        {
            bool PermitionToShoot = false;

            if (this.m_Bullets.Count < this.r_MaxNumOfBullets)
            {
                PermitionToShoot = true;
            }

            if(!PermitionToShoot)
            {
                foreach(Bullet currBullet in this.m_Bullets)
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
