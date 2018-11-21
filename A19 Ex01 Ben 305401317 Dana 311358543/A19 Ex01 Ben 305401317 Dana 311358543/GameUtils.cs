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
    public class GameUtils
    {
        private SpriteBatch m_SpriteBatch;
        private InputManager m_InputManager;
        private ShootingManager m_ShootingManager;
        private ScoreManager m_ScoreManager;

        public SpriteBatch SpriteBatch
        {
            get { return this.m_SpriteBatch; }
            set { this.m_SpriteBatch = value; }
        }

        public InputManager InputManager
        {
            get { return this.m_InputManager; }
            set { this.m_InputManager = value; }
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
