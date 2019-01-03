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
    class MotherSpaceShip : Sprite, ICollidable2D
    {
        private const string k_AssteName = @"Sprites\MotherShip_32x120";
        private const float k_MotherShipVelocity = 40;
        private const int k_MaxRandomNumToDrawMotherShip = 70;
        private int m_CurrRandom = 100;
        public const int k_MaxRandomNumber = 50000;
        private IGameEngine m_GameEngine;

        public MotherSpaceShip(Game i_Game): base(k_AssteName, i_Game)
		{
            this.m_TintColor = Color.Red;
            this.Visible = false;
        }

        public override void Update(GameTime i_GameTime)
        {
            if (!this.Visible)
            {
                this.m_CurrRandom = SpaceInvaders.s_RandomNum.Next(0, k_MaxRandomNumber);
            }

            if (this.m_CurrRandom <= k_MaxRandomNumToDrawMotherShip)
            {
                this.Visible = true;
                this.m_Position.X += k_MotherShipVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
                if (m_Position.X >= GraphicsDevice.Viewport.Width)
                {
                    Visible = false;
                }
            }
        }

        void ICollidable.Collided(ICollidable i_Collidable)
        {
            Visible = false;

            if (m_GameEngine == null)
            {
                m_GameEngine = Game.Services.GetService(typeof(IGameEngine)) as IGameEngine;
            }

            m_GameEngine.HandleHit(this, i_Collidable);
        }
    }
}
