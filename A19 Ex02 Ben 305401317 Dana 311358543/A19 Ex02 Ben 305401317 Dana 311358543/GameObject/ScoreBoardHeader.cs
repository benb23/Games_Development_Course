using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Infrastructure;

namespace A19_Ex03_Ben_305401317_Dana_311358543
{
    public class ScoreBoardHeader : DrawableGameComponent
    {
        private SpriteFont m_Font;
        private ISpaceInvadersEngine m_GameEngine;
        private GameScreen m_GameScreen;

        public ScoreBoardHeader(GameScreen i_GameScreen) : base(i_GameScreen.Game)
        {
            this.m_GameScreen = i_GameScreen;
            i_GameScreen.Add(this);
        }

        public override void Initialize()
        {
            this.m_GameEngine = this.m_GameScreen.Game.Services.GetService(typeof(ISpaceInvadersEngine)) as ISpaceInvadersEngine;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.m_Font = this.m_GameScreen.Game.Content.Load<SpriteFont>(@"Fonts\ComicSansMS");
        }

        public override void Draw(GameTime i_GameTime)
        {
            int playerTwoScore = 0;

            int playerOneScore = this.m_GameEngine.Players[(int)PlayerIndex.One].Score;
            if (SpaceInvadersConfig.s_NumOfPlayers == SpaceInvadersConfig.eNumOfPlayers.TwoPlayers)
            {
                playerTwoScore = this.m_GameEngine.Players[(int)PlayerIndex.Two].Score;
            }

            this.m_GameScreen.SpriteBatch.Begin();
            this.drawPlayerString((int)PlayerIndex.One, playerOneScore, new Color(46, 145, 232));

            if (SpaceInvadersConfig.s_NumOfPlayers == SpaceInvadersConfig.eNumOfPlayers.TwoPlayers)
            {
                this.drawPlayerString((int)PlayerIndex.Two, playerTwoScore, new Color(55, 232, 46));
            }

            this.m_GameScreen.SpriteBatch.End();
        }

        private void drawPlayerString(int i_PlayerIndex, int i_PlayerScore, Color i_Color)
        {
            this.m_GameScreen.SpriteBatch.DrawString(this.m_Font, string.Format("P{0} Score: {1}", i_PlayerIndex + 1, i_PlayerScore.ToString()), new Vector2(2, (i_PlayerIndex) * 15), i_Color);
        }
    }
}
