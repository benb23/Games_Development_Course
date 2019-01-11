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
    public class SpaceInvaders : Game
    {
        public const int k_MaxRandomNumber = 50000;
        private const string k_GameName = "Space Invaders";
        private const int k_NumOfPlayers = 2;
        private const int k_NumOfWalls = 4;
        private ISpaceInvadersEngine m_GameEngine;
        private IInputManager m_InputManager;
        private SpriteBatch m_SpriteBatch;
        public Random m_Random = new Random(); 
        private GraphicsDeviceManager m_Graphics;
        private MotherSpaceShip m_MotherSpaceShip;
        private EnemiesGroup m_EnemysGroup;
        private Background m_Background;
        private List<Player> m_Players;
        private CollisionsManager m_CollisionManager;
        private WallsGroup m_WallsGroup;

        public SpaceInvaders()
        {
            this.IsMouseVisible = true;
            this.m_Graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);
            this.m_MotherSpaceShip = new MotherSpaceShip(this);
            this.m_Players = new List<Player>(k_NumOfPlayers);
            this.m_Players.Add(new Player(this, PlayerIndex.One, Keys.H, Keys.K, Keys.U, true, new Vector2(0, 0)));
            this.m_Players.Add(new Player(this, PlayerIndex.Two, Keys.A, Keys.D, Keys.W, false, new Vector2(1, 0)));
            this.m_CollisionManager = new CollisionsManager(this);
            this.m_InputManager = new InputManager(this);
            this.m_GameEngine = new SpaceInvadersEngine(this);
            this.m_GameEngine.Players = this.m_Players;
            this.m_EnemysGroup = new EnemiesGroup(this);
            this.m_WallsGroup = new WallsGroup(this, k_NumOfWalls);
            this.m_Graphics.IsFullScreen = false;
            this.m_Graphics.PreferredBackBufferWidth = 800;
            this.m_Graphics.PreferredBackBufferHeight = 600;
            this.m_Graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            this.m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), this.m_SpriteBatch);
            this.Services.AddService(typeof(Random), this.m_Random);
            this.Window.Title = k_GameName;
            base.Initialize();

            this.m_WallsGroup.WallsYShift = this.m_Graphics.GraphicsDevice.Viewport.Height - (2 * this.m_Players[(int)PlayerIndex.One].SpaceShip.Texture.Height);            
        }

        protected override void Draw(GameTime gameTime)
        {
            this.m_Graphics.GraphicsDevice.Clear(Color.Black);

            this.m_SpriteBatch.Begin();
            base.Draw(gameTime);
            this.m_SpriteBatch.End();
        }
    }
}
