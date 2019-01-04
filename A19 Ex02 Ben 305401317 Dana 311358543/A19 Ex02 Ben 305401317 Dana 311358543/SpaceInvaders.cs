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
        private GameEngine m_GameEngine;
        private InputManager m_InputManager;
        SpriteBatch m_SpriteBatch;
        public static Random s_RandomNum;
        public const int k_MaxRandomNumber = 50000;
        private const string k_GameName = "Space Invaders";
        private GraphicsDeviceManager m_Graphics;
        private MotherSpaceShip m_MotherSpaceShip;
        private EnemiesGroup m_EnemysGroup;
        private Background m_Background;
        private List<Player> m_Players;
        private CollisionsManager m_CollisionManager;

        public SpaceInvaders()
        {
            m_CollisionManager = new CollisionsManager(this);
            s_RandomNum = new Random();
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);
            this.m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.m_MotherSpaceShip = new MotherSpaceShip(this);
            this.m_EnemysGroup = new EnemiesGroup(this);
            Components.Add(this.m_EnemysGroup);
            this.IsMouseVisible = true;
            m_InputManager = new InputManager(this);
            m_Players = new List<Player>(2);
            m_Players.Add(new Player(this, PlayerIndex.One));
            m_Players.Add(new Player(this, PlayerIndex.Two));
            Components.Add(m_Players[0]);
            Components.Add(m_Players[1]);
            m_GameEngine = new GameEngine(this);
            m_GameEngine.Players = m_Players;

        }

        protected override void Initialize()
        {
            Mouse.SetPosition((int)m_Players[0].SpaceShip.Position.X, GraphicsDevice.Viewport.Height);
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), m_SpriteBatch);
            this.Window.Title = k_GameName;
            this.m_Graphics.IsFullScreen = false;
            this.m_Graphics.PreferredBackBufferWidth = 800;
            this.m_Graphics.PreferredBackBufferHeight = 600;
            this.m_Graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void Update(GameTime i_GameTime)
        {
            if (IsPlayerAskToExit())
            {
                this.Exit();
            }

            base.Update(i_GameTime);
        }

        protected override void Draw(GameTime i_GameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            m_SpriteBatch.Begin();
            base.Draw(i_GameTime);
            m_SpriteBatch.End();
        }

        public bool IsPlayerAskToExit()
        {
            bool IsPlayerAskToExit;

            if (m_InputManager.KeyboardState.IsKeyDown(Keys.Escape))
            {
                IsPlayerAskToExit = true;
            }
            else
            {
                IsPlayerAskToExit = false;
            }
            return IsPlayerAskToExit;
        }

    }
}
