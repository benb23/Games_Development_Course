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
    public class PlayScreen : GameScreen
    {
        private bool m_initialize = false;
        private const int k_NumOfPlayers = 2;
        private const int k_NumOfWalls = 4;
        private ISpaceInvadersEngine m_GameEngine;
        private IInputManager m_InputManager;
        //private SpriteBatch m_SpriteBatch;
        public const int k_MaxRandomNumber = 50000;
        private const string k_GameName = "Space Invaders";
        public Random m_Random = new Random();
        //private GraphicsDeviceManager m_Graphics;
        private MotherSpaceShip m_MotherSpaceShip;
        private EnemiesGroup m_EnemysGroup;
        private Background m_Background;
        private List<Player> m_Players;
        private CollisionsManager m_CollisionManager;
        private WallsGroup m_WallsGroup;
        //SpriteFont m_FontCalibri;
        PauseScreen m_PauseScreenScreen;

        public PlayScreen(Game i_Game)
            : base(i_Game)
        {
            i_Game.IsMouseVisible = true;
            this.m_Background = new Background(this, @"Sprites\BG_Space01_1024x768", 1);
            this.m_MotherSpaceShip = new MotherSpaceShip(this);
            m_Players = new List<Player>(k_NumOfPlayers);
            m_Players.Add(new Player(this, PlayerIndex.One, Keys.H, Keys.K, Keys.U, true, new Vector2(0, 0)));
            m_Players.Add(new Player(this, PlayerIndex.Two, Keys.A, Keys.D, Keys.W, false, new Vector2(1, 0)));
            m_CollisionManager = new CollisionsManager(this.Game);
            //m_InputManager = new InputManager(this);
            m_GameEngine = new SpaceInvadersEngine(this);
            m_GameEngine.Players = m_Players;
            this.m_EnemysGroup = new EnemiesGroup(this);
            this.m_WallsGroup = new WallsGroup(this, k_NumOfWalls);
            m_PauseScreenScreen = new PauseScreen(this.Game);
        }

        private void OnGameOver()//TODO: CALL 
        {
            this.ExitScreen();
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            // we want to fade in only uppon first activation:
            this.ActivationLength = TimeSpan.Zero;
        }

        //protected override void LoadContent()
        //{
        //    base.LoadContent();

        //    //m_FontCalibri = ContentManager.Load<SpriteFont>(@"Fonts\Calibri");
        //}

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.P))
            {
                ScreensManager.SetCurrentScreen(m_PauseScreenScreen);
            }

            if(!m_initialize)//todo: fix
            {
                m_WallsGroup.WallsYShift = GraphicsDevice.Viewport.Height - (2 * m_Players[(int)PlayerIndex.One].SpaceShip.Texture.Height);
                m_initialize = true;
            }
        }

        public override void Initialize()
        {
            //m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            //this.Game.Services.AddService(typeof(SpriteBatch), m_SpriteBatch);
            this.Game.Services.AddService(typeof(Random), m_Random);
            this.Game.Window.Title = k_GameName;
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            m_SpriteBatch.Begin();
            base.Draw(gameTime);
            m_SpriteBatch.End();
        }
    }
}

