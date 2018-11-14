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
        public static GraphicsDeviceManager graphics;
        private SpriteBatch m_SpriteBatch;
        private SpaceShip m_SpaceShip;
        private MotherSpaceShip m_MotherSpaceShip;
        private EnemiesGroup m_EnemysGroup;
        private Player m_Player;
        private Background m_Background;
        KeyboardState m_PastKey;//??
        MouseState m_pastMouseState;

        public SpaceInvaders()
        {
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
            base.Initialize();
        }

        private void InitPositions()
        {
            // 1. init the ship position
            // Get the bottom and left:
            

            // 2. Init the enemy position

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

            if(((currKeyboardState.IsKeyDown(Keys.Enter) && m_PastKey.IsKeyUp(Keys.Enter)) || (currMouseState.LeftButton.Equals(ButtonState.Pressed)&& m_pastMouseState.LeftButton.Equals(ButtonState.Pressed)))&& m_SpaceShip.CountNumOfVisibleBullets()<3)
            {
                m_SpaceShip.Shoot(this);
            }

            m_PastKey = Keyboard.GetState();
            m_pastMouseState = Mouse.GetState();

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();

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
