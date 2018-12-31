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
    class Enemy : Sprite, ICollidable2D
    {
        private const string k_AssteName = @"Sprites\Enemy01_32x32";
        Gun m_Gun;

        public Enemy(Game i_Game, Color i_EnemyColor)
            : base(k_AssteName, i_Game)
        {
            m_TintColor = i_EnemyColor;
        }

        protected override void InitBounds()
        {
            base.InitBounds();

            // put in bottom center of view port:
            // get the bottom and center
            float x = (float)GraphicsDevice.Viewport.Width / 2;
            float y = 50;

            // offset:
            x -= m_Width / 2;

            m_Position = new Vector2(x, y);
        }

        void ICollidable.Collided(ICollidable i_Collidable)
        {

        }

        bool ICollidable.CheckCollision(ICollidable i_Source)
        {
            return false;
        }
    }
    
}
