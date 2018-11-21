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
    public class ScoreManager : DrawableGameComponent
    {
        private enum eScoreValue
        {
            MotherShip = 850,
            Soul = 1100,
            PinkEnemy = 260,
            BlueEnemy = 140,
            YellowEnemy = 110
        }

        private SpriteFont m_ArialFont;
        private SpriteBatch m_SpriteBatch;
        private List<Soul> m_Souls;
        private int m_Score;

        public ScoreManager(Game i_Game) : base(i_Game)
        {
            this.m_Souls = new List<Soul>(3);
            this.m_Score = 0;

        }

        public int Score
        {
            get { return this.m_Score; }
            set { this.m_Score = value; }

        }

        public List<Soul> Souls
        {
            get { return this.m_Souls; }
            set { this.m_Souls = value; }

        }

        public void updateScoreAfterLoosingSoul()
        {
            this.m_Souls[this.m_Souls.Count - 1].RemoveComponent();
            this.m_Souls.RemoveAt(this.m_Souls.Count - 1);
            this.m_Score = (int)MathHelper.Clamp(this.m_Score - (int)eScoreValue.Soul, 0, float.PositiveInfinity);
        }

        public void UpdateScoreAfterCollision(Sprite sprite)
        {
            if(sprite is Enemy)
            {
                this.updateScoreAfterkillingEnemy(((Enemy)sprite).Tint);
            }
            else if(sprite is MotherSpaceShip)
            {
                this.updateScoreAfterKillingMotherShip();
            }
            else
            {
                this.updateScoreAfterLoosingSoul();
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

            this.m_Score += (int)scoreAddition;
        }

        private void updateScoreAfterKillingMotherShip()
        {
            this.m_Score += (int)eScoreValue.MotherShip;
        }

        

        protected override void LoadContent()
        {
            this.m_SpriteBatch = this.Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            this.m_ArialFont = Game.Content.Load<SpriteFont>("Arial");
        }

        public override void Draw(GameTime gameTime)
        {
            this.m_SpriteBatch.DrawString(this.m_ArialFont, "Score: " + this.m_Score, Vector2.One, Color.White);
            this.m_SpriteBatch.DrawString(this.m_ArialFont, "Souls: ", new Vector2(Game.GraphicsDevice.Viewport.Width - 170, 1), Color.White);
        }

        public override void Initialize()
        {
            for (int i = 0; i < this.m_Souls.Capacity; i++)
            {
                this.m_Souls.Add(new Soul(Game, Color.Green, i));
                this.m_Souls[i].AddComponent();
            }

            base.Initialize();
        }

    }
}
