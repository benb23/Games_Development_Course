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
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D m_TextureBackground;
        private Vector2 m_PositionBackground;
        private Color m_TintBackground = Color.White;
        private SpaceShip m_spaceShip;
        private MotherSpaceShip m_motherSpaceShip;
        private EnemysGroup m_enemysGroup;

        public SpaceInvaders()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.IsMouseVisible = true;
        }

        
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        //protected override void Initialize()
        //{
        //    // TODO: Add your initialization logic here
        //}


        private void InitPositions()
        {
            // 1. init the ship position
            // Get the bottom and left:
            float x = 0;
            float y = (float)GraphicsDevice.Viewport.Height;

            // Offset for ship start point:
            y -= (m_TextureShip.Height * 1.5f);

            
            m_PositionShip = new Vector2(x, y);


            // 2. Init the enemy position

            // 3. Init the bg position:
            m_PositionBackground = Vector2.Zero;

            //create an alpah channel for background:
            Vector4 bgTint = Vector4.One;
            bgTint.W = 0.4f; // set the alpha component to 0.2
            m_TintBackground = new Color(bgTint);
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            m_TextureBackground = Content.Load<Texture2D>(@"Sprites\BG_Space01_1024x768");
            m_TextureShip = Content.Load<Texture2D>(@"Sprites\Ship01_32x32");
            InitPositions();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        MouseState? m_PrevMouseState;

        private Vector2 GetMousePositionDelta()
        {
            Vector2 retVal = Vector2.Zero;

            MouseState currState = Mouse.GetState();

            if (m_PrevMouseState != null)
            {
                retVal.X = (currState.X - m_PrevMouseState.Value.X);
                retVal.Y = (currState.Y - m_PrevMouseState.Value.Y);
            }

            m_PrevMouseState = currState;

            return retVal;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // get the current input devices state:
            GamePadState currGamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState currKeyboardState = Keyboard.GetState();

            // Allows the game to exit by GameButton 'back' button or Esc:
            if (currGamePadState.Buttons.Back == ButtonState.Pressed
                || currKeyboardState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            // move the ship using the GamePad left thumb stick and set viberation according to movement:
            m_PositionShip.X += currGamePadState.ThumbSticks.Left.X * 120 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            GamePad.SetVibration(PlayerIndex.One, 0, Math.Abs(currGamePadState.ThumbSticks.Left.X));

            // move the ship using the mouse:
            m_PositionShip.X += GetMousePositionDelta().X;

            // clam the position between screen boundries:
            m_PositionShip.X = MathHelper.Clamp(m_PositionShip.X, 0, this.GraphicsDevice.Viewport.Width - m_TextureShip.Width);

            // if we hit the wall, lets change direction:
            if (m_PositionShip.X == 0 || m_PositionShip.X == this.GraphicsDevice.Viewport.Width - m_TextureShip.Width)
            {
                m_ShipDirection *= -1f;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.Draw(m_TextureBackground, m_PositionBackground, m_TintBackground); // tinting with alpha channel
            spriteBatch.Draw(m_TextureShip, m_PositionShip, Color.White); //no tinting
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
