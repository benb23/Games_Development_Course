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
        private const int k_NumOfPlayers = 2;
        private IGameEngine m_GameEngine;
        private IInputManager m_InputManager;
        SpriteBatch m_SpriteBatch;
        
        public const int k_MaxRandomNumber = 50000;
        private const string k_GameName = "Space Invaders";
        public static Random k_Random = new Random(); // TODO: Move to Engine
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
            Content.RootDirectory = "Content";
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);
            this.m_MotherSpaceShip = new MotherSpaceShip(this);


            m_Players = new List<Player>(2);
            m_Players.Add(new Player(this, PlayerIndex.One, Keys.H, Keys.K, Keys.U, true, new Vector2(0, 0)));
            m_Players.Add(new Player(this, PlayerIndex.Two, Keys.A, Keys.D, Keys.W, false, new Vector2(1, 0)));
            Components.Add(m_Players[0]);
            Components.Add(m_Players[1]);


            m_CollisionManager = new CollisionsManager(this);
            m_InputManager = new InputManager(this) as IInputManager;
            m_GameEngine = new GameEngine(this) as IGameEngine;
            m_GameEngine.Players = m_Players;

            

            
            this.m_EnemysGroup = new EnemiesGroup(this);
            Components.Add(this.m_EnemysGroup);

            this.m_WallsGroup = new WallsGroup(this, 4);
            Components.Add(this.m_WallsGroup);


            this.m_Graphics.IsFullScreen = false;
            this.m_Graphics.PreferredBackBufferWidth = 800;
            this.m_Graphics.PreferredBackBufferHeight = 600;
            this.m_Graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            base.LoadContent();


        }

        protected override void Initialize()
        {
            //Mouse.SetPosition((int)m_Players[0].SpaceShip.Position.X, GraphicsDevice.Viewport.Height);
            base.Initialize();
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), m_SpriteBatch);
            this.Window.Title = k_GameName;

            
            m_WallsGroup.WallsYShift = m_Graphics.GraphicsDevice.Viewport.Height - 2*m_Players[0].SpaceShip.Texture.Height;
            
        }

    }
}
