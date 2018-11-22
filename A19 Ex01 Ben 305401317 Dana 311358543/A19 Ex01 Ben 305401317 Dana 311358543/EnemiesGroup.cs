using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        private bool m_IsLastStepInRow = false;
        private float m_TimeCounter = 0f;
        private float m_TimeUntilNextStepInSec = 0.5f;
        private Enemy[,] m_EnemiesMatrix;

        public EnemiesGroup(Game i_Game) : base(i_Game)
        {
            this.m_EnemiesMatrix = new Enemy[k_EnemiesRows, k_EnemiesColumns];
        }

        public override void Initialize()
        {
            this.initEnemyGroup();
            base.Initialize();
        }

        private void initEnemyGroup()
        {
            this.initEnemiesRow(0, @"Sprites\Enemy0101_32x32", Color.Pink);
            for (int i = 1; i < k_EnemiesRows; i++)
            {
                if (i < 3)
                {
                    this.initEnemiesRow(i, @"Sprites\Enemy0201_32x32", Color.LightBlue);
                }
                else
                {
                    this.initEnemiesRow(i, @"Sprites\Enemy0301_32x32", Color.LightYellow);
                }
            }

            this.m_currTopLeftX = 0;
            this.m_currTopLeftY = k_enemyHeight * 3f;
            this.updatePositions(this.m_currTopLeftX, this.m_currTopLeftY);
        }

        private void initEnemiesRow(int i_Row, string i_AssetName, Color i_Tint)
        {
            for (int colum = 0; colum < k_EnemiesColumns; colum++)
            {
                this.m_EnemiesMatrix[i_Row, colum] = new Enemy(Game, i_Tint, i_AssetName);
                this.m_EnemiesMatrix[i_Row, colum].VisibleChanged += this.countDeadEnemies;
                this.m_EnemiesMatrix[i_Row, colum].AddComponent();
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            this.m_TimeCounter += (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            
            if (this.m_IncreaseVelocityWhen4Dead)
            {
                this.m_IncreaseVelocityWhen4Dead = false;
                this.increaseVelocity(0.04f);
            }

            if (this.m_TimeCounter >= this.m_TimeUntilNextStepInSec)
            {
                this.m_TimeCounter -= this.m_TimeUntilNextStepInSec;

                if (this.m_IsLastStepInRow)
                {
                    this.m_IsLastStepInRow = false;
                    this.jumpDown();
                    this.m_Direction *= -1f;
                }
                else
                {
                    this.jumpHorizontalStep(i_GameTime);
                }
            }

            this.updatePositions(this.m_currTopLeftX, this.m_currTopLeftY);

            if (this.isEnemiesGroupTouchTheBotton() || this.isAllEnemiesDead())
            {
                SpaceInvaders.s_GameUtils.InputManager.ShowGameOverMessage();
                Game.Exit();
            }
        }

        private void updatePositions(float i_X, float i_Y)
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

            if (this.m_NumOfDeadEnemies == k_EnemiesColumns * k_EnemiesRows)
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

        private float getLeftGroupBorder()  
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

        private float getRightGroupBorder()
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

        private float getBottomGroupBorder()
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

        private void countDeadEnemies(object sender, EventArgs args)
        {
            this.m_NumOfDeadEnemies++;
            if(this.m_NumOfDeadEnemies % 4 == 0 && this.m_NumOfDeadEnemies != 0)
            {
                this.m_IncreaseVelocityWhen4Dead = true;
            }
        }

        private void jumpHorizontalStep(GameTime i_GameTime)
        {
                float lastRightJump = GraphicsDevice.Viewport.Width - this.getRightGroupBorder();
                float lastLeftJump = this.getLeftGroupBorder();
                
                if (lastRightJump < (this.m_EnemiesMatrix[0, 0].Texture.Width / 2) && lastRightJump > 0)
                {
                    this.m_currTopLeftX += this.m_Direction * lastRightJump;
                    this.m_IsLastStepInRow = true;
                }
                else if (lastLeftJump < (this.m_EnemiesMatrix[0, 0].Texture.Width / 2) && lastLeftJump > 0 && this.m_Direction == -1f )
                {
                    this.m_currTopLeftX += this.m_Direction * lastLeftJump;
                    this.m_IsLastStepInRow = true;
                }
                else
                {
                    this.m_currTopLeftX += this.m_Direction * (this.m_EnemiesMatrix[0, 0].Texture.Width / 2);
                }
        }
        
        private void jumpDown()
        {
            this.m_currTopLeftY += this.m_EnemiesMatrix[0, 0].Texture.Width / 2;
            this.increaseVelocity(0.08f);
        }

        private void increaseVelocity(float i_TimeToIncrease)
        {
            this.m_TimeUntilNextStepInSec -= this.m_TimeUntilNextStepInSec * i_TimeToIncrease;
        }
    }
}
