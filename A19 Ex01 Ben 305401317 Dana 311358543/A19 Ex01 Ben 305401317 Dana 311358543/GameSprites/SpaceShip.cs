﻿using System;
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
        public static readonly int r_MaxNumOfBullets=3;
        private List<Bullet> m_BulletList = new List<Bullet>(3);
        private readonly float r_KeyboardVelocity = 120;//TODO: ctor?
        private Gun m_Gun = new Gun();//TODO: ctor?

        public override void Update(GameTime gameTime)
        {
            moveUsingKeyboard(gameTime);
            moveUsingMouse();

            // clam the position between screen boundries:
            Position = new Vector2(MathHelper.Clamp(Position.X, 0, SpaceInvaders.graphics.GraphicsDevice.Viewport.Width - Texture.Width), Position.Y);

            // if we hit the wall, lets change direction:
            //if (Position.X == 0 || Position.X == SpaceInvaders.graphics.GraphicsDevice.Viewport.Width - Texture.Width)
            //{
            //    Direction *= -1f;
            //}

            //update spaceship bullets position
            if (Gun.BulletsList.Count != 0)
            {
                foreach (Bullet element in Gun.BulletsList)
                {
                    element.Update(gameTime);
                }
            }
        }

        public int CountNumOfVisibleBullets()
        {
            int numOfVisibleBullets = 0;
            
            foreach (Bullet element in m_BulletList)
            {
                if(element.m_visible==true)
                {
                    numOfVisibleBullets++;
                }
            }

            return numOfVisibleBullets;
        }

        private void moveUsingMouse()
        {
            Position = new Vector2(Position.X +SpaceInvaders.m_InputManager.GetMousePositionDelta().X, Position.Y);
        }

        private void moveUsingKeyboard(GameTime i_GameTime)
        {
            KeyboardState currKeyboardState = Keyboard.GetState();

            // move the ship using the keyboard:
            if (currKeyboardState.IsKeyDown(Keys.Left))
            {
                Position = new Vector2(Position.X - r_KeyboardVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds, Position.Y);
            }
            else if (currKeyboardState.IsKeyDown(Keys.Right))
            {
                Position = new Vector2(Position.X + r_KeyboardVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds, Position.Y);
            }
        }

        public Gun Gun
        {
            get{ return m_Gun; }
        }

        public SpaceShip(Game game) : base(game)
        {
            m_AssetName = @"Sprites\Ship01_32x32";
            m_Tint = Color.White;
        }
        public override void initPosition()
        {
            m_Position.X = 0f;
            m_Position.Y = Game.GraphicsDevice.Viewport.Height - (Texture.Height * 1.2f );

        }

        public void  Shoot(Game i_game)
        {
            Bullet currBullet;

            currBullet = getBullet(i_game);
            m_Gun.Shoot(currBullet);
        }

        private Bullet getBullet(Game i_game)
        {
                Bullet currBullet;

                if (m_BulletList.Count < r_MaxNumOfBullets)
                {
                    currBullet = new Bullet(i_game, Bullet.BulletType.SpaceShipBullet, m_Position);
                    m_BulletList.Add(currBullet);
                }
                else
                {
                    currBullet = getUnVisibleBulletFromList();
                    currBullet.m_visible = true;
                    currBullet.initBulletPosition(Position);
                }
            return currBullet;
        }

        private Bullet getUnVisibleBulletFromList()
        {
            Bullet bullet = null;

            foreach (Bullet element in m_BulletList)
            {
                if (element.m_visible == false)
                {
                    bullet = element;
                }
            }

            return bullet;
        }
        //public void Draw()
        //{
        //    m_SpriteBatch.Draw(Texture, Position, Color.White); //no tinting
        //}
    }
}
