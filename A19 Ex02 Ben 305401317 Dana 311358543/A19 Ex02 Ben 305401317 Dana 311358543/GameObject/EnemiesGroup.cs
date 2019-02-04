using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Infrastructure;

namespace A19_Ex03_Ben_305401317_Dana_311358543
{
    public class EnemiesGroup : GameComponent
    {
        public enum eDirection
        {
            left = -1,
            right = 1,
        }

        public event EventHandler<EventArgs> AllEnemiesDied;

        private ISpaceInvadersEngine m_GameEngine;
        private int m_CurrentColumns = 9;
        private float m_Direction = 1f;
        private float m_TimeCounter = 0f;
        private float m_TimeUntilNextStepInSec = 0.5f;
        private float k_TimeUntilNextStepInSec = 0.5f;
        private bool m_IncreaseVelocityWhen4Dead = false;
        private bool m_IsLastStepInRow = false;
        private float m_EnemiesGap;
        private List<List<Enemy>> m_EnemiesGroup = new List<List<Enemy>>(SpaceInvadersConfig.k_NumOfEnemiesRows);
        private List<Enemy> m_AliveEnemiesByColum = new List<Enemy>(SpaceInvadersConfig.k_NumOfEnemiesRows * SpaceInvadersConfig.k_NumOfEnemiesColumns);
        private List<Enemy> m_AliveEnemiesByRow = new List<Enemy>(SpaceInvadersConfig.k_NumOfEnemiesRows * SpaceInvadersConfig.k_NumOfEnemiesColumns);
        private GameScreen m_GameScreen;

        public EnemiesGroup(GameScreen i_GameScreen) : base(i_GameScreen.Game)
        {
            this.m_GameScreen = i_GameScreen;
            i_GameScreen.Add(this);
        }

        public void InitEnemyGroupForNextLevel()
        {
            this.m_AliveEnemiesByRow.Clear();
            this.m_AliveEnemiesByColum.Clear();

            this.m_Direction = 1f;

            if (SpaceInvadersConfig.s_LogicLevel != SpaceInvadersConfig.eLevel.One)
            {
                this.addColumToEnemiesGroup();
                this.m_CurrentColumns++;
            }
            else
            {
                this.revertEnemiesGroupToOriginalSize();
                this.m_CurrentColumns = SpaceInvadersConfig.k_NumOfEnemiesColumns;
            }

            this.m_TimeUntilNextStepInSec = this.k_TimeUntilNextStepInSec;

            foreach (List<Enemy> list in this.m_EnemiesGroup)
            {
                foreach (Enemy enemy in list)
                {
                    if (enemy.Colum < this.m_CurrentColumns)
                    {
                        enemy.TimeUntilNextStepInSec = TimeSpan.FromSeconds(this.k_TimeUntilNextStepInSec);
                        enemy.Animations["CellAnimation"].Reset();
                        enemy.Animations["CellAnimation"].Pause();
                        this.updateScoreValueAndShootingFrequency(enemy);
                        enemy.Enabled = true;
                        enemy.Visible = true;
                        enemy.initPosition();
                        this.m_AliveEnemiesByRow.Add(enemy);
                        enemy.Animations["CellAnimation"].Reset();
                    }
                }
            }

            this.m_TimeCounter = 0f;
            this.initAliveEnemiesByColum();
        }

        // TODO: DEBUG CASE LEVEL 7
        private void revertEnemiesGroupToOriginalSize()
        {
            for (int colum = this.m_CurrentColumns; colum > SpaceInvadersConfig.k_NumOfEnemiesColumns; colum--)
            {
                this.removeColumToEnemyMantrix();
            }
        }

        private void AddOrRemoveEnemiesGroupColum()
        {
            for (int row = 0; row < SpaceInvadersConfig.k_NumOfEnemiesRows; row++)
            {
                this.m_EnemiesGroup[row][this.m_CurrentColumns].Visible = !this.m_EnemiesGroup[row][this.m_CurrentColumns].Visible;
                this.m_EnemiesGroup[row][this.m_CurrentColumns].Enabled = !this.m_EnemiesGroup[row][this.m_CurrentColumns].Enabled;
            }
        }

        private void addColumToEnemiesGroup()
        {
            this.AddOrRemoveEnemiesGroupColum();
        }

        private void removeColumToEnemyMantrix()
        {
            this.AddOrRemoveEnemiesGroupColum();
        }

        public override void Initialize()
        {
            if (this.m_GameEngine == null)
            {
                this.m_GameEngine = Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
            }

            for (int i = 0; i < SpaceInvadersConfig.k_NumOfEnemiesRows; i++)
            {
                this.m_EnemiesGroup.Add(new List<Enemy>(SpaceInvadersConfig.k_NumOfEnemiesColumns));
            }

            this.initEnemyGroup();
            this.initAliveEnemiesByColum();
            base.Initialize();
        }

        private void initEnemyGroup()
        {
            Enemy newEnemy;

            for (int row = 0; row < SpaceInvadersConfig.k_NumOfEnemiesRows; row++)
            {
                for (int colum = 0; colum < SpaceInvadersConfig.k_NumOfEnemiesColumns + 5; colum++)
                {
                    newEnemy = this.initEnemyByRow(row, colum);

                    this.m_EnemiesGroup[row].Add(newEnemy);
                    if (colum < SpaceInvadersConfig.k_NumOfEnemiesColumns)
                    {
                        this.m_AliveEnemiesByRow.Add(newEnemy);
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
            this.m_EnemiesGap = this.m_EnemiesGroup[0][0].Texture.Height * 0.6f;
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
            Enemy retEnemy = new Enemy(this.m_GameScreen, i_Tint, (int)i_ScoreValue, i_StartSqureIndex, i_Row, i_Colum, this.m_EnemiesGap, this.m_TimeUntilNextStepInSec);
            retEnemy.Toggeler = i_Toggeler;
            retEnemy.VisibleChanged += this.updateAliveLists;
            retEnemy.VisibleChanged += this.isFourEnemiesDead;

            return retEnemy;
        }

        private void initAliveEnemiesByColum()
        {
            for (int colomn = 0; colomn < this.m_CurrentColumns; colomn++)
            {
                for (int row = 0; row < SpaceInvadersConfig.k_NumOfEnemiesRows; row++)
                {
                    this.m_AliveEnemiesByColum.Add(this.m_EnemiesGroup[row][colomn]);
                }
            } 
        }

        private void updateScoreValueAndShootingFrequency(Enemy i_Enemy)
        {
            if (SpaceInvadersConfig.s_LogicLevel == SpaceInvadersConfig.eLevel.One)
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
            foreach (Enemy enemy in this.m_AliveEnemiesByRow)
            {
                enemy.Position += i_StepToJump;
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            if (this.isEnemiesGroupTouchTheBotton())
            {
                this.m_GameEngine.IsGameOver = true;
            }

            if (this.isAllEnemiesDead())
            {
                this.OnAllEnemiesDied(this, EventArgs.Empty);
            }

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
        }

        protected virtual void OnAllEnemiesDied(object sender, EventArgs args)
        {
            if (this.AllEnemiesDied != null)
            {
                this.AllEnemiesDied.Invoke(sender, args);
            }
        }

        private bool isAllEnemiesDead()
        {
            bool isAllDead = this.m_AliveEnemiesByRow.Count == 0;
            return isAllDead;
        }

        private bool isEnemiesGroupTouchTheBotton()
        {
            bool isEnemiesGroupTouchTheBotton = false;

            if (this.m_AliveEnemiesByRow.Count > 0)
            {
                isEnemiesGroupTouchTheBotton = this.getBottomGroupBorder() >= this.Game.GraphicsDevice.Viewport.Height;
            }

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
                this.jump(new Vector2(this.m_Direction * this.m_EnemiesGroup[0][0].Texture.Height / 2, 0));
            }
        }

        private bool isLastStep(float i_LastStep, eDirection i_MoveDirection)
        {
            bool isLastStep = false;

            if (this.m_Direction == (float)i_MoveDirection)
            {
                isLastStep = i_LastStep < this.m_EnemiesGroup[0][0].Texture.Height && i_LastStep > 0;
            }

            return isLastStep;
        }

        private void jumpDown()
        {
            this.jump(new Vector2(0, this.m_EnemiesGroup[0][0].Texture.Height / 2));
            this.increaseVelocity(0.08f);
        }

        private void increaseVelocity(float i_TimeToIncrease)
        {
            this.m_TimeUntilNextStepInSec -= this.m_TimeUntilNextStepInSec * i_TimeToIncrease;
        }
    }
}
