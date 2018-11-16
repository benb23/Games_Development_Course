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
        }

        public int CountNumOfVisibleBullets()
        {
            int numOfVisibleBullets = 0;
            
            foreach (Bullet element in m_BulletList)
            {
                if(element.Visible==true)
                {
                    numOfVisibleBullets++;
                }
            }

            return numOfVisibleBullets;
        }

        public void RemoveSoul()
        {
            ScoreManager scoreManager = Game.Services.GetService(typeof(ScoreManager)) as ScoreManager;
            if (scoreManager.Souls.Count-1 == 0)
            {
                Visible = false;
                Dispose();
                RemoveComponent();
                Game.Exit();
            }
            else
            {
                scoreManager.UpdateScoreAfterCollision(this);
            }
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

        public void  Shoot()
        {
            Bullet currBullet;

            currBullet = getBullet();
            m_Gun.Shoot(currBullet);
        }

        private Bullet getBullet()
        {
                Bullet currBullet;

                if (m_BulletList.Count < r_MaxNumOfBullets)
                {
                    currBullet = new Bullet(Game, Bullet.BulletType.SpaceShipBullet, this);
                    m_BulletList.Add(currBullet);
                }
                else
                {
                    currBullet = getUnVisibleBulletFromList();
                    currBullet.Visible = true;
                    currBullet.initBulletPosition(this);
                }
            return currBullet;
        }

        private Bullet getUnVisibleBulletFromList()
        {
            Bullet bullet = null;

            foreach (Bullet element in m_BulletList)
            {
                if (element.Visible == false)
                {
                    bullet = element;
                }
            }

            return bullet;
        }

    }
}
