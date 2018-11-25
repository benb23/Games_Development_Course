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
    public class InputOutputManager
    {
        private KeyboardState m_PastKey;
        private KeyboardState m_CurrKeyboardState;
        private MouseState m_PrevMouseState;

        public Vector2 GetMousePositionDelta()
        {
            Vector2 retVal = Vector2.Zero;

            MouseState currState = Mouse.GetState();

            if (this.m_PrevMouseState != null)
            {
                retVal.X = currState.X - this.m_PrevMouseState.X;
                retVal.Y = currState.Y - this.m_PrevMouseState.Y;
            }

            this.m_PrevMouseState = currState;

            return retVal;
        }

        public bool IsUserAskedToShoot()
        {
            bool isShootingOrder;
            KeyboardState currKeyboardState = Keyboard.GetState();
            MouseState currMouseState = Mouse.GetState();

            isShootingOrder = (currKeyboardState.IsKeyDown(Keys.Enter) && this.m_PastKey.IsKeyUp(Keys.Enter)) ||
                              (currMouseState.LeftButton.Equals(ButtonState.Pressed) && this.m_PrevMouseState.LeftButton.Equals(ButtonState.Released));

            this.m_PastKey = Keyboard.GetState();
            return isShootingOrder;
        }

        public bool IsUserAskedToMoveLeft()
        {
            this.m_CurrKeyboardState = Keyboard.GetState();
            bool IsUserAskedToMoveLeft = this.m_CurrKeyboardState.IsKeyDown(Keys.Left);
            return IsUserAskedToMoveLeft;
        }

        public bool IsUserAskedToMoveRight()
        {
            this.m_CurrKeyboardState = Keyboard.GetState();
            bool IsUserAskedToMoveRight = this.m_CurrKeyboardState.IsKeyDown(Keys.Right);
            return IsUserAskedToMoveRight;
        }

        public bool IsUserAskedToExit()
        {
            this.m_CurrKeyboardState = Keyboard.GetState();
            bool isUserAskedToExit = this.m_CurrKeyboardState.IsKeyDown(Keys.Escape);

            return isUserAskedToExit;
        }

        public void ShowGameOverMessage()
        {
            System.Windows.Forms.MessageBox.Show(string.Format(
@"Game Over
Youre score is: {0}", 
SpaceInvaders.s_GameUtils.ScoreManager.Score));
        }
    }
}
