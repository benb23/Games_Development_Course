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
        private GameInputManager m_InputManager;
        SpriteBatch m_SpriteBatch;
        public static Random s_RandomNum;
        public const int k_MaxRandomNumber = 50000;
        private const string k_GameName = "Space Invaders";
        private GraphicsDeviceManager m_Graphics;
        private MotherSpaceShip m_MotherSpaceShip;
        private EnemiesGroup m_EnemysGroup;
        private Background m_Background;
        private List<Player> m_Players;

        public SpaceInvaders()
        {
            m_InputManager = new GameInputManager(this); 
            m_Players = new List<Player>(2);
            m_GameEngine = new GameEngine(this);
            m_GameEngine.Players = m_Players;
            m_InputManager = new GameInputManager(this);
            s_RandomNum = new Random();
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);
            Components.Add(this.m_Background);
            this.m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.m_MotherSpaceShip = new MotherSpaceShip(this);
            Components.Add(this.m_MotherSpaceShip);
            this.m_EnemysGroup = new EnemiesGroup(this);
            Components.Add(this.m_EnemysGroup);
            this.IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //Mouse.SetPosition((int)this.m_SpaceShip.Position.X, GraphicsDevice.Viewport.Height);
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
            if (m_InputManager.IsPlayerAskToExit())
            {
                this.Exit();
            }

            for (int i=0; i< k_NumOfPlayers; i++)
            {
                if(m_Players[i].IsFreeBulletExists())
                {
                    if(m_InputManager.IsplayerAskedToShoot(i))
                    {
                        m_Players[i].Shoot();
                    }
                }
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
    }
}
