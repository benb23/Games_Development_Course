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

using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    class EnemiesGroup : RegisteredComponent
    {
        private const int k_EnemiesRows = 5;
        private const int k_EnemiesColumns = 9;
        private float m_Direction = 1f;
        private bool m_IncreaseVelocityWhen4Dead = false;
        private bool m_IsLastStepInRow = false;
        private float m_TimeCounter = 0f;
        private float m_TimeUntilNextStepInSec = 0.5f;
        private float m_EnemiesGap;
        private Enemy[,] m_EnemiesMatrix;
        private List<Enemy> m_AliveEnemiesByColum = new List<Enemy>(k_EnemiesRows * k_EnemiesColumns);
        private List<Enemy> m_AliveEnemiesByRow = new List<Enemy>(k_EnemiesRows * k_EnemiesColumns);
        private ISpaceInvadersEngine m_GameEngine;

        public EnemiesGroup(Game i_Game) : base(i_Game)
        {
            this.m_EnemiesMatrix = new Enemy[k_EnemiesRows, k_EnemiesColumns];
        }

        public override void Update(GameTime i_GameTime)
        {
            this.m_TimeCounter += (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            if (m_IncreaseVelocityWhen4Dead)
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

            cheackGameOver();
        }

        private bool cheackGameOver()
        {
            bool isGameOver = this.isEnemiesGroupTouchTheBotton() || this.isAllEnemiesDead();
            if (isGameOver)
            {
                if (m_GameEngine == null)
                {
                    m_GameEngine = Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
                }

                m_GameEngine.ShowGameOverMessage();
                Game.Exit();
            }

            return isGameOver;
        }

        private void jump(Vector2 i_StepToJump)
        {
            foreach (Enemy enemy in m_AliveEnemiesByRow)
            {
                enemy.Position += i_StepToJump;
            }
        }

        public override void Initialize()
        {
            this.initEnemyGroup();
            this.initAliveEnemiesByColum();
            base.Initialize();
        }

        private void initEnemyGroup()
        {
            for (int row = 0; row < k_EnemiesRows; row++)
            {
                switch (row)
                {
                    case 0:
                        this.initEnemiesRow(0, 0, Color.Pink, 1);
                        break;
                    case 1:
                        this.initEnemiesRow(1, 2, Color.LightBlue, 1);
                        break;
                    case 2:
                        this.initEnemiesRow(2, 3, Color.LightBlue, -1);
                        break;
                    case 3:
                        this.initEnemiesRow(3, 4, Color.LightYellow, 1);
                        break;
                    case 4:
                        this.initEnemiesRow(4, 5, Color.LightYellow, -1);
                        break;
                }
            }

            /// For calculating positions according to enemy texture width (generic)
            this.m_EnemiesMatrix[0, 0].LoadAsset();
            m_EnemiesGap = this.m_EnemiesMatrix[0, 0].Texture.Height * 0.6f;
        }
  
        private void initEnemiesRow(int i_Row, int i_StartSqureIndex, Color i_Tint, int i_Toggeler)
        {
            for (int colum = 0; colum < k_EnemiesColumns; colum++)
            {
                this.m_EnemiesMatrix[i_Row, colum] = new Enemy(Game, i_Tint, i_StartSqureIndex, i_Row, colum, m_EnemiesGap, m_TimeUntilNextStepInSec);
                m_AliveEnemiesByRow.Add(m_EnemiesMatrix[i_Row, colum]);
                m_EnemiesMatrix[i_Row, colum].m_Toggeler = i_Toggeler;
                this.m_EnemiesMatrix[i_Row, colum].VisibleChanged += this.updateAliveLists;
                this.m_EnemiesMatrix[i_Row, colum].VisibleChanged += this.isFourEnemiesDead;
            }
        }

        // TODO: change list initilize
        private void initAliveEnemiesByColum()
        {
            for (int colum = 0; colum < k_EnemiesColumns; colum++)
            {
                for (int row = 0; row < k_EnemiesRows; row++)
                {
                    m_AliveEnemiesByColum.Add(m_EnemiesMatrix[row, colum]);
                }
            } 
        }

        private bool isAllEnemiesDead()
        {
            bool isAllDead = m_AliveEnemiesByRow.Count == 0;

            return isAllDead;
        }

        private bool isEnemiesGroupTouchTheBotton()
        {
            bool isEnemiesGroupTouchTheBotton = this.getBottomGroupBorder() >= Game.GraphicsDevice.Viewport.Height;

            return isEnemiesGroupTouchTheBotton;
        }

        private float getRightGroupBorder()
        {
            return m_AliveEnemiesByColum.Last().Position.X + m_AliveEnemiesByColum.Last().HeightBeforeScale / 2;
        }

        private float getLeftGroupBorder()
        {
            return m_AliveEnemiesByColum.First().Position.X - m_AliveEnemiesByColum.First().HeightBeforeScale / 2;
        }

        private float getBottomGroupBorder()
        {
            return m_AliveEnemiesByRow.Last().Position.Y + (m_AliveEnemiesByRow.Last().HeightBeforeScale / 2);
        }

        private void updateAliveLists(object sender, EventArgs args)
        {
            m_AliveEnemiesByRow.Remove((sender as Enemy));
            m_AliveEnemiesByColum.Remove((sender as Enemy));
        }

        private void isFourEnemiesDead(object sender, EventArgs args)
        {
            int numOfDeadEnemies = m_AliveEnemiesByRow.Capacity - m_AliveEnemiesByRow.Count;
            m_IncreaseVelocityWhen4Dead = numOfDeadEnemies % 4 == 0 && numOfDeadEnemies != 0;
        }

        // TODO: code dup
        private void jumpHorizontalStep(GameTime i_GameTime)
        {
            float lastRightJump = Game.GraphicsDevice.Viewport.Width - this.getRightGroupBorder();
            float lastLeftJump = this.getLeftGroupBorder();

            if (isLastStepToRightWall(lastRightJump))
            {
                this.m_IsLastStepInRow = true;
                jump(new Vector2(m_Direction * lastRightJump, 0));
            }
            else if (isLastStepToLeftWall(lastLeftJump))
            {
                this.m_IsLastStepInRow = true;
                jump(new Vector2(m_Direction * lastLeftJump, 0));
            }
            else
            {
                jump(new Vector2(m_Direction * this.m_EnemiesMatrix[0, 0].Texture.Height / 2, 0));
            }
        }
        
        // TODO: code dup
        private bool isLastStepToRightWall(float lastRightJump)
        {
            bool isLastStep = false;

            if (m_Direction == 1)
            {
                isLastStep =  lastRightJump < (this.m_EnemiesMatrix[0, 0].Texture.Height) && lastRightJump > 0;
            }

            return isLastStep;
        }

        // TODO: code dup
        private bool isLastStepToLeftWall(float lastLeftJump)
        {
            bool isLastStep = false;

            if (this.m_Direction == -1f)
            {
                isLastStep = lastLeftJump < (this.m_EnemiesMatrix[0, 0].Texture.Height) && lastLeftJump > 0;
            }

            return isLastStep;
        }

        private void jumpDown()
        {
            jump(new Vector2(0, this.m_EnemiesMatrix[0, 0].Texture.Height / 2));
            this.increaseVelocity(0.08f);
        }

        private void increaseVelocity(float i_TimeToIncrease)
        {
            this.m_TimeUntilNextStepInSec -= this.m_TimeUntilNextStepInSec * i_TimeToIncrease;
        }
    }
}
