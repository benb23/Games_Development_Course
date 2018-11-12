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
    class EnemysGroup : GameObject
    {
        private Gun m_Gun;
        private Enemy[,] m_enemiesMatrix = new Enemy[9, 5];
        private float m_JumpingVelocity;

        public override void Move()
        {

        }
        public Gun Gun
        {
            get { return m_Gun; }
        }

        public EnemysGroup()
        {
            //matrix of enemy
        }

        public void Init()
        {

        }

        private void changeDirection()
        {

        }
    }
}
