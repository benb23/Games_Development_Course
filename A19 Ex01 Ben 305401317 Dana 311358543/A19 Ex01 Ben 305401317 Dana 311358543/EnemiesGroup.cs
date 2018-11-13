﻿using System;
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
    class EnemiesGroup : DrawableGameComponent
    {
       

        private Enemy[,] m_EnemiesMatrix = new Enemy[5, 9];
        private float m_JumpingVelocity;

        private const int k_EnemiesRows = 5;
        private const int k_EnemiesColumns = 9;

        public override void Update(GameTime gameTime)
        {
            ////update enemysGroup position
            ////move{
            //if (LeftBorder() == 0f || RightBorder() == SpaceInvaders.graphics.GraphicsDevice.Viewport.Width)
            //{

            //}
            //else if (LeftBorder() < 0.5 * m_enemiesMatrix[0, 0].Texture.Width || RightBorder() > SpaceInvaders.graphics.GraphicsDevice.Viewport.Width - 0.5 * m_enemiesMatrix[0, 0].Texture.Width)
            //{

            //}
            //else
            //{

            //}
            ////}

            ////update enemies bullets position
            ///*
            //if (Gun.BulletsList.Count != 0)
            //{

            //}*/
        }

        //public float LeftBorder()
        //{
        //    //float leftBorderX = 0;

        //    //for(int col=0 ; col < 9; col++)//TODO: CONST
        //    //{
        //    //    for(int row=0; row < 5; row++)//TODO: CONST
        //    //    {
        //    //        if(m_enemiesMatrix[col, row].m_visible)
        //    //        {
        //    //            leftBorderX = m_enemiesMatrix[col, row].Position.X;
        //    //            break;
        //    //        }
        //    //    }
        //    //}

        //    //return leftBorderX;
        //}

        //public float RightBorder()
        //{
        //    //float rightBorderX = 0;

        //    //for (int col = 8; col >= 0 ; col--)//TODO: CONST
        //    //{
        //    //    for (int row = 0; row < 5; row++)//TODO: CONST
        //    //    {
        //    //        if (m_enemiesMatrix[col, row].m_visible)
        //    //        {
        //    //            rightBorderX = m_enemiesMatrix[col, row].Position.X + m_enemiesMatrix[col, row].Texture.Width;
        //    //            break;
        //    //        }
        //    //    }
        //    //}

        //    //return rightBorderX;
        //}

        public void Move()
        {

        }

        public EnemiesGroup(Game i_Game) : base(i_Game)
        {
        }

        public override void Initialize()
        {
            this.InitEnemyGroup();
            base.Initialize();
            initPosions();

        }

        private void InitEnemyGroup()
        {
            // TODO: initilize the EnemiesGroup matrix
            InitEnemiesRow(0, @"Sprites\Enemy0101_32x32", Color.Pink); //firstROW - Enemy0101_32x32
            for (int i = 1; i < k_EnemiesRows; i++)
            {
                if ( i < 3)
                {
                    InitEnemiesRow(i, @"Sprites\Enemy0201_32x32", Color.LightBlue);
                }
                else 
                {
                    InitEnemiesRow(i, @"Sprites\Enemy0301_32x32", Color.LightYellow);
                }
            }
        }

        private void InitEnemiesRow(int i_Row, string i_AssetName, Color i_Tint)
        {
            for (int colum = 0; colum < k_EnemiesColumns; colum++)
            {
                m_EnemiesMatrix[i_Row, colum] = new Enemy(Game) { AssetName = i_AssetName, Tint = i_Tint };
                m_EnemiesMatrix[i_Row, colum].AddComponent();
            }
        }

        public void initPosions()
        {
            float enemyHeight = 32f;
            float enemiesGap = enemyHeight * 0.6f;
            float startX = 0f;
            float strartY = enemyHeight * 3f;

            for (int i = 0; i < k_EnemiesRows; i++)
            {
                for (int j = 0; j < k_EnemiesColumns; j++)
                { 
                    m_EnemiesMatrix[i, j].Position = new Vector2(startX, strartY);
                    startX += enemyHeight + enemiesGap;
                }
                startX = 0f;
                strartY += enemyHeight + enemiesGap;
            }
        }

        private void changeDirection()
        {

        }

        //public void LoadContent()
        //{

        //}
    }
}
