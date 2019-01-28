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

        public float OffsetY
        {
            set { m_OffsetY = value; }
        }

        public float OffsetX
        {
            set { m_OffsetX = value; }
        }

        public override void Update(GameTime gameTime)
        {
            if (!m_Initialize)
            {
                InitOrigins();
                this.Position = new Vector2(m_GameScreen.Game.GraphicsDevice.Viewport.Width / 2 + m_OffsetX, m_OffsetY);
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

