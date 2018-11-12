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
    class EnemysGroup 
    {
        private Enemy[,] m_enemiesMatrix = new Enemy[9, 5];
        private float m_JumpingVelocity;

        public void Update(GameTime gameTime)
        {
            //update enemysGroup position
            //move{
            if (LeftBorder() == 0f || RightBorder() == SpaceInvaders.graphics.GraphicsDevice.Viewport.Width)
            {

            }
            else if (LeftBorder() < 0.5 * m_enemiesMatrix[0, 0].Texture.Width || RightBorder() > SpaceInvaders.graphics.GraphicsDevice.Viewport.Width - 0.5 * m_enemiesMatrix[0, 0].Texture.Width)
            {

            }
            else
            {

            }
            //}

            //update enemies bullets position
            /*
            if (Gun.BulletsList.Count != 0)
            {

            }*/
        }

        public float LeftBorder()
        {
            int x = 0;
            int col = 0, row = 0;

            for(int i=0 ; i<=5 ; i++)
            {

            }
        }

        public float RightBorder()
        {

        }

        public void Move()
        {

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
