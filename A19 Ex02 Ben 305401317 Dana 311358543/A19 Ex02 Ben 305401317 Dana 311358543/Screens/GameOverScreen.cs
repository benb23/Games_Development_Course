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
    class GameOverScreen : MenuScreen
    {
        private Background m_Background;
        private MenuHeader m_GameOverHeader;

        public GameOverScreen(Game i_Game) : base(i_Game, new Vector2(250 ,250), 15f)
        {
            this.IsUsingKeyboard = false;
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);
            this.m_Background.TintColor = Color.Red;
            this.m_GameOverHeader = new MenuHeader(this, @"Screens\GameOver\GameOverLogo");
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

    }
}
