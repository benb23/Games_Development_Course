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
    public class Background : Sprite
    {
        public Background(Game i_Game) : base(i_Game)
        {
            this.m_AssetName = @"Sprites\BG_Space01_1024x768";
            this.m_Tint = Color.White;
        }

        public override void Initialize()
        {
            this.DrawOrder = int.MinValue;
            base.Initialize();
        }

        public override void InitPosition()
        {
            this.m_Position = Vector2.Zero;
        }
    }
}
