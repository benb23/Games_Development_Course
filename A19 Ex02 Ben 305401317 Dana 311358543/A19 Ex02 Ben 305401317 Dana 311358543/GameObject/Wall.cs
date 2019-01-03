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
    public class Wall : Sprite, ICollidable2D
    {
        private const int k_NumOfWalls = 4;
        private const string k_AssteName = @"XXXXXX";
        private List<Wall> m_Walls;

        public Wall(Game i_Game)
            : base(k_AssteName, i_Game)
        {
            this.m_Velocity = new Vector2(45, 0);
        }

        void ICollidable.Collided(ICollidable i_Collidable)
        {
        }
    }
}
