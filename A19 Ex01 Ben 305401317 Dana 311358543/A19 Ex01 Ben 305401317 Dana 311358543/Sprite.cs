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
    public class Sprite :  DrawableGameComponent
    {
        protected string m_AssetName;
        protected Texture2D m_Texture;
        protected Vector2 m_Position;
        public bool m_visible = true;
        protected Color m_Tint; 
        protected float m_Direction = 1f;
        protected SpriteBatch m_SpriteBatch;
        Gun m_Gun;

        public Sprite(Game i_game):base(i_game)
        {
        }

        protected override void LoadContent()
        {
            m_SpriteBatch = this.Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            this.Texture = this.Game.Content.Load<Texture2D>(this.m_AssetName);
            base.LoadContent();
        }

        public float Direction
        {
            get { return m_Direction; }

            set { m_Direction = value; }
        }
        public Vector2 Position
        {
            get { return m_Position; }

            set { m_Position = value; }
        }
        public Texture2D Texture
        {
            get { return m_Texture; }

            set { m_Texture = value; }
        }
        /*
        public void Draw()
        {

        }*/
    }
}
