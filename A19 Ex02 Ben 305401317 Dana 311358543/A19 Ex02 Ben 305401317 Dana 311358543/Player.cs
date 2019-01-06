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
        Game m_Game;

        private IInputManager m_InputManager;

        bool m_IsAllowedToUseMouse;

        Keys m_RightMoveKey;

        Keys m_LeftMoveKey;

        Keys m_ShootKey;

        PlayerIndex m_PlayerType;

        private SpaceShip m_SpaceShip;

        private List<Soul> m_Souls = new List<Soul>(3);

        public List<Soul> Souls
        {
            get { return m_Souls; }
        }

        public SpaceShip SpaceShip
        {
            get{ return m_SpaceShip; }
        }

        private int m_Score;

        private int m_CurrentSoulsNumber;

        private int CurrentSoulsNum
        {
            get { return m_CurrentSoulsNumber; }
        }

        public int Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }

        //TODO: SHOOT FROM PLAYER
        public override void Update(GameTime i_GameTime)
        {
            if (m_IsAllowedToUseMouse)
            {
                moveSpaceShipUsingMouse(i_GameTime);
            }

            moveSpaceShipUsingKeyboard(i_GameTime, m_LeftMoveKey, m_RightMoveKey);

            if(isPlayerAskedToShoot(m_ShootKey) && m_SpaceShip.PermitionToShoot())
            {
                m_SpaceShip.Shoot();
            }

            m_CurrentSoulsNumber = m_Souls.Count;
            m_SpaceShip.Position = new Vector2(MathHelper.Clamp(m_SpaceShip.Position.X, m_SpaceShip.Texture.Width / 2, m_Game.GraphicsDevice.Viewport.Width -m_SpaceShip.Texture.Width/2),m_SpaceShip.Position.Y);
        }

        private bool isPlayerAskedToShoot(Keys i_shootKey)
        {
            bool isPlayerAskedToShoot = false;

            if (m_InputManager.KeyReleased(i_shootKey) || 
                (m_InputManager.MouseState.LeftButton.Equals(ButtonState.Pressed) && m_InputManager.MouseState.LeftButton.Equals(ButtonState.Released) && m_IsAllowedToUseMouse))
            {//TODO : function for mouse .
                isPlayerAskedToShoot = true;
            }
            
            return isPlayerAskedToShoot;
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

        public Player(Game i_Game,PlayerIndex i_PlayerType, Keys i_LeftKey, Keys i_RightKey, Keys i_ShootKey,
                      bool i_IsAllowdToUseMouse, Vector2 initialPosition) : base(i_Game)
        {
            m_LeftMoveKey = i_LeftKey;
            m_RightMoveKey = i_RightKey;
            m_ShootKey = i_ShootKey;
            m_IsAllowedToUseMouse = i_IsAllowdToUseMouse;
            m_PlayerType = i_PlayerType;
            m_Game = i_Game;
            createSpaceShip(i_PlayerType);
            m_CurrentSoulsNumber = m_Souls.Capacity;
        }

        private void createSpaceShip(PlayerIndex i_PlayerType)
        {
            if(i_PlayerType == PlayerIndex.One)
            {
                m_SpaceShip = new SpaceShip(m_Game, @"Sprites\Ship01_32x32", Bullet.eBulletType.PlayerOneBullet, PlayerIndex.One);
            }
            else
            {
                m_SpaceShip = new SpaceShip(m_Game, @"Sprites\Ship02_32x32", Bullet.eBulletType.PlayerTwoBullet, PlayerIndex.Two);
            }
        }

        public void destroyed_Finished(object sender, EventArgs e)
        {
            Enabled = false;
            SpaceShip.Enabled = false;
            SpaceShip.Visible = false;
        }

        public override void Initialize()
        {
            for (int i = 0; i < m_Souls.Capacity; i++)
            {
                this.m_Souls.Add(new Soul(Game, new Vector2(0.5f), 0.5f, SpaceShip.AssetName, m_PlayerType, i));
            }

            if (m_InputManager == null)
            {
                m_InputManager = Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            }

            m_SpaceShip.Position = new Vector2(((int)m_PlayerType*m_SpaceShip.Texture.Width/2)+ m_SpaceShip.Texture.Width, m_Game.GraphicsDevice.Viewport.Height);
            base.Initialize();
        }

    }
}
