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

        public SpaceShip(Game game) : base(game)
        {
            m_AssetName = @"Sprites\Ship01_32x32";
            m_Tint = Color.White;
        }

        public override void Update(GameTime i_GameTime)
        {
            moveUsingKeyboard(i_GameTime);
            moveUsingMouse();

            // clamp the position between screen boundries:
            Position = new Vector2(MathHelper.Clamp(Position.X, 0, GraphicsDevice.Viewport.Width - Texture.Width), Position.Y);
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

        // move to INPUTMANAGER??
        private void moveUsingMouse()
        {
            InputManager inputManger = Game.Services.GetService(typeof(InputManager)) as InputManager;
            Position = new Vector2(Position.X + inputManger.GetMousePositionDelta().X, Position.Y);
        }

        // move to INPUTMANAGER
        private void moveUsingKeyboard(GameTime i_GameTime)
        {
            KeyboardState currKeyboardState = Keyboard.GetState();

           
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
                currBullet = new Bullet(Game, Bullet.eBulletType.SpaceShipBullet, this);
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
