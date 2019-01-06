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
    public class Soul : Sprite
    {
        private const int k_MaxSoulsNumber = 3;
        private int m_CurrSoulsNumber = k_MaxSoulsNumber;
        private int m_SoulIndx;
        private PlayerIndex m_SoulOwner;

        public Soul(Game i_Game, Vector2 i_Scale, float i_Opacity, string i_AssteName, PlayerIndex i_SoulOwner, int i_SoulIndex)
            : base(i_AssteName, i_Game)
        {
            Scales = i_Scale;
            Opacity = i_Opacity;  //TODO --> Cheak opacity
            m_SoulIndx = i_SoulIndex;
            m_SoulOwner = i_SoulOwner;
        }

        public int SoulIndex
        {
            set { this.m_SoulIndx = value; }
        }


        public override void Initialize()
        {
            base.Initialize();
            this.m_CurrSoulsNumber -= this.m_SoulIndx;
            float soulsGap = 5;
            this.Position = new Vector2(Game.GraphicsDevice.Viewport.Width - (this.m_CurrSoulsNumber * (Width + soulsGap)), 1 + ((int)m_SoulOwner * 20));
        }

    }
}
