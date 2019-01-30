using System;
using Microsoft.Xna.Framework;
using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public class LevelTransitionScreen : GameScreen
    {
        private float m_TimeLeftForScreen = 3;
        private Background m_Background;
        private MenuHeader m_Counter;

        public LevelTransitionScreen(Game i_Game)
        : base(i_Game)
        {
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);
            this.m_Background.TintColor = Color.Green;
            this.m_Counter = new MenuHeader(this, @"Screens/Numbers_210x25");
        }

        public override void Initialize()
        {
            base.Initialize();
            initAnimations();
        }



        public override void Update(GameTime i_GameTime)
        {
            m_Counter.SourceRecWidth = (int)m_Counter.Width / 10;
            m_Counter.Animations.Restart();

            this.m_TimeLeftForScreen -= (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            if (this.m_TimeLeftForScreen <= 0)
            {
                this.ExitScreen();
            }
            base.Update(i_GameTime);
        }

        private void initAnimations()
        {

            CellAnimator countDownAnimation = new CellAnimator(new TimeSpan(0, 0, 1), 3, TimeSpan.Zero, 4);
            m_Counter.Animations.Add(countDownAnimation);
            m_Counter.Animations.Enabled = true;
        }


        public override string ToString()
        {
            return "LevelTransitionScreen";
        }

    }
}
