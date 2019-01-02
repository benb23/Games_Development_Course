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
    public class Player : IScoreable
    {
        private SpaceShip m_SpaceShip;

        private List<Soul> m_Souls;

        private int m_Score;

        public SpaceShip SpaceShip
        {
            get { return m_SpaceShip; }
        }
        public Player(Game i_Game)
        {
            m_Souls = new List<Soul>(3);
            m_SpaceShip = new SpaceShip(i_Game);

        }
        public int Score { get; set; }
    }
}
