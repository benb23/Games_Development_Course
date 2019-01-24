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
    public class WelcomeScreen : GameScreen
    {
        private Background m_Background;

        public WelcomeScreen(Game i_Game)
        : base(i_Game)
        {
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.Enter))
            {
                ExitScreen();
            }
            if (InputManager.KeyPressed(Keys.Escape))
            {
                Game.Exit();
            }
        }
    }
}
