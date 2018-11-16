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
    class ScoreManager : DrawableGameComponent
    {
        private enum eScoreValue
        {
            MotherShip = 850,
            Soul = 1100,
            PinkEnemy =260,
            BlueEnemy =140,
            YellowEnemy =110
        };
        private SpriteFont m_ArialFont;
        private SpriteBatch m_SpriteBatch;
        private List<Soul> m_Souls;
        private int m_Score ;

        public ScoreManager(Game i_Game) : base(i_Game)
        {
            m_Souls = new List<Soul>(3);
            m_Score = 0;

        }

        public int Score
        {
            get { return m_Score; }
            set { m_Score = value; }

        }

        public List<Soul> Souls
        {
            get { return m_Souls; }
            set { m_Souls = value; }

        }

        public void updateScoreAfterLoosingSoul()
        {
            m_Souls[m_Souls.Count - 1].RemoveComponent();
            m_Souls.RemoveAt(m_Souls.Count - 1);
            m_Score = (int)MathHelper.Clamp(m_Score - (int)eScoreValue.Soul, 0, float.PositiveInfinity);
        }

        public void UpdateScoreAfterCollision(Sprite sprite)
        {
            if(sprite is Enemy)
            {
                updateScoreAfterkillingEnemy(((Enemy)sprite).Tint);
            }
            else if(sprite is MotherSpaceShip)
            {
                updateScoreAfterKillingMotherShip();
            }
            else
            {
                updateScoreAfterLoosingSoul();
            }

        }

        private void updateScoreAfterkillingEnemy(Color i_EnemyTint)
        {
            ScoreManager.eScoreValue scoreAddition;

            if (i_EnemyTint == Color.Pink)
            {
                scoreAddition = ScoreManager.eScoreValue.PinkEnemy;
            }
            else if (i_EnemyTint == Color.LightBlue)
            {
                scoreAddition = ScoreManager.eScoreValue.BlueEnemy;
            }
            else
            {
                scoreAddition = ScoreManager.eScoreValue.YellowEnemy;
            }

            m_Score += (int)scoreAddition;
        }

        private void updateScoreAfterKillingMotherShip()
        {
            m_Score += (int)eScoreValue.MotherShip;
        }

        

        protected override void LoadContent()
        {
            m_SpriteBatch = this.Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            m_ArialFont = Game.Content.Load<SpriteFont>("Arial");
        }

        public override void Draw(GameTime gameTime)
        {
            m_SpriteBatch.DrawString(m_ArialFont, "Score: " + m_Score , Vector2.One, Color.White);
            m_SpriteBatch.DrawString(m_ArialFont, "Souls: ", new Vector2(Game.GraphicsDevice.Viewport.Width - 170, 1), Color.White);
        }

        public override void Initialize()
        {
            for (int i = 0; i < m_Souls.Capacity ; i++)
            {
                m_Souls.Add(new Soul(Game, Color.Green, i));
                m_Souls[i].AddComponent();
            }

            base.Initialize();
        }

    }
}
