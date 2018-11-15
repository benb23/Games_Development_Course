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
        public Soul(Game game, Color i_Tint) : base(game)
        {
            m_AssetName = @"Sprites\Ship01_32x32";
            m_Tint = i_Tint;
            
        }

        public override void Draw(GameTime i_GameTime)
        {
            SpriteBatch.Draw(Texture, Position, null, m_Tint, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void initPosition()
        {
            
        }
    }
}
