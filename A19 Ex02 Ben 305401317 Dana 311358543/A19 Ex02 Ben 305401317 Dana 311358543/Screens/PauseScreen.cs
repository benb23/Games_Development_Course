using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure;

namespace A19_Ex03_Ben_305401317_Dana_311358543
{
    public class PauseScreen : GameScreen
    {
        private MenuHeader m_PauseMsg;

        public PauseScreen(Game i_Game) : base(i_Game)
        {
            this.IsModal = true;
            this.IsOverlayed = true;
            this.UseGradientBackground = true;
            this.BlackTintAlpha = 0.4f;
            this.m_PauseMsg = new MenuHeader(this, @"Screens\Pause\PausedMessage");
            this.m_PauseMsg.OffsetY = 250;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.R))
            {
                this.ExitScreen();
            }
        }

        public override string ToString()
        {
            return "PauseScreen";
        }
    }
}
