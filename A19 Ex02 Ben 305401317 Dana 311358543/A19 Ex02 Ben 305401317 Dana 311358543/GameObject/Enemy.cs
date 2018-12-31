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
        private const string k_AssteName = @"Sprites\Enemy01_32x32";
        private Gun m_Gun;
        private const int k_MaxRandomToShoot = 10; //TODO: LOCATION?
        public const int k_MaxRandomNumber = 50000; //TODO: LOCATION?


        public Enemy(Game i_Game, Color i_EnemyColor)
            : base(k_AssteName, i_Game)
        {
            m_TintColor = i_EnemyColor;
        }

        public override void Update(GameTime i_GameTime)
        {
            int rnd = SpaceInvaders.s_RandomNum.Next(0, k_MaxRandomNumber);

            if (rnd <= k_MaxRandomToShoot)
            {
                this.m_Gun.Shoot(Bullet.eBulletType.EnemyBullet,m_Origin ,Game); //TODO: is it class Game or space invaders?
            }
        }
        void ICollidable.Collided(ICollidable i_Collidable)
        {
                Visible = false;

        }
    }
    
}
