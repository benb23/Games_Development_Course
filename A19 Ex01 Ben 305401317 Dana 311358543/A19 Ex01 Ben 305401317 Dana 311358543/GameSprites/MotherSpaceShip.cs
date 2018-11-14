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
    class MotherSpaceShip : Sprite
    {
        public MotherSpaceShip(Game i_Game) : base(i_Game)
        {
            m_AssetName = @"Sprites\MotherShip_32x120";
            m_Tint = Color.Red;
            m_visible = false;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void initPosition()
        {

        }

        public override void Update(GameTime i_GameTime)
        {
            m_Position.X += 90 * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
