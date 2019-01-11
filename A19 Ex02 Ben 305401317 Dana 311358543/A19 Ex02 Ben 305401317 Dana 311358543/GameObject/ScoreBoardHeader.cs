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
    public class ScoreBoardHeader : Sprite
    {
        private SpriteFont m_Font;
        private ISpaceInvadersEngine m_GameEngine;
        private Game m_Game;

        public ScoreBoardHeader(Game i_Game) : base(string.Empty, i_Game)
        {
            this.m_Game = i_Game;
        }

        public override void Initialize()
        {
            this.m_GameEngine = this.m_Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.m_SpriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            this.m_Font = this.m_Game.Content.Load<SpriteFont>(@"Fonts\ComicSansMS");
        }

        public override void Draw(GameTime i_GameTime)
        {
            int playerOneScore = this.m_GameEngine.Players[(int)PlayerIndex.One].Score;
            int playerTwoScore = this.m_GameEngine.Players[(int)PlayerIndex.Two].Score;

            this.m_SpriteBatch.Begin();
            this.m_SpriteBatch.DrawString(this.m_Font, string.Format("P1 Score: {0}", playerOneScore.ToString()), new Vector2(2, 1 + ((int)PlayerIndex.One * 15)), new Color(46, 145, 232));
            this.m_SpriteBatch.DrawString(this.m_Font, string.Format("P2 Score: {0}", playerTwoScore.ToString()), new Vector2(2, 1 + ((int)PlayerIndex.Two * 15)), new Color(55, 232, 46));
            this.m_SpriteBatch.End();
        }
    }
}
