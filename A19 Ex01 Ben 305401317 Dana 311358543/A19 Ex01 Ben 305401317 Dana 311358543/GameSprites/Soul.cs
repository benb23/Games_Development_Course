using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace A19_Ex01_Ben_305401317_Dana_311358543
{
    class Soul : Sprite
    {
        private int m_SoulsNumber = 3;
        private int m_SoulIndx;

        public Soul(Game game, Color i_Tint, int SoulIndex) : base(game)
        {
            m_AssetName = @"Sprites\Ship01_32x32";
            m_Tint = i_Tint;
            m_SoulIndx = SoulIndex;


        }

        public int SoulIndex
        {
            set
            {
                m_SoulIndx = value;
            }
        }

        public override void Draw(GameTime i_GameTime)
        {
            float scaleRate = 0.8f;
            SpriteBatch.Draw(Texture, Position, null, m_Tint, 0f, Vector2.Zero, scaleRate, SpriteEffects.None, 0f);
        }

        public override void initPosition()
        {
            m_SoulsNumber -= m_SoulIndx;
            float soulsGap = Texture.Width * 0.2f;
            Position = new Vector2(Game.GraphicsDevice.Viewport.Width - m_SoulsNumber * (Texture.Width + soulsGap), 1); 
        }
    }
}
