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
    public class WallsGroup : DrawableGameComponent
    {
        private const int k_NumOfWalls = 4;
        private float m_Direction = 1f;
        private List<Wall> m_WallGroup;
        private Color[] m_OriginalPixels;

        public WallsGroup(Game i_Game) : base(i_Game)
        {
            this.m_WallGroup = new List<Wall>(k_NumOfWalls);
        }

        public override void Initialize()
        {
            foreach (Wall wall in m_WallGroup)
            {
                
            }

            base.Initialize();
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
        }
    }
}
