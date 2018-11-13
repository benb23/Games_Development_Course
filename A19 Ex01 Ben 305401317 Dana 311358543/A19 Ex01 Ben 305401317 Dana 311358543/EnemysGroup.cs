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
            float leftBorderX = 0;

            for(int col=0 ; col < 9; col++)//TODO: CONST
            {
                for(int row=0; row < 5; row++)//TODO: CONST
                {
                    if(m_enemiesMatrix[col, row].m_visible)
                    {
                        leftBorderX = m_enemiesMatrix[col, row].Position.X;
                        break;
                    }
                }
            }

            return leftBorderX;
        }

        public float RightBorder()
        {
            float rightBorderX = 0;

            for (int col = 8; col >= 0 ; col--)//TODO: CONST
            {
                for (int row = 0; row < 5; row++)//TODO: CONST
                {
                    if (m_enemiesMatrix[col, row].m_visible)
                    {
                        rightBorderX = m_enemiesMatrix[col, row].Position.X + m_enemiesMatrix[col, row].Texture.Width;
                        break;
                    }
                }
            }

            return rightBorderX;
        }

        public void Move()
        {

        }

        public EnemysGroup(Game game)
        {
            //matrix of enemy
            for (int row=0 ; row<5 ; row++)
            {
                for (int col=0 ; col<9 ; col++)
                {
                    m_enemiesMatrix[col, row] = new Enemy(game);
                }
            }
        }

        public void Init()
        {

        }

        private void changeDirection()
        {

        }

        public void LoadContent()
        {

        }
    }
}
