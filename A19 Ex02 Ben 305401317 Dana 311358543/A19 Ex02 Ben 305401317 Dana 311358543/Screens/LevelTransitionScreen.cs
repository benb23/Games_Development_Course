using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure;

namespace A19_Ex03_Ben_305401317_Dana_311358543
{
    public class LevelTransitionScreen : GameScreen
    {
        private float m_TimeLeftForScreen = 3;
        private Background m_Background;
        private MenuHeader m_Counter;
        private SpriteFont m_Font;
        private Vector2 m_TextPosition;

        public LevelTransitionScreen(Game i_Game) : base(i_Game)
        {
            this.m_TextPosition = new Vector2((this.Game.GraphicsDevice.Viewport.Width / 2) - 50, (this.Game.GraphicsDevice.Viewport.Height / 2) - 100);
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);
            this.m_Background.TintColor = Color.Green;
            this.m_Counter = new MenuHeader(this, @"Screens/Numbers_210x25");
        }

        public override void Initialize()
        {
            base.Initialize();

            if (!this.m_initialized)
            {
                this.initAnimations();
                this.m_initialized = true;
            }

            this.m_Counter.Animations.Restart();
            this.m_Counter.Position = this.m_TextPosition + new Vector2(this.m_Counter.Width * 0.75f, this.m_Counter.Height * 3);
        }

        public override void Update(GameTime i_GameTime)
        {
            this.m_Counter.SourceRecWidth = (int)this.m_Counter.Width / 10;
            
            this.m_TimeLeftForScreen -= (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            if (this.m_TimeLeftForScreen <= 0)
            {
                this.ExitScreen();
            }

            base.Update(i_GameTime);
        }

        private void initAnimations()
        {
            CellAnimator countDownAnimation = new CellAnimator(new TimeSpan(0, 0, 1), 7, TimeSpan.Zero, 4);
            countDownAnimation.IsIncreasingProgression = false;
            this.m_Counter.Animations.Add(countDownAnimation);
            this.m_Counter.Animations.Enabled = true;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.m_Font = ContentManager.Load<SpriteFont>(@"Fonts\ERASDEMI");
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            this.SpriteBatch.Begin();
            this.SpriteBatch.DrawString(this.m_Font, string.Format(@"Level : {0}", SpaceInvadersConfig.m_Level + 1 ), this.m_TextPosition, Color.White);
            this.SpriteBatch.End();
        }

        public override string ToString()
        {
            return "LevelTransitionScreen";
        }
    }
}
