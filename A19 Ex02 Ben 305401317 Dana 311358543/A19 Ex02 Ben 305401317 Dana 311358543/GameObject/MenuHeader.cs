using System;
using Microsoft.Xna.Framework;
using Infrastructure;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public class MenuHeader : Sprite
    {
        GameScreen m_GameScreen;
        private float m_OffsetY = 20;
        private float m_OffsetX = 0;


        public MenuHeader(GameScreen i_GameScreen, string i_AssetName) : base(i_AssetName, i_GameScreen)
        {
            this.m_GameScreen = i_GameScreen;
            initDedaultPosition();
        }

        public MenuHeader(GameScreen i_GameScreen, string i_AssetName, Vector2 i_Position) : base(i_AssetName, i_GameScreen)
        {
            this.m_GameScreen = i_GameScreen;
            this.Position = i_Position;
        }

        public MenuHeader(GameScreen i_GameScreen, string i_AssetName, float i_Scale) : base(i_AssetName, i_GameScreen)
        {
            this.m_GameScreen = i_GameScreen;
            initDedaultPosition();
            this.m_Scales = new Vector2(i_Scale);
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            this.Position = new Vector2(m_GameScreen.Game.Window.ClientBounds.Width / 2 + m_OffsetX, m_OffsetY);
        }

        private void initDedaultPosition()
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

        public override void Initialize()
        {
            base.Initialize();
            InitOrigins();
            this.m_GameScreen.Game.Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        protected override void InitOrigins()
        {
            this.PositionOrigin = new Vector2(this.Texture.Width/2, 0);
            base.InitOrigins();
        }
    }
}

