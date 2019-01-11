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
    public class EnemiesGroup : RegisteredComponent
    {
        public enum eDirection
        {
            left = -1,
            right = 1,
        }

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

            this.cheackGameOver();
        }

        private bool cheackGameOver()
        {
            bool isGameOver = this.isEnemiesGroupTouchTheBotton() || this.isAllEnemiesDead();
            if (isGameOver)
            {
                if (this.m_GameEngine == null)
                {
                    this.m_GameEngine = Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
                }

                this.m_GameEngine.ShowGameOverMessage();
                this.Game.Exit();
            }

            return isGameOver;
        }

        private void jump(Vector2 i_StepToJump)
        {
            foreach (Enemy enemy in this.m_AliveEnemiesByRow)
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
            this.m_EnemiesGap = this.m_EnemiesMatrix[0, 0].Texture.Height * 0.6f;
        }
  
        private void initEnemiesRow(int i_Row, int i_StartSqureIndex, Color i_Tint, int i_Toggeler)
        {
            for (int colum = 0; colum < k_EnemiesColumns; colum++)
            {
                this.m_EnemiesMatrix[i_Row, colum] = new Enemy(Game, i_Tint, i_StartSqureIndex, i_Row, colum, this.m_EnemiesGap, this.m_TimeUntilNextStepInSec);
                this.m_AliveEnemiesByRow.Add(this.m_EnemiesMatrix[i_Row, colum]);
                this.m_EnemiesMatrix[i_Row, colum].m_Toggeler = i_Toggeler;
                this.m_EnemiesMatrix[i_Row, colum].VisibleChanged += this.updateAliveLists;
                this.m_EnemiesMatrix[i_Row, colum].VisibleChanged += this.isFourEnemiesDead;
            }
        }

        private void initAliveEnemiesByColum()
        {
            for (int colomn = 0; colomn < k_EnemiesColumns; colomn++)
            {
                for (int row = 0; row < k_EnemiesRows; row++)
                {
                    this.m_AliveEnemiesByColum.Add(this.m_EnemiesMatrix[row, colomn]);
                }
            } 
        }

        private bool isAllEnemiesDead()
        {
            bool isAllDead = this.m_AliveEnemiesByRow.Count == 0;

            return isAllDead;
        }

        private bool isEnemiesGroupTouchTheBotton()
        {
            bool isEnemiesGroupTouchTheBotton = this.getBottomGroupBorder() >= this.Game.GraphicsDevice.Viewport.Height;

            return isEnemiesGroupTouchTheBotton;
        }

        private float getRightGroupBorder()
        {
            return this.m_AliveEnemiesByColum.Last().Position.X + (this.m_AliveEnemiesByColum.Last().HeightBeforeScale / 2);
        }

        private float getLeftGroupBorder()
        {
            return this.m_AliveEnemiesByColum.First().Position.X - (this.m_AliveEnemiesByColum.First().HeightBeforeScale / 2);
        }

        private float getBottomGroupBorder()
        {
            return this.m_AliveEnemiesByRow.Last().Position.Y + (this.m_AliveEnemiesByRow.Last().HeightBeforeScale / 2);
        }

        private void updateAliveLists(object sender, EventArgs args)
        {
            this.m_AliveEnemiesByRow.Remove(sender as Enemy);
            this.m_AliveEnemiesByColum.Remove(sender as Enemy);
        }

        private void isFourEnemiesDead(object sender, EventArgs args)
        {
            int numOfDeadEnemies = this.m_AliveEnemiesByRow.Capacity - this.m_AliveEnemiesByRow.Count;
            this.m_IncreaseVelocityWhen4Dead = numOfDeadEnemies % 4 == 0 && numOfDeadEnemies != 0;
        }

        private void jumpHorizontalStep(GameTime i_GameTime)
        {
            float lastRightJump = this.Game.GraphicsDevice.Viewport.Width - this.getRightGroupBorder();
            float lastLeftJump = this.getLeftGroupBorder();

            if (this.isLastStep(lastRightJump, eDirection.right))
            {
                this.m_IsLastStepInRow = true;
                this.jump(new Vector2(this.m_Direction * lastRightJump, 0));
            }
            else if (this.isLastStep(lastLeftJump, eDirection.left))
            {
                this.m_IsLastStepInRow = true;
                this.jump(new Vector2(this.m_Direction * lastLeftJump, 0));
            }
            else
            {
                this.jump(new Vector2(this.m_Direction * this.m_AliveEnemiesByRow.First().Texture.Height / 2, 0));
            }
        }

        private bool isLastStep(float i_LastStep, eDirection i_MoveDirection)
        {
            bool isLastStep = false;

            if (this.m_Direction == (float)i_MoveDirection)
            {
                isLastStep = i_LastStep < this.m_AliveEnemiesByRow.First().Texture.Height && i_LastStep > 0;
            }

            return isLastStep;
        }

        private void jumpDown()
        {
            this.jump(new Vector2(0, this.m_AliveEnemiesByRow.First().Texture.Height / 2));
            this.increaseVelocity(0.08f);
        }

        private void increaseVelocity(float i_TimeToIncrease)
        {
            this.m_TimeUntilNextStepInSec -= this.m_TimeUntilNextStepInSec * i_TimeToIncrease;
        }
    }
}
