﻿using System;
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
    public class ScoreBoardHeader : DrawableGameComponent
    {
        private SpriteFont m_Font;
        private ISpaceInvadersEngine m_GameEngine;
        private GameScreen m_GameScreen;

        public ScoreBoardHeader(GameScreen i_GameScreen) : base (i_GameScreen.Game)
        {
            m_GameScreen = i_GameScreen;
            i_GameScreen.Add(this);
        }

        public override void Initialize()
        {
            m_GameEngine = m_GameScreen.Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //m_SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
            this.m_Font = m_GameScreen.Game.Content.Load<SpriteFont>(@"Fonts\ComicSansMS");
        }

        public override void Draw(GameTime i_GameTime)
        {
            int playerTwoScore=0; //todo: dana change

            int playerOneScore = m_GameEngine.Players[(int)PlayerIndex.One].Score;
            if (m_GameEngine.NumOfPlayers == SpaceInvadersEngine.eNumOfPlayers.TwoPlayers)
            {
                playerTwoScore = m_GameEngine.Players[(int)PlayerIndex.Two].Score;
            }

            this.m_GameScreen.SpriteBatch.Begin();
            this.m_GameScreen.SpriteBatch.DrawString(this.m_Font, String.Format("P1 Score: {0}", playerOneScore.ToString()), new Vector2(2, (1 + (int)PlayerIndex.One * 15)), new Color(46, 145, 232));
            if (m_GameEngine.NumOfPlayers == SpaceInvadersEngine.eNumOfPlayers.TwoPlayers)
            {
                this.m_GameScreen.SpriteBatch.DrawString(this.m_Font, String.Format("P2 Score: {0}", playerTwoScore.ToString()), new Vector2(2, (1 + (int)PlayerIndex.Two * 15)), new Color(55, 232, 46));
            }
            this.m_GameScreen.SpriteBatch.End();
        }
    }
}
