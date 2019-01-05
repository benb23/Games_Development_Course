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
    class EnemiesGroup : DrawableGameComponent
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
        private List<Enemy> m_AliveEnemies = new List<Enemy>(k_EnemiesRows* k_EnemiesColumns);
        private List<Enemy> m_AliveEnemiesByColum = new List<Enemy>(k_EnemiesRows * k_EnemiesColumns);
        private List<Enemy> m_AliveEnemiesByRow = new List<Enemy>(k_EnemiesRows * k_EnemiesColumns);

        private IGameEngine m_GameEngine;
        private Rectangle m_AliveEnemiesRec;
        float m_EnemiesGap;
        private int m_Toggeler = 1;

        public EnemiesGroup(Game i_Game) : base(i_Game)
        {
            this.m_EnemiesMatrix = new Enemy[k_EnemiesRows, k_EnemiesColumns];
        }

        public override void Update(GameTime i_GameTime)
        {
            this.m_TimeCounter += (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            //TODO: CHEAK
            if (isFourEnemiesDead())
            {
               // this.m_IncreaseVelocityWhen4Dead = false;
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
                doEnemyCellAnimation();
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
            this.initAliveEnemiesByColum();

            // initilize enemies positions
            this.updatePositions(this.m_currTopLeftX, this.m_currTopLeftY);

            base.Initialize();
        }

        private void initEnemyGroup()
        {
            for (int i = 0; i < k_EnemiesRows; i++)
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
            m_EnemiesGap = this.m_EnemiesMatrix[0, 0].Texture.Height * 0.6f;

            m_AliveEnemiesRec = new Rectangle(
                    0,
                    this.m_EnemiesMatrix[0, 0].Texture.Height * 3,
                    (int)((this.m_EnemiesMatrix[0, 0].Texture.Height + m_EnemiesGap) * 9 - m_EnemiesGap),
                    (int)((this.m_EnemiesMatrix[0, 0].Texture.Height + m_EnemiesGap) * 5 - m_EnemiesGap)
                );




            this.m_currTopLeftX = this.m_EnemiesMatrix[0, 0].Texture.Height / 2;
            this.m_currTopLeftY = this.m_EnemiesMatrix[0, 0].Texture.Height * 3f;
            // Vector2 x = m_EnemiesMatrix[0, 0].TopLeftPosition;
        }
  
        private void initEnemiesRow(int i_Row, int i_StartSqureIndex, Color i_Tint)
        {
            for (int colum = 0; colum < k_EnemiesColumns; colum++)
            {
                this.m_EnemiesMatrix[i_Row, colum] = new Enemy(Game, i_Tint, i_StartSqureIndex, i_Row, colum);
                m_AliveEnemiesByRow.Add(m_EnemiesMatrix[i_Row, colum]);
                this.m_EnemiesMatrix[i_Row, colum].VisibleChanged += this.updateAliveLists;
            }
        }

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

        private void updatePositions(float i_X, float i_Y)
        {
            
            float enemiesGap = this.m_EnemiesMatrix[0, 0].Texture.Height * 0.6f;
            int halfEnemySize = this.m_EnemiesMatrix[0, 0].Texture.Height / 2;
            float startX = i_X;
            float strartY = i_Y;

            for (int i = 0; i < k_EnemiesRows; i++)
            {
                for (int j = 0; j < k_EnemiesColumns; j++)
                {
                    this.m_EnemiesMatrix[i, j].Position = new Vector2(startX + halfEnemySize, strartY + halfEnemySize);
                    this.m_EnemiesMatrix[i, j].PositionOrigin = new Vector2(halfEnemySize);
                    startX += this.m_EnemiesMatrix[0, 0].Texture.Height + enemiesGap;
                }

                startX = i_X;
                strartY += this.m_EnemiesMatrix[0, 0].Texture.Height + enemiesGap;
            }
        }

        private void doEnemyCellAnimation()
        {
            foreach (Enemy enemy in m_AliveEnemiesByRow)
            {
                if (enemy.Row == 0)
                {
                    enemy.m_StartSqureIndex++;
                    enemy.m_StartSqureIndex %= enemy.k_NumOfFrames;
                }
                else if (enemy.Row == 1 || enemy.Row == 3)
                {
                    enemy.m_StartSqureIndex += m_Toggeler;
                }
                else if (enemy.Row == 2 || enemy.Row == 4)
                {
                    enemy.m_StartSqureIndex -= m_Toggeler;
                }

                enemy.SourceRectangle = new Rectangle(
                    (int)enemy.WidthBeforeScale * enemy.m_StartSqureIndex,
                    0,
                    (int)enemy.WidthBeforeScale,
                    (int)enemy.Height
                );
            }

            m_Toggeler *= -1;
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
            return m_AliveEnemiesByColum.Last().Position.X + m_AliveEnemiesByColum.Last().HeightBeforeScale/2;
        }

        private float getLeftGroupBorder()
        {
            return m_AliveEnemiesByColum.First().Position.X - m_AliveEnemiesByColum.First().HeightBeforeScale / 2;
        }

        private float getBottomGroupBorder()
        {
            return m_AliveEnemiesByRow.Last().Position.Y + m_AliveEnemiesByRow.Last().HeightBeforeScale / 2;
        }

        private void updateAliveLists(object sender, EventArgs args)
        {
            m_AliveEnemiesByRow.Remove((sender as Enemy));
            m_AliveEnemiesByColum.Remove((sender as Enemy));
        }

        private bool isFourEnemiesDead()
        {
            int numOfDeadEnemies = m_AliveEnemiesByRow.Capacity - m_AliveEnemiesByRow.Count;
            bool isFourEnemiesDead = numOfDeadEnemies % 4 == 0 && numOfDeadEnemies != 0;

            return isFourEnemiesDead;
        }

        private void jumpHorizontalStep(GameTime i_GameTime)
        {
            float lastRightJump = Game.GraphicsDevice.Viewport.Width - this.getRightGroupBorder();
            float lastLeftJump = this.getLeftGroupBorder();

            if (lastRightJump < (this.m_EnemiesMatrix[0, 0].Texture.Height / 2) && lastRightJump > 0)
            {
                this.m_currTopLeftX += this.m_Direction * lastRightJump;
                this.m_IsLastStepInRow = true;
            }
            else if (lastLeftJump < (this.m_EnemiesMatrix[0, 0].Texture.Height / 2) && lastLeftJump > 0 && this.m_Direction == -1f)
            {
                this.m_currTopLeftX += this.m_Direction * lastLeftJump;
                this.m_IsLastStepInRow = true;
            }
            else
            {
                this.m_currTopLeftX += this.m_Direction * (this.m_EnemiesMatrix[0, 0].Texture.Height / 2);
            }
        }

        private void jumpDown()
        {
            this.m_currTopLeftY += this.m_EnemiesMatrix[0, 0].Texture.Height / 2;
            this.increaseVelocity(0.08f);
        }

        private void increaseVelocity(float i_TimeToIncrease)
        {
            this.m_TimeUntilNextStepInSec -= this.m_TimeUntilNextStepInSec * i_TimeToIncrease;
        }
    }
}
