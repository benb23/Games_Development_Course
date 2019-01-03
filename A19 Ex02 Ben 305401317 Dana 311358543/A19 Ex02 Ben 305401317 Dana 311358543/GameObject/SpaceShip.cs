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
    public class SpaceShip : Sprite , ICollidable2D
    {
        private const float k_KeyboardVelocity = 120;
        private const string k_AssteName = @"Sprites\Ship01_32x32";
        private GameInputManager m_GameInputManager;
        private IGameEngine m_GameEngine;

        public SpaceShip(Game i_Game)
            : base(k_AssteName, i_Game)
        {}

        public override void Update(GameTime i_GameTime)
        {
            this.moveUsingKeyboard(i_GameTime);
            this.moveUsingMouse();
            this.Position = new Vector2(MathHelper.Clamp(Position.X, 0, GraphicsDevice.Viewport.Width - Texture.Width), Position.Y);
        }

        private void moveUsingMouse()
        {
            this.Position = new Vector2(this.Position.X + m_GameInputManager.MousePositionDelta.X, Position.Y);
        }

        private void moveUsingKeyboard(GameTime i_GameTime)
        {
            if (m_GameInputManager.KeyboardState.IsKeyDown(Keys.Left))
            {
                this.Position = new Vector2(this.Position.X - (k_KeyboardVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds), this.Position.Y);
            }
            else if (m_GameInputManager.KeyboardState.IsKeyDown(Keys.Right))
            {
                this.Position = new Vector2(this.Position.X + (k_KeyboardVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds), this.Position.Y);
            }
        }

        protected override void InitBounds()
        {
            m_Position.X = 0f;
            m_Position.Y = Game.GraphicsDevice.Viewport.Height - (Texture.Height * 1.2f);
        }

        void ICollidable.Collided(ICollidable i_Collidable)
        {
            if (m_GameEngine == null)
            {
                m_GameEngine = Game.Services.GetService(typeof(IGameEngine)) as IGameEngine;
            }

            m_GameEngine.HandleHit(this, i_Collidable);
        }
    }
}
