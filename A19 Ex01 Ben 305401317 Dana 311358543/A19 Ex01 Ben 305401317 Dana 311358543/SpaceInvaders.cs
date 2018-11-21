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
        public static Random m_RandomNum;
        private InputManager m_InputManager;
        private ShootingManager m_CollisionManager;
        private GraphicsDeviceManager m_Graphics;
        private ScoreManager m_ScoreManager;
        private SpriteBatch m_SpriteBatch;
        private SpaceShip m_SpaceShip;
        private MotherSpaceShip m_MotherSpaceShip;
        private EnemiesGroup m_EnemysGroup;
        private Background m_Background;
        // private bool isGameOver = false;

        public SpaceInvaders()
        {
            m_RandomNum = new Random();
            this.m_CollisionManager = new ShootingManager(this);
            this.Services.AddService(typeof(ShootingManager), this.m_CollisionManager);
            this.m_ScoreManager = new ScoreManager(this);
            this.Services.AddService(typeof(ScoreManager), this.m_ScoreManager);
            Components.Add(this.m_ScoreManager);
            this.m_InputManager = new InputManager();
            this.Services.AddService(typeof(InputManager), this.m_InputManager);
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
            bool isAllowed = this.m_SpaceShip.CountNumOfVisibleBullets() < SpaceShip.r_MaxNumOfBullets;

            return isAllowed;
        }

        protected override void Initialize()
        {
            this.m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), this.m_SpriteBatch);

            this.m_Graphics.IsFullScreen = false;
            this.m_Graphics.PreferredBackBufferWidth = 800;
            this.m_Graphics.PreferredBackBufferHeight = 640;
            this.m_Graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit by GameButton 'back' button or Esc:
            if (this.m_InputManager.isUserAskedToExit())   
            {
                System.Windows.Forms.MessageBox.Show(string.Format(
@"Game Over
Youre score is: {0}",
this.m_ScoreManager.Score ));
                this.Exit();
            }

            if (this.isSpaceShipAllowedToShoot())
            {
                if (this.m_InputManager.IsUserAskedToShoot())
                {
                    this.m_SpaceShip.Shoot();
                }
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            this.m_SpriteBatch.Begin();
            base.Draw(gameTime);
            this.m_SpriteBatch.End();
        }

        private void gameOver()
        {
            System.Windows.Forms.MessageBox.Show(string.Format(
@"Game Over
Youre score is: {0}",
this.m_ScoreManager.Score ));
            this.Exit();
        }
    }
}
