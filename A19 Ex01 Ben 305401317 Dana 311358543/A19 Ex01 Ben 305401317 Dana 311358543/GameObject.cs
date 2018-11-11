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
    abstract class GameObject
    {
        float m_Height;
        float m_Width;
        private Texture2D m_Texture;
        private Vector2 m_Position;
        private float m_Direction = 1f;
        Gun m_Gun;

        public float Direction
        {
            get { return m_Direction; }

            set { }
        }
        public Vector2 Position
        {
            get { return m_Position; }

            set { }
        }
        public Texture2D Texture
        {
            get { return m_Texture; }

            set { }
        }

        public abstract void Move();
    }
}
