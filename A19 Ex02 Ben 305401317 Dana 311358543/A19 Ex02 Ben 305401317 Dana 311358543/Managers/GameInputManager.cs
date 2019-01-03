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
    public class GameInputManager : InputManager
    {
        private IGameEngine m_GameEngine;
        public GameInputManager(Game i_Game): base(i_Game)
        {

        }

        public bool IsplayerAskedToShoot(int i_PlayerIndex)
        {
            bool IsplayerAskedToShoot;

            if (i_PlayerIndex ==0 && KeyboardState.IsKeyDown(Keys.U) && PrevKeyboardState.IsKeyUp(Keys.U))
            {
                IsplayerAskedToShoot = true;
            }
            else if (i_PlayerIndex == 1 && KeyboardState.IsKeyDown(Keys.W) && PrevKeyboardState.IsKeyUp(Keys.W))
            {
                IsplayerAskedToShoot = true;
            }
            else
            {
                IsplayerAskedToShoot = false;
            }

            return IsplayerAskedToShoot;
        }

        public bool IsPlayerAskToExit()
        {
            bool IsPlayerAskToExit;

            if (KeyboardState.IsKeyDown(Keys.Escape))
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
