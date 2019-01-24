﻿using System;
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
    public class WallsGroup : GameComponent
    {
        private bool m_Initialize;
        private int m_NumOfWalls;
        private List<Wall> m_Walls;
        private Vector2 m_GroupPosition;
        private float m_GroupYShift;
        private GameScreen m_GameScreen;

        public float WallsYShift
        {
            set { this.m_GroupYShift = value; }
        }
 
        public WallsGroup(GameScreen i_GameScreen, int i_numOfWalls) : base(i_GameScreen.Game)
        {
            this.m_GameScreen = i_GameScreen;
            this.m_NumOfWalls = i_numOfWalls;
            this.m_Walls = new List<Wall>(this.m_NumOfWalls);
            i_GameScreen.Add(this);
        }

        public override void Initialize()
        {
            for (int i = 0; i < this.m_NumOfWalls; i++)
            {
                this.m_Walls.Add(new Wall(m_GameScreen));
            }

            base.Initialize();
        }

        private void initWallsPositions()
        {
            this.m_GroupPosition = new Vector2((Game.GraphicsDevice.Viewport.Width / 3) - (this.m_Walls[0].Texture.Width / 2), this.m_GroupYShift);

            for (int i = 0; i < this.m_NumOfWalls; i++)
            {
                this.m_Walls[i].Position = this.m_GroupPosition + new Vector2(this.m_Walls[i].Texture.Width * 2 * i, 0);
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            if (!this.m_Initialize)
            {
                this.initWallsPositions();
                this.m_Initialize = true;
            }

            base.Update(i_GameTime);
        }

        private bool hitRightOrLeftBorders()
        {
            return this.hitRightBorder() || this.hitLeftBorder();
        }

        private bool hitRightBorder()
        {
            Wall rightWall = null;

            for (int i = this.m_NumOfWalls - 1; i >= 0; i--)
            {
                if (this.m_Walls[i].Enabled)
                {
                    rightWall = this.m_Walls[i];
                    break;
                }
            }

            return rightWall.Position.X >= Game.GraphicsDevice.Viewport.Width - (rightWall.Texture.Width / 2);
        }

        private bool hitLeftBorder()
        {
            Wall leftWall = null;

            for (int i = 0; i < this.m_NumOfWalls; i++)
            {
                if (this.m_Walls[i].Enabled)
                {
                    leftWall = this.m_Walls[i];
                    break;
                }
            }

            return leftWall.Position.X <= leftWall.Texture.Width / 2;
        }
    }
}
