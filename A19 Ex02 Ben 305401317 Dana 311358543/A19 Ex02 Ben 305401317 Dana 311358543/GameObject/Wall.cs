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
    public class Wall : CollidableSprite, IPixelsCollidable , IRectangleCollidable
    {
        private Vector2 m_StartingPosition;
        private const int k_NumOfWalls = 4;
        private const string k_AssteName = @"Sprites\Barrier_44x32";
        private List<Wall> m_Walls;
        private bool m_Initialize = false;

        public Wall(Game i_Game)
            : base(k_AssteName, i_Game)
        {
            this.m_Velocity = new Vector2(45, 0);
        }

        public override void Update(GameTime gameTime)
        {
            if(!m_Initialize)
            {
                m_StartingPosition = Position;
                m_Initialize = true;
            }

            if((Position.X - m_StartingPosition.X >= Texture.Width/2) || (Position.X + Texture.Width / 2 <= m_StartingPosition.X))
            {
                Velocity *= -1;
            }

            base.Update(gameTime);
        }
        protected override void InitOrigins()
        {
            m_PositionOrigin = new Vector2(Texture.Width / 2, Texture.Height);
            base.InitOrigins();
        }

        void ICollidable.Collided(ICollidable i_Collidable)
        {
            //TODO : move to gameEngine?
            if(i_Collidable is Bullet)
            {
                


            }
        }

    }
}
