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
    public class SpaceShip : Sprite, ICollidable2D
    {
        private float k_Speed = 145; //TODO : VELOCITY
        private const string k_AssteName = @"Sprites\Ship01_32x32";
        private IInputManager m_InputManager;
        private IGameEngine m_GameEngine;
        private Gun m_Gun = new Gun();

        public Gun Gun
        {
            get { return m_Gun; }
        }

        public float Speed
        {
            get{return k_Speed; }
        }

        public SpaceShip(Game i_Game)
            : base(k_AssteName, i_Game)
        {}

        void ICollidable.Collided(ICollidable i_Collidable)
        {
            if (!(i_Collidable is SpaceShip))
            {
                if (m_GameEngine == null)
                {
                    m_GameEngine = Game.Services.GetService(typeof(IGameEngine)) as IGameEngine;
                }

                m_GameEngine.HandleHit(this, i_Collidable);
            }
        }

        protected override void InitOrigins()
        {
            m_PositionOrigin = new Vector2(Texture.Width / 2, Texture.Height);
            m_RotationOrigin = new Vector2(Texture.Width / 2, Texture.Height/2);
            base.InitOrigins();
        }
    }
}
