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

using Infrastructure;


namespace A19_Ex02_Ben_305401317_Dana_311358543
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
                ExitScreen();
            }
        }
    }
}
