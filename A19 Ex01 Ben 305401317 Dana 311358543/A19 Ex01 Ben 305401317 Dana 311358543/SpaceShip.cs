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
    class SpaceShip : Sprite
    {
        private readonly float r_KeyboardVelocity = 120;//TODO: ctor?
        private float m_NumOfBullets = 3;//TODO: ctor?
        private Gun m_Gun = new Gun();//TODO: ctor?

        public override void Update(GameTime gameTime)
        {
            KeyboardState currKeyboardState = Keyboard.GetState();

            // move the ship using the keyboard:
            if (currKeyboardState.IsKeyDown(Keys.Left))
            {
                Position = new Vector2(Position.X - r_KeyboardVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds, Position.Y);
            }
            else if (currKeyboardState.IsKeyDown(Keys.Right))
            {
                Position = new Vector2(Position.X + r_KeyboardVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds, Position.Y);
            }

            // move the ship using the mouse:
            Position = new Vector2(Position.X + InputManager.GetMousePositionDelta().X, Position.Y);

            // clam the position between screen boundries:
            Position = new Vector2(MathHelper.Clamp(Position.X, 0, SpaceInvaders.graphics.GraphicsDevice.Viewport.Width - Texture.Width), Position.Y);

            // if we hit the wall, lets change direction:
            if (Position.X == 0 || Position.X == SpaceInvaders.graphics.GraphicsDevice.Viewport.Width - Texture.Width)
            {
                Direction *= -1f;
            }

            //update spaceship bullets position
            if (Gun.BulletsList.Count != 0)
            {
                foreach (Bullet element in Gun.BulletsList)
                {
                    element.Update(gameTime);
                }
            }
        }
        public Gun Gun
        {
            get{ return m_Gun; }
        }

        public SpaceShip(Game game):base(game)
        {
            m_AssetName = @"Sprites\Ship01_32x32";
            m_Direction = 1f;
        }
        public void Init()
        {

        }
        private void move()
        {
            
        }

        public void  Shoot()
        {

        }
        public void Draw()
        {
            m_SpriteBatch.Draw(Texture, Position, Color.White); //no tinting
        }
    }
}
