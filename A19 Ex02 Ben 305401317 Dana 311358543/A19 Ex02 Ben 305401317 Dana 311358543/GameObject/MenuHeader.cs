using System;
using Microsoft.Xna.Framework;
using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public class MenuHeader : Sprite
    {
        GameScreen m_GameScreen;
        float m_OffsetY = 20;
        float m_OffsetX = 0;

        public MenuHeader(GameScreen i_GameScreen, string i_AssetName) : base(i_AssetName, i_GameScreen)
        {
            this.m_GameScreen = i_GameScreen;
        }

        public MenuHeader(GameScreen i_GameScreen, string i_AssetName, Vector2 i_Scale) : base(i_AssetName, i_GameScreen)
        {
            this.m_GameScreen = i_GameScreen;
            this.m_Scales = i_Scale;    
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            this.Position = new Vector2(m_GameScreen.Game.Window.ClientBounds.Width / 2 + m_OffsetX, m_OffsetY);
        }

        public float OffsetY
        {
            set { m_OffsetY = value; }
        }

        public float OffsetX
        {
            set { m_OffsetX = value; }
        }

        public int SourceRecWidth
        {
            set { m_SourceRectangle.Width = value; }
        }

        public override void Update(GameTime gameTime)
        {
            if (!m_Initialize)
            {
                InitOrigins();
                this.Position = new Vector2(m_GameScreen.Game.Window.ClientBounds.Width / 2 + m_OffsetX, m_OffsetY);
                this.m_GameScreen.Game.Window.ClientSizeChanged += Window_ClientSizeChanged;
                m_Initialize = true;
            }

            base.Update(gameTime);
        }

        protected override void InitOrigins()
        {
            this.PositionOrigin = new Vector2(this.Texture.Width / 2, 0);
            base.InitOrigins();
        }
    }
}

