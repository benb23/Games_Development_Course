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
    public class Player : IScoreable
    {
        public enum ePlayer //change the direction
        {
            PlayerOne,
            PlayerTwo,
        }

        Game m_Game;

        public const int k_MaxNumOfBullets = 3;

        private List<Bullet> m_BulletList = new List<Bullet>(k_MaxNumOfBullets);

        Bullet.eBulletType m_BulletsType;

        private Gun m_Gun;

        private SpaceShip m_SpaceShip;

        private List<Soul> m_Souls;

        private int m_Score;

        public SpaceShip SpaceShip
        {
            get { return m_SpaceShip; }
        }

        public int CountNumOfVisibleBullets()
        {
            int numOfVisibleBullets = 0;

            foreach (Bullet element in this.m_BulletList)
            {
                if (element.Visible)
                {
                    numOfVisibleBullets++;
                }
            }

            return numOfVisibleBullets;
        }
        public Player(Game i_Game,ePlayer i_PlayerType)
        {
            m_Souls = new List<Soul>(3);
            m_SpaceShip = new SpaceShip(i_Game);
            i_Game.Components.Add(this.m_SpaceShip);
            m_Game = i_Game;
            if(i_PlayerType==ePlayer.PlayerOne)
            {
                m_BulletsType = Bullet.eBulletType.PlayerOneBullet;
            }
            else
            {
                m_BulletsType = Bullet.eBulletType.PlayerTwoBullet;
            }
        }

        public void Shoot()
        {
            Bullet currBullet;

            currBullet = this.getBullet();
            this.m_Gun.Shoot(currBullet, m_SpaceShip.Position, m_Game);
        }

        private Bullet getBullet()
        {
            Bullet currBullet;

            if (this.m_BulletList.Count < k_MaxNumOfBullets)
            {
                currBullet = new Bullet(m_Game, m_BulletsType);
                this.m_BulletList.Add(currBullet);
            }
            else
            {
                currBullet = this.getUnVisibleBulletFromList();
                currBullet.Visible = true;
                currBullet.Position = m_SpaceShip.Position;
            }

            return currBullet;
        }

        public bool IsFreeBulletExists()
        {
            bool IsFreeBulletExists;

            for (int i=0 ; i< k_MaxNumOfBullets; i++)
            {
                if(m_BulletList[i].Visible == false)
                {
                    IsFreeBulletExists = true;
                }
            }

            IsFreeBulletExists = false;

            return IsFreeBulletExists;
        }
        private Bullet getUnVisibleBulletFromList()
        {
            Bullet bullet = null;

            foreach (Bullet element in this.m_BulletList)
            {
                if (!element.Visible)
                {
                    bullet = element;
                }
            }

            return bullet;
        }
        public int Score { get; set; }
    }
}
