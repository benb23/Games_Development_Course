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
    public class SpaceInvaders : Game
    {
        public static Random m_RandomNum;
        private InputManager m_InputManager;
        private ShootingManager m_CollisionManager;
        private GraphicsDeviceManager m_Graphics;
        private ScoreManager m_ScoreManager;
        private SpriteBatch m_SpriteBatch;
        private SpaceShip m_SpaceShip;
        private MotherSpaceShip m_MotherSpaceShip;
        private EnemiesGroup m_EnemysGroup;
        private Player m_Player; //??
        private Background m_Background;

        public SpaceInvaders()
        {
            m_RandomNum = new Random();
            m_CollisionManager = new ShootingManager(this);
            this.Services.AddService(typeof(ShootingManager), m_CollisionManager);
            m_ScoreManager = new ScoreManager(this);
            this.Services.AddService(typeof(ScoreManager), m_ScoreManager);
            Components.Add(m_ScoreManager);
            m_InputManager = new InputManager();
            this.Services.AddService(typeof(InputManager), m_InputManager);
            m_Background = new Background(this);
            Components.Add(m_Background);
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            m_SpaceShip = new SpaceShip(this);
            Components.Add(m_SpaceShip);
            m_MotherSpaceShip = new MotherSpaceShip(this);
            Components.Add(m_MotherSpaceShip);
            m_EnemysGroup = new EnemiesGroup(this);
            Components.Add(m_EnemysGroup);
            m_Player = new Player();
            this.IsMouseVisible = true;
        }

        private bool isSpaceShipAllowedToShoot()
        {
            bool isAllowed = m_SpaceShip.CountNumOfVisibleBullets() < SpaceShip.r_MaxNumOfBullets;

            return isAllowed;
        }

        protected override void Initialize()
        {
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), m_SpriteBatch);

            m_Graphics.IsFullScreen = false;
            m_Graphics.PreferredBackBufferWidth = 800;
            m_Graphics.PreferredBackBufferHeight = 640;
            m_Graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit by GameButton 'back' button or Esc:
            if (m_InputManager.isUserAskedToExit())   
            {
                //TODO: msg box
                this.Exit();
            }

            if (isSpaceShipAllowedToShoot())
            {
                if (m_InputManager.IsUserAskedToShoot())
                {
                    m_SpaceShip.Shoot();
                }
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            m_SpriteBatch.Begin();
            base.Draw(gameTime);
            m_SpriteBatch.End();
        }
    }
}
