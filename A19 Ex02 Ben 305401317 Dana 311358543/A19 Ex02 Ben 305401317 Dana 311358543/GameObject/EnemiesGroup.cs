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
        private GameScreen m_GameScreen;

        public EnemiesGroup(GameScreen i_GameScreen) : base(i_GameScreen.Game)
        {
            m_GameScreen = i_GameScreen;
            this.m_EnemiesMatrix = new Enemy[k_EnemiesRows, k_EnemiesColumns];
            i_GameScreen.Add(this);
        }

        protected virtual void OnAllEnemiesDied(object sender, EventArgs args)
        {
            if (AllEnemiesDied != null)
            {
                AllEnemiesDied.Invoke(sender, args);
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

        public void InitEnemyGroupForNextLevel()
        {
            m_AliveEnemiesByRow.Clear();
            m_AliveEnemiesByColum.Clear();

            this.m_Direction = 1f;
            foreach(Enemy enemy in m_EnemiesMatrix)
            {
                updateScoreValueAndShootingFrequency(enemy);
                enemy.Enabled = true;
                enemy.Visible = true;
                enemy.initPosition();
                m_AliveEnemiesByRow.Add(enemy);
                enemy.Animations["CellAnimation"].Reset(); 
            }
            m_TimeUntilNextStepInSec = 0.5f;
            m_TimeCounter = 0f;
            initAliveEnemiesByColum();
        }

        private void updateScoreValueAndShootingFrequency(Enemy i_Enemy)
        {
            if (m_GameEngine == null)
            {
                m_GameEngine = Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
            }

            if (m_GameEngine.Level == SpaceInvadersEngine.eLevel.One)
            {
                i_Enemy.m_MaxRandomToShoot = i_Enemy.m_OriginalMaxRandomToShoot;
                i_Enemy.ScoreValue = i_Enemy.OriginalScoreValue;
            }
            else
            {
                i_Enemy.m_MaxRandomToShoot += m_GameEngine.EnemyShootingFrequencyAddition;
                i_Enemy.ScoreValue += m_GameEngine.EnemyScoreAddition;
            }
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
                        this.initEnemiesRow(0, 0, Color.Pink, 1, SpaceInvadersEngine.eScoreValue.PinkEnemy);
                        break;
                    case 1:
                        this.initEnemiesRow(1, 2, Color.LightBlue, 1, SpaceInvadersEngine.eScoreValue.BlueEnemy);
                        break;
                    case 2:
                        this.initEnemiesRow(2, 3, Color.LightBlue, -1, SpaceInvadersEngine.eScoreValue.BlueEnemy);
                        break;
                    case 3:
                        this.initEnemiesRow(3, 4, Color.LightYellow, 1, SpaceInvadersEngine.eScoreValue.YellowEnemy);
                        break;
                    case 4:
                        this.initEnemiesRow(4, 5, Color.LightYellow, -1, SpaceInvadersEngine.eScoreValue.YellowEnemy);
                        break;
                }
            }

            /// For calculating positions according to enemy texture width (generic)
            this.m_EnemiesMatrix[0, 0].LoadAsset();
            m_EnemiesGap = this.m_EnemiesMatrix[0, 0].Texture.Height * 0.6f;
        }
  
        private void initEnemiesRow(int i_Row, int i_StartSqureIndex, Color i_Tint, int i_Toggeler, SpaceInvadersEngine.eScoreValue i_ScoreValue)
        {
            for (int colum = 0; colum < k_EnemiesColumns; colum++)
            {
                this.m_EnemiesMatrix[i_Row, colum] = new Enemy(m_GameScreen, i_Tint, (int)i_ScoreValue, i_StartSqureIndex, i_Row, colum, m_EnemiesGap, m_TimeUntilNextStepInSec);
                m_AliveEnemiesByRow.Add(m_EnemiesMatrix[i_Row, colum]);
                m_EnemiesMatrix[i_Row, colum].m_Toggeler = i_Toggeler;
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
                    m_AliveEnemiesByColum.Add(m_EnemiesMatrix[row, colomn]);
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
                jump(new Vector2(m_Direction * this.m_EnemiesMatrix[0, 0].Texture.Height / 2, 0));
            }
        }

        private bool isLastStep(float i_LastStep, eDirection i_MoveDirection)
        {
            bool isLastStep = false;

            if (this.m_Direction ==(float)i_MoveDirection)
            {
                isLastStep = i_LastStep < (this.m_EnemiesMatrix[0, 0].Texture.Height) && i_LastStep > 0;
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
