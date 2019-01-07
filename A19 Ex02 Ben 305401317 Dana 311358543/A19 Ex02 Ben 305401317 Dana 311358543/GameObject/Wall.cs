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
    public class Wall : PixelsCollidableSprite, IPixelsCollidable
    {
        private const int k_NumOfWalls = 4;
        private const string k_AssteName = @"Sprites\Barrier_44x32";
        private List<Wall> m_Walls;

        public Wall(Game i_Game)
            : base(k_AssteName, i_Game)
        {
            this.m_Velocity = new Vector2(45, 0);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        protected override void InitOrigins()
        {
            m_PositionOrigin = new Vector2(Texture.Width / 2, Texture.Height);
            base.InitOrigins();
        }

        void ICollidable.Collided(ICollidable i_Collidable)
        {
        }

    }
}
