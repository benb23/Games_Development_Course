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
    public class SpaceShip : Sprite
    {
        private const float k_KeyboardVelocity = 120;
        public const int k_MaxNumOfBullets = 3;
        private List<Bullet> m_BulletList = new List<Bullet>(3);

        private Gun m_Gun;

        public SpaceShip(Game i_Game) : base(i_Game)
        {
            this.m_AssetName = @"Sprites\Ship01_32x32";
            this.m_Tint = Color.White;
            this.m_Gun = new Gun();
        }

        public override void Update(GameTime i_GameTime)
        {
            this.moveUsingKeyboard(i_GameTime);
            this.moveUsingMouse();

            // clamp the position between screen boundries:
            this.Position = new Vector2(MathHelper.Clamp(Position.X, 0, GraphicsDevice.Viewport.Width - Texture.Width), Position.Y);
        }

        public int CountNumOfVisibleBullets()
        {
            int numOfVisibleBullets = 0;
            
            foreach (Bullet element in this.m_BulletList)
            {
                if(element.Visible)
                {
                    numOfVisibleBullets++;
                }
            }

            return numOfVisibleBullets;
        }

        // move to INPUTMANAGER??
        private void moveUsingMouse()
        {
            InputManager inputManger = SpaceInvaders.s_GameUtils.InputManager;
            this.Position = new Vector2(this.Position.X + inputManger.GetMousePositionDelta().X, Position.Y);
        }

        // move to INPUTMANAGER
        private void moveUsingKeyboard(GameTime i_GameTime)
        {
            KeyboardState currKeyboardState = Keyboard.GetState();

            if (currKeyboardState.IsKeyDown(Keys.Left))
            {
                this.Position = new Vector2(this.Position.X - (k_KeyboardVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds), this.Position.Y);
            }
            else if (currKeyboardState.IsKeyDown(Keys.Right))
            {
                this.Position = new Vector2(this.Position.X + (k_KeyboardVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds), this.Position.Y);
            }
        }

        public Gun Gun
        {
            get
            {
                return this.m_Gun;
            }
        }

        public override void InitPosition()
        {
            m_Position.X = 0f;
            m_Position.Y = Game.GraphicsDevice.Viewport.Height - (Texture.Height * 1.2f );
        }

        public void Shoot()
        {
            Bullet currBullet;

            currBullet = this.getBullet();
            this.m_Gun.Shoot(currBullet);
        }

        private Bullet getBullet()
        {
            Bullet currBullet;

            if (this.m_BulletList.Count < k_MaxNumOfBullets)
            {
                currBullet = new Bullet(Game, Bullet.eBulletType.SpaceShipBullet, this);
                this.m_BulletList.Add(currBullet);
            }
            else
            {
                currBullet = this.getUnVisibleBulletFromList();
                currBullet.Visible = true;
                currBullet.initBulletPosition(this);
            }

            return currBullet;
        }

        private Bullet getUnVisibleBulletFromList()
        {
            Bullet bullet = null;

            foreach (Bullet element in this.m_BulletList)
            {
                if (!element.Visible)
                {
                    bullet = element;
                }
            }

            return bullet;
        }
    }
}
