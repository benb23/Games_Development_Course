using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    class EnemiesGroup : GameComponent 
    {
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
        private IGameEngine m_GameEngine;

        public EnemiesGroup(Game i_Game) : base(i_Game)
        {
            this.m_EnemiesMatrix = new Enemy[k_EnemiesRows, k_EnemiesColumns];
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
                if (m_GameEngine == null)
                {
                    m_GameEngine = Game.Services.GetService(typeof(IGameEngine)) as IGameEngine;
                }
                m_GameEngine.ShowGameOverMessage();
                Game.Exit();
            }
        }

        public override void Initialize()
        {
            this.initEnemyGroup();

            // initilize enemies positions
            this.updatePositions(this.m_currTopLeftX, this.m_currTopLeftY);

            base.Initialize();
        }

        private void initEnemyGroup()
        {
            for (int i = 0; i < k_EnemiesRows ; i++)
            {
                switch (i)
                {
                    case 0:
                        this.initEnemiesRow(0, 0, Color.Pink);
                        break;
                    case 1:
                        this.initEnemiesRow(1, 2, Color.LightBlue);
                        break;
                    case 2:
                        this.initEnemiesRow(2, 3, Color.LightBlue);
                        break;
                    case 3:
                        this.initEnemiesRow(3, 4, Color.LightYellow);
                        break;
                    case 4:
                        this.initEnemiesRow(4, 5, Color.LightYellow);
                        break;
                }
            }

            

            // For calculating positions according to enemy texture width (generic)
            this.m_EnemiesMatrix[0, 0].LoadAsset();

            m_currTopLeft = new Vector2(this.m_EnemiesMatrix[0, 0].Texture.Height / 2, this.m_EnemiesMatrix[0, 0].Texture.Height * 3f);

            this.m_currTopLeftX = this.m_EnemiesMatrix[0, 0].Texture.Height/2;
            this.m_currTopLeftY = this.m_EnemiesMatrix[0, 0].Texture.Height * 3f - m_currTopLeftX;
            // Vector2 x = m_EnemiesMatrix[0, 0].TopLeftPosition;
        }

        private Vector2 m_currTopLeft;

        private void initEnemiesRow(int i_Row, int i_StartSqureIndex, Color i_Tint)
        {
            for (int colum = 0; colum < k_EnemiesColumns; colum++)
            {
                this.m_EnemiesMatrix[i_Row, colum] = new Enemy(Game, i_Tint, i_StartSqureIndex);
                this.m_EnemiesMatrix[i_Row, colum].VisibleChanged += this.countDeadEnemies;
            }
        }

        private void updatePositions(float i_X, float i_Y)
        {
            float enemiesGap = this.m_EnemiesMatrix[0, 0].Texture.Height * 0.6f;
            float startX = i_X;
            float strartY = i_Y;

            for (int i = 0; i < k_EnemiesRows; i++)
            {
                for (int j = 0; j < k_EnemiesColumns; j++)
                {
                    this.m_EnemiesMatrix[i, j].Position = new Vector2(startX + 5 , strartY);
                    this.m_EnemiesMatrix[i, j].PositionOrigin = new Vector2(16,16);
                    startX += this.m_EnemiesMatrix[0, 0].Texture.Height + enemiesGap;
                }

                startX = i_X;
                strartY += this.m_EnemiesMatrix[0, 0].Texture.Height + enemiesGap;
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

            if (this.getBottomGroupBorder() >= Game.GraphicsDevice.Viewport.Height)
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
                        leftX = this.m_EnemiesMatrix[row, col].Position.X - this.m_EnemiesMatrix[row, col].Texture.Width/2;
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
                        rightBorderX = this.m_EnemiesMatrix[row, col].Position.X + this.m_EnemiesMatrix[row, col].Texture.Width/2;
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
                        bottomBorderY = this.m_EnemiesMatrix[row, col].Position.Y + this.m_EnemiesMatrix[row, col].Texture.Height/2;
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
            if (this.m_NumOfDeadEnemies % 4 == 0 && this.m_NumOfDeadEnemies != 0)
            {
                this.m_IncreaseVelocityWhen4Dead = true;
            }
        }

        private void jumpHorizontalStep(GameTime i_GameTime)
        {
            float lastRightJump = Game.GraphicsDevice.Viewport.Width - this.getRightGroupBorder();
            float lastLeftJump = this.getLeftGroupBorder();

            if (lastRightJump < (this.m_EnemiesMatrix[0, 0].Texture.Width / 2) && lastRightJump > 0)
            {
                this.m_currTopLeftX += this.m_Direction * lastRightJump;
                this.m_IsLastStepInRow = true;
            }
            else if (lastLeftJump < (this.m_EnemiesMatrix[0, 0].Texture.Width / 2) && lastLeftJump > 0 && this.m_Direction == -1f)
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
