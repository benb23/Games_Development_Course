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
    class EnemiesGroup : DrawableGameComponent
    {
       

        private Enemy[,] m_EnemiesMatrix = new Enemy[5, 9];
        private float m_JumpingVelocity;
        private float m_Direction = 1f;
        private float m_currTopLeftX;
        private float m_currTopLeftY;
        private float m_JumpFrequency = 2;
        private float m_FrequencRateIncreasment = 1.08f;



        private const int k_EnemiesRows = 5;
        private const int k_EnemiesColumns = 9;
        private const float k_enemyHeight = 32f;

        public override void Update(GameTime i_GameTime)
        {
            ////update enemysGroup position
        
             
            JumpHorizontalStep(i_GameTime);
        }

        public float getLeftGroupBorder()  // ?? : Use as a first alive function
        {
            float leftX = 0;
            bool isFound = false;

            for (int row = 0; row < k_EnemiesRows; row++)//TODO: CONST
            {
                for (int col = 0; col < k_EnemiesColumns; col++)//TODO: CONST
                {
                    if (m_EnemiesMatrix[row, col].m_visible)
                    {
                        leftX = m_EnemiesMatrix[row, col].Position.X;
                        isFound = true;
                        break;
                    }
                }
                if (isFound)
                    break;
            }

            return leftX;
        }

        public float getRightGroupBorder()
        {
            float rightBorderX = 0;
            bool isFound = false;

            for (int row = 0; row < k_EnemiesRows; row++)//TODO: CONST
            {
                for (int col = k_EnemiesColumns - 1; col >= 0; col--)//TODO: CONST
                {
                    if (m_EnemiesMatrix[row, col].m_visible)
                    {
                        rightBorderX = m_EnemiesMatrix[row, col].Position.X + m_EnemiesMatrix[row, col].Texture.Width;
                        isFound = true;
                        break;
                    }
                }
                if (isFound)
                    break;
            }

            return rightBorderX;
        }

        public float getBottomGroupBorder()
        {
            float bottomBorderY = 0;
            bool isFound = false;

            for (int row = k_EnemiesRows - 1; row >= 0; row--)//TODO: CONST
            {
                for (int col = 0; col < k_EnemiesColumns; col++)//TODO: CONST
                {
                    if (m_EnemiesMatrix[row, col].m_visible)
                    {
                        bottomBorderY = m_EnemiesMatrix[row, col].Position.Y + m_EnemiesMatrix[row, col].Texture.Height;
                        isFound = true;
                        break;
                    }
                }
                if (isFound)
                    break;
            }
            return bottomBorderY;
        }

        public void Move()
        {

        }

        public EnemiesGroup(Game i_Game) : base(i_Game)
        {
        }

        public override void Initialize()
        {
            m_currTopLeftX = 0;
            m_currTopLeftY = k_enemyHeight * 3f;
            this.InitEnemyGroup();
            base.Initialize();
            initPosions(m_currTopLeftX, m_currTopLeftY);

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

        public void initPosions(float i_X, float i_Y)
        {
            float enemiesGap = k_enemyHeight * 0.6f;
            float startX = i_X;
            float strartY = i_Y;

            for (int i = 0; i < k_EnemiesRows; i++)
            {
                for (int j = 0; j < k_EnemiesColumns; j++)
                { 
                    m_EnemiesMatrix[i, j].Position = new Vector2(startX, strartY);
                    startX += k_enemyHeight + enemiesGap;
                }
                startX = i_X;
                strartY += k_enemyHeight + enemiesGap;
            }
        }

        private void JumpHorizontalStep(GameTime i_GameTime)
        {
            if (getRightGroupBorder() >= SpaceInvaders.graphics.GraphicsDevice.Viewport.Width)
            {
                m_Direction = -1f;
                JumpDown();
            }
            else if (getLeftGroupBorder() <= 0f)
            {
                m_Direction = 1f;
                JumpDown();
            }
            initPosions(m_currTopLeftX, m_currTopLeftY);
            m_currTopLeftX += m_Direction * (m_JumpFrequency * (float)i_GameTime.ElapsedGameTime.TotalSeconds * (m_EnemiesMatrix[0, 0].Texture.Width / 2));
        }

        private void JumpDown()
        {
            m_currTopLeftY += m_EnemiesMatrix[0, 0].Texture.Width / 2;
            m_JumpFrequency *= m_FrequencRateIncreasment;
        }
    }
}
