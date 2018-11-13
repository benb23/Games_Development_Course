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
        
        protected SpriteBatch m_SpriteBatch;
        protected Texture2D m_Texture;
        protected Color m_Tint;
        protected string m_AssetName;


        protected Vector2 m_Position;
        public bool m_visible = true;
         
        protected float m_Direction = 1f;
        

        public Sprite(Game i_game):base(i_game)
        {
        }

        protected override void LoadContent()
        {
            m_SpriteBatch = this.Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            this.Texture = this.Game.Content.Load<Texture2D>(this.m_AssetName);
            base.LoadContent();
        }

        public override void Draw(GameTime i_GameTime)
        {
            this.SpriteBatch.Draw(this.Texture, this.Position, this.Tint);
            base.Draw(i_GameTime);
        }

        public virtual void AddComponent()
        {
            Game.Components.Add(this);
        }

        public SpriteBatch SpriteBatch
        {
            get { return SpriteBatch; }
            set { m_SpriteBatch = value; }
        }

        public Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }

        public Color Tint
        {
            get { return m_Tint; }
            set { m_Tint = value; }
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

        public string AssetName
        {
            get { return m_AssetName; }
            set { m_AssetName = value; }
        }




    }
}
