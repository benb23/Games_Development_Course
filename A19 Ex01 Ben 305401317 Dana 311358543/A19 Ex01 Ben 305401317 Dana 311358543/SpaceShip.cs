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
    class SpaceShip : GameObject
    {
        private float m_NumOfBullets;
        private Gun m_Gun;

        public Gun Gun
        {
            get{ return m_Gun; }
        }

        public SpaceShip()
        {
            m_Direction = 1f;
        }
        public void Init()
        {

        }
        public override void Move()
        {
            
        }

        public void  Shoot()
        {

        }


    }
}
