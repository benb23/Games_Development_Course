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
        private SpriteBatch spriteBatch;
        private Texture2D m_TextureBackground;
        private Vector2 m_PositionBackground;
        private Color m_TintBackground = Color.White;//??
        private SpaceShip m_SpaceShip;
        private MotherSpaceShip m_MotherSpaceShip;
        private EnemysGroup m_EnemysGroup;
        private Player m_Player;
        KeyboardState m_PastKey;//??


        public SpaceInvaders()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            m_SpaceShip = new SpaceShip();
            m_MotherSpaceShip = new MotherSpaceShip();
            m_EnemysGroup = new EnemysGroup();
            m_Player = new Player();


            base.Initialize();
        }

        private void InitPositions()
        {
            // 1. init the ship position
            // Get the bottom and left:
            float x = 0;
            float y = (float)GraphicsDevice.Viewport.Height;

            // Offset for ship start point:
            y -= (m_SpaceShip.Texture.Height * 1.5f);

            m_SpaceShip.Position = new Vector2(x, y);

            // 2. Init the enemy position

            // 3. Init the bg position:
            m_PositionBackground = Vector2.Zero;

            //create an alpah channel for background:
            Vector4 bgTint = Vector4.One;
            bgTint.W = 0.4f; // set the alpha component to 0.2
            m_TintBackground = new Color(bgTint);
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            m_TextureBackground = Content.Load<Texture2D>(@"Sprites\BG_Space01_1024x768");
            m_SpaceShip.Texture = Content.Load<Texture2D>(@"Sprites\Ship01_32x32");
            InitPositions();
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // get the current input devices state:
            KeyboardState currKeyboardState = Keyboard.GetState();

            // Allows the game to exit by GameButton 'back' button or Esc:
            if (currKeyboardState.IsKeyDown(Keys.Escape))   
            {
                //TODO: msg box
                this.Exit();
            }

            if(currKeyboardState.IsKeyDown(Keys.Enter) && m_PastKey.IsKeyUp(Keys.Enter))
            {
                m_SpaceShip.Shoot();
            }


            m_EnemysGroup.Update(gameTime);
            m_SpaceShip.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.Draw(m_TextureBackground, m_PositionBackground, m_TintBackground); // tinting with alpha channel
            spriteBatch.Draw(m_SpaceShip.Texture, m_SpaceShip.Position, Color.White); //no tinting
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
