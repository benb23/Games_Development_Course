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
    public class Player : GameComponent, IScoreable
    {
        public enum ePlayer //change the direction
        {
            PlayerOne,
            PlayerTwo,
        }

        Game m_Game;

        public const int k_MaxNumOfBullets = 3;

        private List<Bullet> m_BulletList = new List<Bullet>(k_MaxNumOfBullets);

        private IInputManager m_InputManager;

        Bullet.eBulletType m_BulletsType;

        ePlayer m_PlayerType;

        private Gun m_Gun;
        
        private SpaceShip m_SpaceShip;

        private List<Soul> m_Souls;

        private int m_Score;

        public SpaceShip SpaceShip
        {
            get { return m_SpaceShip; }
        }

        public override void Update(GameTime i_GameTime)
        {        
            if (m_PlayerType == ePlayer.PlayerOne)
            {
                moveSpaceShipUsingMouse(i_GameTime);
                moveSpaceShipUsingKeyboard(i_GameTime, Keys.H, Keys.K);
            }
            else if(m_PlayerType == ePlayer.PlayerTwo)
            {
                moveSpaceShipUsingKeyboard(i_GameTime, Keys.A, Keys.D);
            }

            m_SpaceShip.Position = new Vector2(MathHelper.Clamp(m_SpaceShip.Position.X, m_SpaceShip.Texture.Width / 2, m_Game.GraphicsDevice.Viewport.Width -m_SpaceShip.Texture.Width/2),m_SpaceShip.Position.Y);
        }

        private void moveSpaceShipUsingMouse(GameTime i_GameTime)
        {
            m_SpaceShip.Position = new Vector2(m_SpaceShip.Position.X + m_InputManager.MousePositionDelta.X, m_SpaceShip.Position.Y);
        }

        private void moveSpaceShipUsingKeyboard(GameTime i_GameTime, Keys i_LeftKey, Keys i_RightKey)
        {
            if (m_InputManager.KeyboardState.IsKeyDown(i_LeftKey))
            {
                m_SpaceShip.Position = new Vector2(m_SpaceShip.Position.X - m_SpaceShip.Speed * (float)i_GameTime.ElapsedGameTime.TotalSeconds, m_SpaceShip.Position.Y);
            }
            else if (m_InputManager.KeyboardState.IsKeyDown(i_RightKey))
            {
                m_SpaceShip.Position = new Vector2(m_SpaceShip.Position.X + m_SpaceShip.Speed * (float)i_GameTime.ElapsedGameTime.TotalSeconds, m_SpaceShip.Position.Y);
            }
        }

        public int CountNumOfVisibleBullets()
        {
            int numOfVisibleBullets = 0;

            foreach (Bullet element in this.m_BulletList)
            {
                if (element.Visible)
                {
                    numOfVisibleBullets++;
                }
            }

            return numOfVisibleBullets;
        }

        public Player(Game i_Game,ePlayer i_PlayerType): base(i_Game)
        {
            m_PlayerType = i_PlayerType;
            if (m_InputManager == null)
            {
                m_InputManager = Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            }

            m_Gun = new Gun();
            m_Souls = new List<Soul>(3);
            m_SpaceShip = new SpaceShip(i_Game);

            m_Game = i_Game;

            if(i_PlayerType==ePlayer.PlayerOne)
            {
                m_BulletsType = Bullet.eBulletType.PlayerOneBullet;
            }
            else
            {
                m_BulletsType = Bullet.eBulletType.PlayerTwoBullet;
            }
        }

        public void Shoot()
        {
            Bullet currBullet;
            currBullet = this.getBullet();
            currBullet.Position = new Vector2(m_SpaceShip.Position.X, m_SpaceShip.Position.Y - m_SpaceShip.Texture.Height);
            currBullet.Visible = true;

            this.m_Gun.Shoot(currBullet, m_Game);
        }

        public override void Initialize()
        {
            if(m_PlayerType==ePlayer.PlayerOne)
            {
                m_SpaceShip.Position = new Vector2( m_SpaceShip.Texture.Width, m_Game.GraphicsDevice.Viewport.Height);
            }
            else if(m_PlayerType == ePlayer.PlayerTwo)
            {
                m_SpaceShip.Position = new Vector2(2*m_SpaceShip.Texture.Width, m_Game.GraphicsDevice.Viewport.Height);
            }
            base.Initialize();
        }
        private Bullet getBullet()
        {
            Bullet bullet = null;
            bool bulletfound = false;

            if(m_BulletList.Count>0)
            {
                foreach(Bullet currbullet in m_BulletList)
                {
                    if(!currbullet.Visible)
                    {
                        bullet = currbullet;
                        bullet.Visible = true;
                        bullet.AddComponent();
                        bulletfound = true;
                        break;
                    }
                }
            }

            if(!bulletfound && m_BulletList.Count< k_MaxNumOfBullets)
            {
                bullet = new Bullet(m_Game, m_BulletsType);
                m_BulletList.Add(bullet);
            }

            return bullet;
        }

        public bool IsFreeBulletExists()
        {
            bool IsFreeBulletExists = false;

            if (m_BulletList.Count < 3)
            {
                IsFreeBulletExists = true;
            }
            else
            {
                foreach (Bullet bullet in m_BulletList)
                {
                    if (bullet.Visible == false)
                    {
                        IsFreeBulletExists = true;
                        break;
                    }
                }
            }

            return IsFreeBulletExists;
        }

        public int Score { get; set; }
    }
}
