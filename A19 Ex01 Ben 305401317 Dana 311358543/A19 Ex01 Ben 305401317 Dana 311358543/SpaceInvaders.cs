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
        public static Random m_RandomNum = new Random();
        public static GraphicsDeviceManager graphics;
        private ScoreManager m_ScoreManager;
        private SpriteBatch m_SpriteBatch;
        private SpaceShip m_SpaceShip;
        private MotherSpaceShip m_MotherSpaceShip;
        private EnemiesGroup m_EnemysGroup;
        private Player m_Player;
        private Background m_Background;
        public static InputManager m_InputManager= new InputManager();


        public SpaceInvaders()
        {
            m_ScoreManager = new ScoreManager(this);
            this.Services.AddService(typeof(ScoreManager), m_ScoreManager);
            Components.Add(m_ScoreManager);
            m_Background = new Background(this);
            Components.Add(m_Background);
            graphics = new GraphicsDeviceManager(this);
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

        protected override void Initialize()
        {
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), m_SpriteBatch);
            
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 640;
            graphics.ApplyChanges();
            base.Initialize();
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // get the current input devices state:
            KeyboardState currKeyboardState = Keyboard.GetState();
            MouseState currMouseState = Mouse.GetState();

            // Allows the game to exit by GameButton 'back' button or Esc:
            if (currKeyboardState.IsKeyDown(Keys.Escape))   
            {
                //TODO: msg box
                this.Exit();
                
            }

            if (m_SpaceShip.CountNumOfVisibleBullets() < SpaceShip.r_MaxNumOfBullets)
            {
                if (m_InputManager.IsShootingOrder())
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
