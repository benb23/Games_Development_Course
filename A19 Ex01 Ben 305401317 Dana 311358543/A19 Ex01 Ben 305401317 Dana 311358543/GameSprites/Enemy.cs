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
    public class Enemy : Sprite
    {
        private Gun m_Gun = new Gun(); 

        public Enemy(Game i_Game, Color i_Tint, string i_AssetName) : base(i_Game)
        {
            this.m_AssetName = i_AssetName;
            this.m_Tint = i_Tint;
        }

        public override void Update(GameTime i_GameTime)
        {
            int rnd = SpaceInvaders.s_RandomNum.Next(0, 55555);
            
            if (rnd <= 10)
            {
                this.m_Gun.Shoot(new Bullet(Game, Bullet.eBulletType.EnemyBullet, this));    // game?
            }
        }

        public override void InitPosition() // TODO: ??
        {
        }
    }
}
