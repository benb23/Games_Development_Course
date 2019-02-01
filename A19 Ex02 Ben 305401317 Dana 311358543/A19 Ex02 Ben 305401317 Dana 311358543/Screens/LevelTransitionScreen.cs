using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public class LevelTransitionScreen : GameScreen
    {
        private float m_TimeLeftForScreen = 3;
        private Background m_Background;
        private MenuHeader m_Counter;
        private SpriteFont m_Font;
        private Vector2 m_TextPosition;

        public LevelTransitionScreen(Game i_Game)
        : base(i_Game)
        {
            m_TextPosition = new Vector2(Game.GraphicsDevice.Viewport.Width / 2 - 50, Game.GraphicsDevice.Viewport.Height / 2 - 100);
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);
            this.m_Background.TintColor = Color.Green;
            this.m_Counter = new MenuHeader(this, @"Screens/Numbers_210x25");
            
        }

        public override void Initialize()
        {
            base.Initialize();
            if (!m_initialized)
            {
                initAnimations();
                m_initialized = true;
            }
            m_Counter.Animations.Restart();
            m_Counter.Position = m_TextPosition + new Vector2(m_Counter.Width * 0.75f, m_Counter.Height * 3);
        }

        public override void Update(GameTime i_GameTime)
        {
            m_Counter.SourceRecWidth = (int)m_Counter.Width / 10;
            
            this.m_TimeLeftForScreen -= (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            if (this.m_TimeLeftForScreen <= 0)
            {
                this.ExitScreen();
            }

            base.Update(i_GameTime);
        }

        private void initAnimations()
        {

            CellAnimator countDownAnimation = new CellAnimator(new TimeSpan(0, 0, 1), 7/**/, TimeSpan.Zero, 4);
            countDownAnimation.IsIncreasingProgression = false;
            m_Counter.Animations.Add(countDownAnimation);
            m_Counter.Animations.Enabled = true;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_Font = ContentManager.Load<SpriteFont>(@"Fonts\ERASDEMI");
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SpriteBatch.Begin();
            SpriteBatch.DrawString(m_Font, string.Format(@"Level : {0}", SpaceInvadersConfig.m_Level), m_TextPosition, Color.White);
            SpriteBatch.End();
        }

        public override string ToString()
        {
            return "LevelTransitionScreen";
        }
    }
}
