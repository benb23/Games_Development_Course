using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    class EnemiesGroup : GameComponent
    {
        public enum eDirection
        {
            left = -1,
            right = 1,
        }

        public event EventHandler<EventArgs> AllEnemiesDied;

        private const int k_EnemiesRows = 5;
        private const int k_EnemiesColumns = 9;
        private int m_CurrentColumns = 9;
        private float m_Direction = 1f;
        private bool m_IncreaseVelocityWhen4Dead = false;
        private bool m_IsLastStepInRow = false;
        private float m_TimeCounter = 0f;
        private float m_TimeUntilNextStepInSec = 0.5f;
        private float m_EnemiesGap;
        //private Enemy[,] m_EnemiesMatrix;
        private List<List<Enemy>> m_EnemiesGroup = new List<List<Enemy>>(k_EnemiesRows);
        //private List<List<Enemy>> m_ExtraEnemiesGroup = new List<List<Enemy>>(k_EnemiesRows);
        private List<Enemy> m_AliveEnemiesByColum = new List<Enemy>(k_EnemiesRows * k_EnemiesColumns);
        private List<Enemy> m_AliveEnemiesByRow = new List<Enemy>(k_EnemiesRows * k_EnemiesColumns);
        private ISpaceInvadersEngine m_GameEngine;
        private GameScreen m_GameScreen;

        public EnemiesGroup(GameScreen i_GameScreen) : base(i_GameScreen.Game)
        {
            m_GameScreen = i_GameScreen;
            //this.m_EnemiesGroup = 
            //this.m_EnemiesMatrix = new Enemy[k_EnemiesRows, k_EnemiesColumns];
            //m_CurrentEnemiesColumns = k_EnemiesColumns;
            i_GameScreen.Add(this);
        }

        public void InitEnemyGroupForNextLevel()
        {
            m_AliveEnemiesByRow.Clear();
            m_AliveEnemiesByColum.Clear();

            this.m_Direction = 1f;


            if (SpaceInvadersConfig.m_Level % 7 != 0)//todo : not consistent
            {
                addColumToEnemiesGroup();
                m_CurrentColumns++;
            }
            else
            {
                ////revertEnemiesGroupToOriginalSize();
                m_CurrentColumns = k_EnemiesColumns;
            }
            


            foreach (List<Enemy> list in m_EnemiesGroup)
            {
                foreach (Enemy enemy in list)
                {
                    if (enemy.Colum < m_CurrentColumns)
                    {
                        enemy.Animations["CellAnimation"].Reset();
                        enemy.Animations["CellAnimation"].Pause();
                        updateScoreValueAndShootingFrequency(enemy);
                        enemy.Enabled = true;
                        enemy.Visible = true;
                        enemy.initPosition();                    
                        m_AliveEnemiesByRow.Add(enemy);
                        enemy.Animations["CellAnimation"].Reset();
                    }
                }
            }

            m_TimeCounter = 0f;
            initAliveEnemiesByColum();
        }

        private void revertEnemiesGroupToOriginalSize()
        {
            for (int colum = m_CurrentColumns; colum > k_EnemiesColumns; colum--)
            {
                removeColumToEnemyMantrix();
            }
        }

        private void AddOrRemoveEnemiesGroupColum()
        {
            for (int row = 0; row < k_EnemiesRows; row++)
            {
                m_EnemiesGroup[row][m_CurrentColumns].Visible = !m_EnemiesGroup[row][m_CurrentColumns].Visible;
                m_EnemiesGroup[row][m_CurrentColumns].Enabled = !m_EnemiesGroup[row][m_CurrentColumns].Enabled;
            }
        }

        private void addColumToEnemiesGroup()
        {
            AddOrRemoveEnemiesGroupColum();
        }

        private void removeColumToEnemyMantrix()
        {
            AddOrRemoveEnemiesGroupColum();
        }

        public override void Initialize()
        {
            if (m_GameEngine == null)
            {
                m_GameEngine = Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
            }



            for (int i = 0; i < k_EnemiesRows; i++)
            {
                m_EnemiesGroup.Add(new List<Enemy>(k_EnemiesColumns));
            }

            this.initEnemyGroup();
            this.initAliveEnemiesByColum();
            base.Initialize();
        }

        private void initEnemyGroup()
        {
            Enemy newEnemy;

            for (int row = 0; row < k_EnemiesRows; row++)
            {
                for (int colum = 0; colum < k_EnemiesColumns + 5; colum++)
                {
                    newEnemy = this.initEnemyByRow(row, colum);

                    m_EnemiesGroup[row].Add(newEnemy);
                    if (colum < k_EnemiesColumns)
                    {
                        m_AliveEnemiesByRow.Add(newEnemy);
                    }
                    else
                    {
                        newEnemy.Visible = false;
                        newEnemy.Enabled = false;
                    }
                }
            }

            /// For calculating positions according to enemy texture width (generic)
            this.m_EnemiesGroup[0][0].LoadAsset();
            m_EnemiesGap = this.m_EnemiesGroup[0][0].Texture.Height * 0.6f;
        }
  
        private Enemy initEnemyByRow(int i_Row, int i_Colum)
        {
            Enemy retEnemy = null;

            switch (i_Row)
            {
                case 0:
                    retEnemy = this.initEnemyByRowHelper(0, i_Colum, 0, Color.Pink, 1, SpaceInvadersConfig.eScoreValue.PinkEnemy);
                    break;
                case 1:
                    retEnemy = this.initEnemyByRowHelper(1, i_Colum, 2, Color.LightBlue, 1, SpaceInvadersConfig.eScoreValue.BlueEnemy);
                    break;
                case 2:
                    retEnemy = this.initEnemyByRowHelper(2, i_Colum, 3, Color.LightBlue, -1, SpaceInvadersConfig.eScoreValue.BlueEnemy);
                    break;
                case 3:
                    retEnemy = this.initEnemyByRowHelper(3, i_Colum, 4, Color.LightYellow, 1, SpaceInvadersConfig.eScoreValue.YellowEnemy);
                    break;
                case 4:
                    retEnemy = this.initEnemyByRowHelper(4, i_Colum, 5, Color.LightYellow, -1, SpaceInvadersConfig.eScoreValue.YellowEnemy);
                    break;
            }

            return retEnemy;
        }

        private Enemy initEnemyByRowHelper(int i_Row, int i_Colum, int i_StartSqureIndex, Color i_Tint, int i_Toggeler, SpaceInvadersConfig.eScoreValue i_ScoreValue)
        {
            Enemy retEnemy = new Enemy(m_GameScreen, i_Tint, (int)i_ScoreValue, i_StartSqureIndex, i_Row, i_Colum, m_EnemiesGap, m_TimeUntilNextStepInSec);
            retEnemy.m_Toggeler = i_Toggeler;
            retEnemy.VisibleChanged += this.updateAliveLists;
            retEnemy.VisibleChanged += this.isFourEnemiesDead;

            return retEnemy;
        }

        private void initAliveEnemiesByColum()
        {
            for (int colomn = 0; colomn < m_CurrentColumns; colomn++)
            {
                for (int row = 0; row < k_EnemiesRows; row++)
                {
                    m_AliveEnemiesByColum.Add(m_EnemiesGroup[row][colomn]);
                }
            } 
        }

        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        private void updateScoreValueAndShootingFrequency(Enemy i_Enemy)
        {
            if (SpaceInvadersConfig.m_LogicLevel == SpaceInvadersConfig.eLevel.One)
            {
                i_Enemy.m_MaxRandomToShoot = i_Enemy.m_OriginalMaxRandomToShoot;
                i_Enemy.ScoreValue = i_Enemy.OriginalScoreValue;
            }
            else
            {
                i_Enemy.m_MaxRandomToShoot += SpaceInvadersConfig.k_EnemyShootingFrequencyAddition;
                i_Enemy.ScoreValue += SpaceInvadersConfig.k_EnemyScoreAddition;
            }
        }

        private void jump(Vector2 i_StepToJump)
        {
            foreach (Enemy enemy in m_AliveEnemiesByRow)
            {
                enemy.Position += i_StepToJump;
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            if (this.isEnemiesGroupTouchTheBotton())
            {
                m_GameEngine.IsGameOver = true;
            }
            if (this.isAllEnemiesDead())
            {
                OnAllEnemiesDied(this, EventArgs.Empty);
            }

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
        }

        protected virtual void OnAllEnemiesDied(object sender, EventArgs args)
        {
            if (AllEnemiesDied != null)
            {
                AllEnemiesDied.Invoke(sender, args);
            }
        }

        private bool isAllEnemiesDead()
        {
            bool isAllDead = m_AliveEnemiesByRow.Count == 0;
            return isAllDead;
        }

        private bool isEnemiesGroupTouchTheBotton()
        {
            bool isEnemiesGroupTouchTheBotton = false;

            if (m_AliveEnemiesByRow.Count > 0)
            {
                isEnemiesGroupTouchTheBotton = this.getBottomGroupBorder() >= Game.GraphicsDevice.Viewport.Height;
            }
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

        private void jumpHorizontalStep(GameTime i_GameTime)
        {
            float lastRightJump = Game.GraphicsDevice.Viewport.Width - this.getRightGroupBorder();
            float lastLeftJump = this.getLeftGroupBorder();

            if (isLastStep(lastRightJump, eDirection.right))
            {
                this.m_IsLastStepInRow = true;
                jump(new Vector2(m_Direction * lastRightJump, 0));
            }
            else if (isLastStep(lastLeftJump, eDirection.left))
            {
                this.m_IsLastStepInRow = true;
                jump(new Vector2(m_Direction * lastLeftJump, 0));
            }
            else
            {
                jump(new Vector2(m_Direction * this.m_EnemiesGroup[0][0].Texture.Height / 2, 0));
            }
        }

        private bool isLastStep(float i_LastStep, eDirection i_MoveDirection)
        {
            bool isLastStep = false;

            if (this.m_Direction ==(float)i_MoveDirection)
            {
                isLastStep = i_LastStep < (this.m_EnemiesGroup[0][0].Texture.Height) && i_LastStep > 0;
            }

            return isLastStep;
        }

        private void jumpDown()
        {
            jump(new Vector2(0, this.m_EnemiesGroup[0][0].Texture.Height / 2));
            this.increaseVelocity(0.08f);
        }

        private void increaseVelocity(float i_TimeToIncrease)
        {
            this.m_TimeUntilNextStepInSec -= this.m_TimeUntilNextStepInSec * i_TimeToIncrease;
        }

    }
}
