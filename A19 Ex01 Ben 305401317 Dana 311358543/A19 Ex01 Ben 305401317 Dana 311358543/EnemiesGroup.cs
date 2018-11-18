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

        private float m_Direction = 1f;
        private float m_currTopLeftX;
        private float m_currTopLeftY;
        // private float m_JumpFrequency = 2;
        // private float m_FrequencRateIncreasment = 1.08f;
        private const int k_EnemiesRows = 5;
        private const int k_EnemiesColumns = 9;
        private Enemy[,] m_EnemiesMatrix = new Enemy[k_EnemiesRows, k_EnemiesColumns];
        private const float k_enemyHeight = 32f;
        private bool m_isLastStepInRow = false;
        private float m_ElapsedTime = 0f;
        private float m_TimeUntilNextStepInSec = 0.5f;


        public override void Update(GameTime i_GameTime)
        {

            m_ElapsedTime += (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            if (m_ElapsedTime >= m_TimeUntilNextStepInSec)
            {
                m_ElapsedTime = 0;

                if (m_isLastStepInRow)
                {
                    m_isLastStepInRow = false;
                    JumpDown();
                    m_Direction *= -1f;
                }
                else
                {
                    JumpHorizontalStep(i_GameTime);
                }
            }

            /*
            if ((countNumOfVisibleEnemies() < k_EnemiesRows * k_EnemiesColumns) && ((k_EnemiesRows * k_EnemiesColumns - countNumOfVisibleEnemies()) % 4 == 0))
            {
                m_TimeUntilNextStepInSec -= (m_TimeUntilNextStepInSec * 0.04f);
            }*/

            initPosions(m_currTopLeftX, m_currTopLeftY);

            if (isEnemiesGroupTouchTheBotton()|| countNumOfVisibleEnemies()==0)
            {
                Game.Exit();
            }
        }

        private int countNumOfVisibleEnemies()
        {
            int NumOfVisibleEnemies=0;

            foreach (Enemy enemy in m_EnemiesMatrix)
            {
                if(enemy.Visible)
                {
                    NumOfVisibleEnemies++;
                }
            }

            return NumOfVisibleEnemies;
        }

        private bool isEnemiesGroupTouchTheBotton()
        {
            bool isEnemiesGroupTouchTheBotton;

            if (getBottomGroupBorder() >= GraphicsDevice.Viewport.Height)
            {
                isEnemiesGroupTouchTheBotton = true;
            }
            else
            {
                isEnemiesGroupTouchTheBotton = false;
            }

            return isEnemiesGroupTouchTheBotton;
        }

        public float getLeftGroupBorder()  
        {
            float leftX = 0;
            bool isFound = false;

            for (int col = 0; col < k_EnemiesColumns; col++)//TODO: CONST
            {
                for (int row = 0; row < k_EnemiesRows; row++)//TODO: CONST
                {
                    if (m_EnemiesMatrix[row, col].Visible)
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

            for (int col = k_EnemiesColumns-1; col >=0; col--)//TODO: CONST
            {
                for (int row = 0; row < k_EnemiesRows; row++)//TODO: CONST
                {
                    if (m_EnemiesMatrix[row, col].Visible)
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
                    if (m_EnemiesMatrix[row, col].Visible)
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

        public EnemiesGroup(Game i_Game) : base(i_Game)
        {
        }

        public override void Initialize()
        {
            m_currTopLeftX = 0;
            m_currTopLeftY = k_enemyHeight  * 3f;
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
                float lastRightJump = GraphicsDevice.Viewport.Width - getRightGroupBorder();
                float lastLeftJump = getLeftGroupBorder();

                
                if (lastRightJump < (m_EnemiesMatrix[0, 0].Texture.Width / 2) && lastRightJump > 0)
                {
                    m_currTopLeftX += m_Direction * lastRightJump;
                    m_isLastStepInRow = true;
                }
                else if (lastLeftJump < (m_EnemiesMatrix[0, 0].Texture.Width / 2) && lastLeftJump > 0 && m_Direction == -1f )
                {
                    m_currTopLeftX += m_Direction * lastLeftJump;
                    m_isLastStepInRow = true;
                }
                else
                {
                    m_currTopLeftX += m_Direction * (m_EnemiesMatrix[0, 0].Texture.Width / 2);
                }
        }

        private float getGroupWidth()
        {
            return getRightGroupBorder() - getLeftGroupBorder();
        }


        private bool isEnemiesGroupCollidedTheGameBorder()
        {
            bool isCollided = getRightGroupBorder() >= GraphicsDevice.Viewport.Width || getLeftGroupBorder() < 0f;

            return isCollided;
        }

        private void JumpDown()
        {
            m_currTopLeftY += m_EnemiesMatrix[0, 0].Texture.Width / 2;
            m_TimeUntilNextStepInSec -=  ( m_TimeUntilNextStepInSec * 0.08f);
        }
    }
}
