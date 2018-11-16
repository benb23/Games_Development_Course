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
    public class InputManager
    {
        private KeyboardState m_PastKey;
        private MouseState m_pastMouseState;
        KeyboardState currKeyboardState;

        public static MouseState? m_PrevMouseState;
        //public static KeyboardState? m_PastKey;??

        public Vector2 GetMousePositionDelta()
        {
            Vector2 retVal = Vector2.Zero;

            MouseState currState = Mouse.GetState();

            if (m_PrevMouseState != null)
            {
                retVal.X = (currState.X - m_PrevMouseState.Value.X);
                retVal.Y = (currState.Y - m_PrevMouseState.Value.Y);
            }

            m_PrevMouseState = currState;

            return retVal;
        }

        public bool IsUserAskedToShoot()
        {
            bool isShootingOrder;
            KeyboardState currKeyboardState = Keyboard.GetState();
            MouseState currMouseState = Mouse.GetState();

            if ((currKeyboardState.IsKeyDown(Keys.Enter) && m_PastKey.IsKeyUp(Keys.Enter)) || (currMouseState.LeftButton.Equals(ButtonState.Pressed) && m_pastMouseState.LeftButton.Equals(ButtonState.Released)))
            {
                isShootingOrder = true;
            }
            else
            {
                isShootingOrder = false;
            }

            m_PastKey = Keyboard.GetState();
            m_pastMouseState = Mouse.GetState();
            return isShootingOrder;
        }

        public bool isUserAskedToExit()
        {
            currKeyboardState = Keyboard.GetState();
            bool isUserAskedToExit = currKeyboardState.IsKeyDown(Keys.Escape);

            return isUserAskedToExit;
        }
    }
}
