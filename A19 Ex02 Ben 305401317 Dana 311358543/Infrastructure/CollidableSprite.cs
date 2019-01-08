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

namespace Infrastructure
{
    public class CollidableSprite : Sprite
    {
        private Color[] m_Pixels;

        public Color[] Pixels
        {
            get { return m_Pixels; }
        }

        private Texture2D m_currTexture;

        public Texture2D CurrTexture
        {
            get { return m_currTexture; }
        }

        public CollidableSprite(string i_AssetName, Game i_Game) : base(i_AssetName, i_Game)
        { }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_currTexture = Texture;
            m_Pixels = new Color[this.Texture.Width * this.Texture.Height];
            this.Texture.GetData<Color>(m_Pixels);
        }


        public virtual bool CheckCollision(ICollidable i_Source)
        {
            if(this is IPixelsCollidable && i_Source is IPixelsCollidable)
            {
                return checkPixelsCollision(i_Source);
            }
            else
            {
                return checkRectangleCollision(i_Source);
            }
        }

        private bool checkPixelsCollision(ICollidable i_Source)
        {
            bool pixelsCollided = false;
            bool rectanglesCollided = false;
            IPixelsCollidable source = i_Source as IPixelsCollidable;
            if (source != null)
            {
                rectanglesCollided = source.Bounds.Intersects(this.Bounds);
            }

            if (rectanglesCollided)
            {
                pixelsCollided = isPixelsCollided(i_Source as IPixelsCollidable);
            }

            return pixelsCollided;
        }

        private bool checkRectangleCollision(ICollidable i_Source)
        {
            {
                bool collided = false;
                IPixelsCollidable source = i_Source as IPixelsCollidable;
                if (source != null)
                {
                    collided = source.Bounds.Intersects(this.Bounds);
                }

                if (collided)
                {
                    collided = true;
                }

                return collided;
            }
        }

        private bool isPixelsCollided(IPixelsCollidable i_Source)
        {
            bool isPixelsCollided = false;

            int top = Math.Max(Bounds.Top, i_Source.Bounds.Top);
            int bottom = Math.Min(Bounds.Bottom, i_Source.Bounds.Bottom);
            int left = Math.Max(Bounds.Left, i_Source.Bounds.Left);
            int right = Math.Min(Bounds.Right, i_Source.Bounds.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color colorA = Pixels[(x - Bounds.Left) + (y - Bounds.Top) * Bounds.Width];
                    Color colorB = i_Source.Pixels[(x - i_Source.Bounds.Left) + (y - i_Source.Bounds.Top) * i_Source.Bounds.Width];

                    if (colorA.A != 0 && colorB.A != 0)
                    {

                        isPixelsCollided = true;
                        break;
                    }
                }
            }

            return isPixelsCollided;
        }

        public override void Draw(GameTime gameTime)
        {
            m_SpriteBatch.Draw(m_currTexture, this.PositionForDraw,
                this.SourceRectangle, this.TintColor,
               this.Rotation, this.RotationOrigin, this.Scales,
               SpriteEffects.None, this.LayerDepth);

            base.Draw(gameTime);
        }
    }
}
