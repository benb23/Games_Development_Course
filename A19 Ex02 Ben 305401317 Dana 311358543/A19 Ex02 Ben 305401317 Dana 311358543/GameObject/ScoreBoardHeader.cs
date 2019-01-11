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


        public ScoreBoardHeader(Game i_Game) : base ("" , i_Game)
        {
            m_Game = i_Game;
        }

        public override void Initialize()
        {
            m_GameEngine = m_Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            m_SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
            this.m_Font = m_Game.Content.Load<SpriteFont>(@"Fonts\ComicSansMS");
        }

        public override void Draw(GameTime i_GameTime)
        {
            int playerOneScore = m_GameEngine.Players[(int)PlayerIndex.One].Score;
            int playerTwoScore = m_GameEngine.Players[(int)PlayerIndex.Two].Score;

            //TODO: String format
            this.m_SpriteBatch.Begin();
            this.m_SpriteBatch.DrawString(this.m_Font, "P1 Score: " + playerOneScore.ToString(), new Vector2(2, 1 + (int)PlayerIndex.One * 15) , new Color(46, 145, 232));
            this.m_SpriteBatch.DrawString(this.m_Font, "P2 Score: " + playerTwoScore.ToString(), new Vector2(2, 1 + (int)PlayerIndex.Two * 15), new Color(55, 232, 46));
            this.m_SpriteBatch.End();
        }





    }
}
