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
        public enum ScoreAddition{ KillingMotherShip = 850, LoosingSoul = -1100, KillingPinkEnemy =260, KillingBlueEnemy=140, KillingYellowEnemy=110 };
        private SpriteBatch m_SpriteBatch;
        private List<Soul> m_Souls;
        private int m_Score = 0;
        private SpriteFont arial;
        
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
            //m_Souls.Remove(Soul);?????????????
            m_Score = (int)MathHelper.Clamp(m_Score + (int)ScoreAddition.LoosingSoul, 0, float.PositiveInfinity);
        }

        public void UpdateScore(Sprite sprite)
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
            ScoreManager.ScoreAddition scoreAddition;

            if (i_EnemyTint == Color.Pink)
            {
                scoreAddition = ScoreManager.ScoreAddition.KillingPinkEnemy;
            }
            else if (i_EnemyTint == Color.LightBlue)
            {
                scoreAddition = ScoreManager.ScoreAddition.KillingBlueEnemy;
            }
            else
            {
                scoreAddition = ScoreManager.ScoreAddition.KillingYellowEnemy;
            }

            m_Score += (int)scoreAddition;
        }

        private void updateScoreAfterKillingMotherShip()
        {
            m_Score += (int)ScoreAddition.KillingMotherShip;
        }

        public ScoreManager(Game i_Game) : base(i_Game)
        {
            m_Souls = new List<Soul>(3);
            
        }

        protected override void LoadContent()
        {
            m_SpriteBatch = this.Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            arial = Game.Content.Load<SpriteFont>("Arial");
        }

        public override void Draw(GameTime gameTime)
        {
            m_SpriteBatch.DrawString(arial, "Score: " + m_Score , Vector2.One, Color.White);
            m_SpriteBatch.DrawString(arial, "Souls: ", new Vector2(Game.GraphicsDevice.Viewport.Width - 170, 1), Color.White);
        }

        public override void Initialize()
        {
            for (int i = 0; i < m_Souls.Capacity ; i++)
            {
                m_Souls.Add(new Soul(Game, Color.Green));
                m_Souls[i].AddComponent();
            }

            // init souls positons
            float gap = 32 * 0.2f;
            int k = 3;
            foreach (Soul soul in m_Souls)
            {
                soul.Position = new  Vector2(Game.GraphicsDevice.Viewport.Width - k * (32 + gap)  , 1); // TODO: CHANGE CONST
                k--;
            }
            base.Initialize();

        }

    }
}
