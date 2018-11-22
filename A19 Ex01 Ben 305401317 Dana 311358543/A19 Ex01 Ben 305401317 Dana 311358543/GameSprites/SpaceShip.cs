using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace A19_Ex01_Ben_305401317_Dana_311358543
{
    public class SpaceShip : Sprite
    {
        private const float k_KeyboardVelocity = 120;
        public const int k_MaxNumOfBullets = 3;
        private List<Bullet> m_BulletList = new List<Bullet>(k_MaxNumOfBullets);

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

        private void moveUsingMouse()
        {
            this.Position = new Vector2(this.Position.X + SpaceInvaders.s_GameUtils.InputManager.GetMousePositionDelta().X, Position.Y);
        }

        private void moveUsingKeyboard(GameTime i_GameTime)
        {
            if (SpaceInvaders.s_GameUtils.InputManager.IsUserAskedToMoveLeft())
            {
                this.Position = new Vector2(this.Position.X - (k_KeyboardVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds), this.Position.Y);
            }
            else if (SpaceInvaders.s_GameUtils.InputManager.IsUserAskedToMoveRight())
            {
                this.Position = new Vector2(this.Position.X + (k_KeyboardVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds), this.Position.Y);
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
                currBullet.InitBulletPosition(this);
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
