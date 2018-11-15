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
        private SpriteBatch m_SpriteBatch;
        private List<Soul> m_Souls;
        private int m_Score = 0;
        private SpriteFont arial;
        
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
