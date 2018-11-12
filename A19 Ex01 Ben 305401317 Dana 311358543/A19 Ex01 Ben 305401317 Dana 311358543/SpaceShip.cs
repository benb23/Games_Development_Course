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
    class SpaceShip : GameObject
    {
        private float m_NumOfBullets;
        private Gun m_Gun;

        public void Update()
        {
                // get the current input devices state:
            KeyboardState currKeyboardState = Keyboard.GetState();

            // Allows the game to exit by GameButton 'back' button or Esc:
            if (currKeyboardState.IsKeyDown(Keys.Escape))   
            {
                //TODO: msg box
                this.Exit();
            }

            if(currKeyboardState.IsKeyDown(Keys.Enter) && m_PastKey.IsKeyUp(Keys.Enter))
            {
                m_SpaceShip.Shoot();
            }

            //update enemysGroup position
            if(m_EnemysGroup.LeftBorder() == 0f || m_EnemysGroup.RightBorder() == this.GraphicsDevice.Viewport.Width)
            {
                
            }
            else if(m_EnemysGroup.LeftBorder() < ?? || m_EnemysGroup.RightBorder() > this.GraphicsDevice.Viewport.Width - ??)
            {

            }
            else
            {

            }

            //update spaceship bullets position
            if(m_SpaceShip.Gun.BulletsList.Count != 0)
            {

            }

            //update enemies bullets position
            if (m_EnemysGroup.Gun.BulletsList.Count != 0)
            {

            }

            // move the ship using the keyboard:
            if (currKeyboardState.IsKeyDown(Keys.Left))
            {
                m_SpaceShip.Position = new Vector2(m_SpaceShip.Position.X - r_KeyboardVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds, m_SpaceShip.Position.Y);
            }
            else if(currKeyboardState.IsKeyDown(Keys.Right))
            {
                m_SpaceShip.Position = new Vector2(m_SpaceShip.Position.X + r_KeyboardVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds, m_SpaceShip.Position.Y);
            }

            // move the ship using the mouse:
            m_SpaceShip.Position = new Vector2(m_SpaceShip.Position.X + GetMousePositionDelta().X,m_SpaceShip.Position.Y);

            // clam the position between screen boundries:
            m_SpaceShip.Position = new Vector2( MathHelper.Clamp(m_SpaceShip.Position.X, 0, this.GraphicsDevice.Viewport.Width - m_SpaceShip.Texture.Width),m_SpaceShip.Position.Y);

            // if we hit the wall, lets change direction:
            if (m_SpaceShip.Position.X == 0 || m_SpaceShip.Position.X == this.GraphicsDevice.Viewport.Width - m_SpaceShip.Texture.Width)
            {
                m_SpaceShip.Direction *= -1f;
            }

            base.Update(gameTime);
        }
        public Gun Gun
        {
            get{ return m_Gun; }
        }

        public SpaceShip()
        {
            m_Direction = 1f;
        }
        public void Init()
        {

        }
        public override void Move()
        {
            
        }

        public void  Shoot()
        {

        }


    }
}
