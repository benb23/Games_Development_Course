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
    public class WallsGroup : RegisteredComponent
    {
        private bool m_Initialize;
        private int m_NumOfWalls;
        private float m_Direction = 1f;
        private List<Wall> m_Walls;
        private Vector2 m_GroupPosition;
        private float m_GroupYShift;

        public float WallsYShift
        {
            set { m_GroupYShift = value; }
        }
 
        private Color[] m_OriginalPixels;

        public WallsGroup(Game i_Game,int i_numOfWalls) : base(i_Game)
        {
            m_NumOfWalls = i_numOfWalls;
            this.m_Walls = new List<Wall>(m_NumOfWalls);
        }

        public override void Initialize()
        {
            for (int i = 0; i < m_NumOfWalls; i++)
            {
                m_Walls.Add(new Wall(Game));
            }

            base.Initialize();
        }

        private void initWallsPositions()
        {
            m_GroupPosition = new Vector2(Game.GraphicsDevice.Viewport.Width / 3- m_Walls[0].Texture.Width/2, m_GroupYShift);

            for (int i=0 ; i < m_NumOfWalls ; i++)
            {
                m_Walls[i].Position = m_GroupPosition + new Vector2(m_Walls[i].Texture.Width*2*i, 0);
            }
        }
        public override void Update(GameTime i_GameTime)
        {
            if (!m_Initialize)
            {
                initWallsPositions();
                m_Initialize = true;
            }
            base.Update(i_GameTime);
        }

        private bool hitRightOrLeftBorders()
        {
            return hitRightBorder() || hitLeftBorder();
        }

        private bool hitRightBorder()
        {
            Wall rightWall = null;

            for (int i = m_NumOfWalls-1 ; i >= 0; i--)
            {
                if (m_Walls[i].Enabled)
                {
                    rightWall = m_Walls[i];
                    break;
                }
            }

            return (rightWall.Position.X >= Game.GraphicsDevice.Viewport.Width - rightWall.Texture.Width / 2);
        }

        private bool hitLeftBorder()
        {
            Wall leftWall = null;

            for (int i = 0; i < m_NumOfWalls; i++)
            {
                if (m_Walls[i].Enabled)
                {
                    leftWall = m_Walls[i];
                    break;
                }
            }

            return (leftWall.Position.X <=  leftWall.Texture.Width / 2);
        }
    }
}
