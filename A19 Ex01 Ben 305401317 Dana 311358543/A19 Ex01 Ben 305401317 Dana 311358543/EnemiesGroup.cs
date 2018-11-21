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
    public class EnemiesGroup : DrawableGameComponent
    {
        private const float k_enemyHeight = 32f;
        private const int k_EnemiesRows = 5;
        private const int k_EnemiesColumns = 9;
        private float m_Direction = 1f;
        private float m_currTopLeftX;
        private float m_currTopLeftY;
        private int m_NumOfDeadEnemies = 0;
        private bool m_IncreaseVelocityWhen4Dead = false;
        private Enemy[,] m_EnemiesMatrix = new Enemy[k_EnemiesRows, k_EnemiesColumns];

        private bool m_isLastStepInRow = false;
        private float m_TimeCounter = 0f;
        private float m_TimeUntilNextStepInSec = 0.5f;

        public EnemiesGroup(Game i_Game) : base(i_Game)
        {
        }

        public override void Initialize()
        {
            this.InitEnemyGroup();
            base.Initialize();
        }

        private void InitEnemyGroup()
        {
            // TODO: initilize the EnemiesGroup matrix
            this.InitEnemiesRow(0, @"Sprites\Enemy0101_32x32", Color.Pink);
            for (int i = 1; i < k_EnemiesRows; i++)
            {
                if (i < 3)
                {
                    this.InitEnemiesRow(i, @"Sprites\Enemy0201_32x32", Color.LightBlue);
                }
                else
                {
                    this.InitEnemiesRow(i, @"Sprites\Enemy0301_32x32", Color.LightYellow);
                }
            }

            this.m_currTopLeftX = 0;
            this.m_currTopLeftY = k_enemyHeight * 3f;
            updatePositions(m_currTopLeftX, m_currTopLeftY);
        }

        private void InitEnemiesRow(int i_Row, string i_AssetName, Color i_Tint)
        {
            for (int colum = 0; colum < k_EnemiesColumns; colum++)
            {
                this.m_EnemiesMatrix[i_Row, colum] = new Enemy(Game, i_Tint, i_AssetName);
                this.m_EnemiesMatrix[i_Row, colum].VisibleChanged += CountDeadEnemies;
                this.m_EnemiesMatrix[i_Row, colum].AddComponent();
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            this.m_TimeCounter += (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            

            if (m_IncreaseVelocityWhen4Dead)
            {
                m_IncreaseVelocityWhen4Dead = false;
                increaseVelocity(0.04f);
            }


            if (this.m_TimeCounter >= this.m_TimeUntilNextStepInSec)
            {
                this.m_TimeCounter -= this.m_TimeUntilNextStepInSec;

                if (this.m_isLastStepInRow)
                {
                    this.m_isLastStepInRow = false;
                    this.JumpDown();
                    this.m_Direction *= -1f;
                }
                else
                {
                    this.JumpHorizontalStep(i_GameTime);
                }
            }

            this.updatePositions(this.m_currTopLeftX, this.m_currTopLeftY);

            if (this.isEnemiesGroupTouchTheBotton() || this.isAllEnemiesDead())
            {
                SpaceInvaders.m_GameUtils.InputManager.showGameOverMessage();
                Game.Exit();
            }
        }

        public void updatePositions(float i_X, float i_Y)
        {
            float enemiesGap = k_enemyHeight * 0.6f;
            float startX = i_X;
            float strartY = i_Y;

            for (int i = 0; i < k_EnemiesRows; i++)
            {
                for (int j = 0; j < k_EnemiesColumns; j++)
                {
                    this.m_EnemiesMatrix[i, j].Position = new Vector2(startX, strartY);
                    startX += k_enemyHeight + enemiesGap;
                }

                startX = i_X;
                strartY += k_enemyHeight + enemiesGap;
            }
        }

        private bool isAllEnemiesDead()
        {
            bool isAllDead = false;

            if (m_NumOfDeadEnemies == k_EnemiesColumns * k_EnemiesRows)
            {
                isAllDead = true;
            }

            return isAllDead;
        }

        private bool isEnemiesGroupTouchTheBotton()
        {
            bool isEnemiesGroupTouchTheBotton;

            if (this.getBottomGroupBorder() >= GraphicsDevice.Viewport.Height)
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

            for (int col = 0; col < k_EnemiesColumns; col++)
            {
                for (int row = 0; row < k_EnemiesRows; row++)
                {
                    if (this.m_EnemiesMatrix[row, col].Visible)
                    {
                        leftX = this.m_EnemiesMatrix[row, col].Position.X;
                        isFound = true;
                        break;
                    }
                }
                if (isFound)
                { 
                    break;
                }
            }

            return leftX;
        }

        public float getRightGroupBorder()
        {
            float rightBorderX = 0;
            bool isFound = false;

            for (int col = k_EnemiesColumns - 1; col >= 0; col--)  
            {
                for (int row = 0; row < k_EnemiesRows; row++)   
                {
                    if (this.m_EnemiesMatrix[row, col].Visible)
                    {
                        rightBorderX = this.m_EnemiesMatrix[row, col].Position.X + this.m_EnemiesMatrix[row, col].Texture.Width;
                        isFound = true;
                        break;
                    }
                }
                if (isFound)
                { 
                    break;
                }
            }

            return rightBorderX;
        }

        public float getBottomGroupBorder()
        {
            float bottomBorderY = 0;
            bool isFound = false;

            for (int row = k_EnemiesRows - 1; row >= 0; row--)
            {
                for (int col = 0; col < k_EnemiesColumns; col++)
                {
                    if (this.m_EnemiesMatrix[row, col].Visible)
                    {
                        bottomBorderY = this.m_EnemiesMatrix[row, col].Position.Y + this.m_EnemiesMatrix[row, col].Texture.Height;
                        isFound = true;
                        break;
                    }
                }
                if (isFound)
                { 
                    break;
                }
            }
            return bottomBorderY;
        }

        private void CountDeadEnemies(object sender, EventArgs args)
        {
            m_NumOfDeadEnemies++;
            if(m_NumOfDeadEnemies%4 == 0 && m_NumOfDeadEnemies != 0)
            {
                m_IncreaseVelocityWhen4Dead = true;
            }
        }

        private void JumpHorizontalStep(GameTime i_GameTime)
        {
                float lastRightJump = GraphicsDevice.Viewport.Width - this.getRightGroupBorder();
                float lastLeftJump = this.getLeftGroupBorder();

                
                if (lastRightJump < (this.m_EnemiesMatrix[0, 0].Texture.Width / 2) && lastRightJump > 0)
                {
                    this.m_currTopLeftX += this.m_Direction * lastRightJump;
                    this.m_isLastStepInRow = true;
                }
                else if (lastLeftJump < (this.m_EnemiesMatrix[0, 0].Texture.Width / 2) && lastLeftJump > 0 && this.m_Direction == -1f )
                {
                    this.m_currTopLeftX += this.m_Direction * lastLeftJump;
                    this.m_isLastStepInRow = true;
                }
                else
                {
                    this.m_currTopLeftX += this.m_Direction * (this.m_EnemiesMatrix[0, 0].Texture.Width / 2);
                }
        }
        
        private void JumpDown()
        {
            this.m_currTopLeftY += this.m_EnemiesMatrix[0, 0].Texture.Width / 2;
            this.increaseVelocity(0.08f);
        }

        private void increaseVelocity(float i_timeToIncrease)
        {
            this.m_TimeUntilNextStepInSec -= this.m_TimeUntilNextStepInSec * i_timeToIncrease;
        }
    }
}
