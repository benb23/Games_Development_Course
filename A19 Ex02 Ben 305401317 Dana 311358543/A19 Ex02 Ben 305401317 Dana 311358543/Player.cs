using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure;

namespace A19_Ex03_Ben_305401317_Dana_311358543
{
    public class Player : GameComponent
    {
        private const int k_NumOfSouls = 3;
        private GameScreen m_GameScreen;
        private IInputManager m_InputManager;
        private bool m_IsAllowedToUseMouse;
        private Keys m_RightMoveKey;
        private Keys m_LeftMoveKey;
        private Keys m_ShootKey;
        private PlayerIndex m_PlayerType;
        private SpaceShip m_SpaceShip;
        private List<Soul> m_Souls = new List<Soul>(k_NumOfSouls);
        private bool m_Initialized = false;
        private int m_Score;
        private int m_CurrentSoulsNumber;

        public void InitSouls()
        {
            this.m_CurrentSoulsNumber = k_NumOfSouls;
            foreach (Soul soul in this.m_Souls)
            {
                soul.Enabled = true;
                soul.Visible = true;
            }
        }

        public SpaceShip SpaceShip
        {
            get { return this.m_SpaceShip; }
        }

        public int CurrentSoulsNum
        {
            get { return this.m_CurrentSoulsNumber; }
        }

        public int Score
        {
            get { return this.m_Score; }
            set { this.m_Score = value; }
        }

        public Player(GameScreen i_GameScreen, PlayerIndex i_PlayerType, Keys i_LeftKey, Keys i_RightKey, Keys i_ShootKey, bool i_IsAllowdToUseMouse, Vector2 initialPosition)
            : base(i_GameScreen.Game)
        {
            this.m_Souls = new List<Soul>(k_NumOfSouls);
            this.m_LeftMoveKey = i_LeftKey;
            this.m_RightMoveKey = i_RightKey;
            this.m_ShootKey = i_ShootKey;
            this.m_IsAllowedToUseMouse = i_IsAllowdToUseMouse;
            this.m_PlayerType = i_PlayerType;
            this.m_GameScreen = i_GameScreen;
            this.createSpaceShip(i_PlayerType);
            this.createSouls();
            this.m_CurrentSoulsNumber = k_NumOfSouls;
            i_GameScreen.Add(this);
        }

        private void createSouls()
        {
            for (int i = 0; i < this.m_Souls.Capacity; i++)
            {
                this.m_Souls.Add(new Soul(this.m_GameScreen, new Vector2(0.5f), 0.5f, this.SpaceShip.AssetName, this.m_PlayerType, i));
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            if(!this.m_Initialized)
            {
                this.m_SpaceShip.Position = new Vector2(((int)this.m_PlayerType * this.m_SpaceShip.Texture.Width / 2) + this.m_SpaceShip.Texture.Width, this.Game.GraphicsDevice.Viewport.Height);
                this.m_Initialized = true;
            }

            if (this.m_IsAllowedToUseMouse)
            {
                this.moveSpaceShipUsingMouse(i_GameTime);
            }

            this.moveSpaceShipUsingKeyboard(i_GameTime, this.m_LeftMoveKey, this.m_RightMoveKey);

            if(this.isPlayerAskedToShoot(this.m_ShootKey) && this.m_SpaceShip.PermitionToShoot())
            {
                this.m_SpaceShip.Shoot();
            }

            this.m_SpaceShip.Position = new Vector2(
                        MathHelper.Clamp(this.m_SpaceShip.Position.X, this.m_SpaceShip.Texture.Width / 2, this.Game.GraphicsDevice.Viewport.Width - (this.m_SpaceShip.Texture.Width / 2)),
                        this.m_SpaceShip.Position.Y);
        }

        public void KillSoul(Player i_Player)
        {
            bool foundActiveSoul = false;

            foreach (Soul soul in this.m_Souls)
            {
                if (soul.Enabled)
                {
                    soul.Enabled = false;
                    soul.Visible = false;
                    foundActiveSoul = true;
                }

                if (foundActiveSoul)
                {
                    break;
                }
            }

            this.m_CurrentSoulsNumber--;
        }
     
        private bool isPlayerAskedToShoot(Keys i_shootKey)
        {
            bool isPlayerAskedToShoot = false;

            if (this.m_InputManager.KeyReleased(i_shootKey) || 
                (this.m_InputManager.ButtonReleased(eInputButtons.Left) && this.m_IsAllowedToUseMouse))
            {
                isPlayerAskedToShoot = true;
            }
            
            return isPlayerAskedToShoot;
        }

        private void moveSpaceShipUsingMouse(GameTime i_GameTime)
        {
            this.m_SpaceShip.Position = new Vector2(this.m_SpaceShip.Position.X + this.m_InputManager.MousePositionDelta.X, this.m_SpaceShip.Position.Y);
        }

        private void moveSpaceShipUsingKeyboard(GameTime i_GameTime, Keys i_LeftKey, Keys i_RightKey)
        {
            if (this.m_InputManager.KeyboardState.IsKeyDown(i_LeftKey))
            {
                this.m_SpaceShip.Position = new Vector2(this.m_SpaceShip.Position.X - (this.m_SpaceShip.Speed * (float)i_GameTime.ElapsedGameTime.TotalSeconds), this.m_SpaceShip.Position.Y);
            }
            else if (this.m_InputManager.KeyboardState.IsKeyDown(i_RightKey))
            {
                this.m_SpaceShip.Position = new Vector2(this.m_SpaceShip.Position.X + (this.m_SpaceShip.Speed * (float)i_GameTime.ElapsedGameTime.TotalSeconds), this.m_SpaceShip.Position.Y);
            }
        }

        public void initPlayerForForNextLevel()
        {
            this.m_SpaceShip.initSpaceShipForNextLevel();
            this.m_Initialized = false;
        }

        private void createSpaceShip(PlayerIndex i_PlayerType)
        {
            if(i_PlayerType == PlayerIndex.One)
            {
                this.m_SpaceShip = new SpaceShip(this.m_GameScreen, @"Sprites\Ship01_32x32", Bullet.eBulletType.PlayerOneBullet, PlayerIndex.One);
            }
            else
            {
                this.m_SpaceShip = new SpaceShip(this.m_GameScreen, @"Sprites\Ship02_32x32", Bullet.eBulletType.PlayerTwoBullet, PlayerIndex.Two);
            }
        }

        public void destroyed_Finished(object sender, EventArgs e)
        {
            this.SpaceShip.Enabled = false;
            this.SpaceShip.Visible = false;  
        }

        public override void Initialize()
        {
            if (this.m_InputManager == null)
            {
                this.m_InputManager = this.Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            }

            base.Initialize();
        }
    }
}
