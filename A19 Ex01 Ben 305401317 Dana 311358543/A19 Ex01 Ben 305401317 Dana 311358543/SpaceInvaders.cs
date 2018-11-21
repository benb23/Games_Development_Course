using System;
using System.Windows.Forms;
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
    public class SpaceInvaders : Game
    {
        public static GameUtils s_GameUtils;
        public static Random s_RandomNum;
        private GraphicsDeviceManager m_Graphics;
        private SpaceShip m_SpaceShip;
        private MotherSpaceShip m_MotherSpaceShip;
        private EnemiesGroup m_EnemysGroup;
        private Background m_Background;

        public SpaceInvaders()
        {
            s_RandomNum = new Random();
            s_GameUtils = new GameUtils();
            s_GameUtils.ShootingManager = new ShootingManager(this);
            s_GameUtils.ScoreManager = new ScoreManager(this);
            Components.Add(s_GameUtils.ScoreManager);
            s_GameUtils.InputManager = new InputManager();
            this.m_Background = new Background(this);
            Components.Add(this.m_Background);
            this.m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.m_SpaceShip = new SpaceShip(this);
            Components.Add(this.m_SpaceShip);
            this.m_MotherSpaceShip = new MotherSpaceShip(this);
            Components.Add(this.m_MotherSpaceShip);
            this.m_EnemysGroup = new EnemiesGroup(this);
            Components.Add(this.m_EnemysGroup);
            this.IsMouseVisible = true;
        }

        private bool isSpaceShipAllowedToShoot()
        {
            bool isAllowed = this.m_SpaceShip.CountNumOfVisibleBullets() < SpaceShip.k_MaxNumOfBullets;

            return isAllowed;
        }

        protected override void Initialize()
        {
            s_GameUtils.SpriteBatch = new SpriteBatch(GraphicsDevice);

            this.m_Graphics.IsFullScreen = false;
            this.m_Graphics.PreferredBackBufferWidth = 800;
            this.m_Graphics.PreferredBackBufferHeight = 640;
            this.m_Graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (s_GameUtils.InputManager.isUserAskedToExit())   
            {
                this.Exit();
            }

            if (this.isSpaceShipAllowedToShoot())
            {
                if (s_GameUtils.InputManager.IsUserAskedToShoot())
                {
                    this.m_SpaceShip.Shoot();
                }
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            s_GameUtils.SpriteBatch.Begin();
            base.Draw(gameTime);
            s_GameUtils.SpriteBatch.End();
        }
    }
}
