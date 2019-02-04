using System;
using Microsoft.Xna.Framework;
using Infrastructure;

namespace A19_Ex03_Ben_305401317_Dana_311358543
{
    public class MenuHeader : Sprite
    {
        private float m_OffsetY = 20;
        private float m_OffsetX = 0;

        public float OffsetY
        {
            set
            {
                this.m_OffsetY = value;
                this.initDedaultPosition();
            }
        }

        public float OffsetX
        {
            set
            {
                this.m_OffsetX = value;
                this.initDedaultPosition();
            }
        }

        public MenuHeader(GameScreen i_GameScreen, string i_AssetName) : base(i_AssetName, i_GameScreen)
        {
            this.m_GameScreen = i_GameScreen;
            this.initDedaultPosition();
        }

        public MenuHeader(GameScreen i_GameScreen, string i_AssetName, float i_Scale) : base(i_AssetName, i_GameScreen)
        {
            this.m_GameScreen = i_GameScreen;
            this.initDedaultPosition();
            this.m_Scales = new Vector2(i_Scale);
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            this.Position = new Vector2((this.m_GameScreen.Game.Window.ClientBounds.Width / 2) + this.m_OffsetX, this.m_OffsetY);
        }

        private void initDedaultPosition()
        {
            this.Position = new Vector2((this.m_GameScreen.Game.Window.ClientBounds.Width / 2) + this.m_OffsetX, this.m_OffsetY);
        }

        public int SourceRecWidth
        {
            set { this.m_SourceRectangle.Width = value; }
        }

        public override void Initialize()
        {
            base.Initialize();
            this.InitOrigins();
            this.m_GameScreen.Game.Window.ClientSizeChanged += this.Window_ClientSizeChanged;
        }

        protected override void InitOrigins()
        {
            this.PositionOrigin = new Vector2(this.Texture.Width / 2, 0);
            base.InitOrigins();
        }
    }
}