using Microsoft.Xna.Framework.Graphics;

namespace A19_Ex01_Ben_305401317_Dana_311358543
{
    public class GameUtils
    {
        private SpriteBatch m_SpriteBatch;
        private InputManager m_InputOutputManager;
        private ShootingManager m_ShootingManager;
        private ScoreManager m_ScoreManager;

        public SpriteBatch SpriteBatch
        {
            get { return this.m_SpriteBatch; }
            set { this.m_SpriteBatch = value; }
        }

        public InputManager InputManager
        {
            get { return this.m_InputOutputManager; }
            set { this.m_InputOutputManager = value; }
        }

        public ScoreManager ScoreManager
        {
            get { return this.m_ScoreManager; }
            set { this.m_ScoreManager = value; }
        }

        public ShootingManager ShootingManager
        {
            get { return this.m_ShootingManager; }
            set { this.m_ShootingManager = value; }
        }
    }
}
