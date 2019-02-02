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
        private Texture2D m_CurrTexture;
        
        public Texture2D CurrTexture
        {
            get { return m_CurrTexture; }
            set { m_CurrTexture = value; }
        }

        private Color[] m_Pixels;
        private Color[] m_OriginalPixels;
        public Color[] Pixels
        {
            get { return m_Pixels; }
            set { m_Pixels = value; }

        }
        public Color[] OriginalPixels
        {
            get { return m_OriginalPixels; }
            set { m_OriginalPixels = value; }
        }
        protected List<Vector2> m_CollidedPixelsPositions;
        protected List<Vector2> m_CollidedPixelsIndex;

        public List<Vector2> LastCollisionPixelsPositions
        {
            get { return m_CollidedPixelsPositions; }
        }

        public List<Vector2> LastCollisionPixelsIndex
        {
            get { return m_CollidedPixelsIndex; }
        }

        public CollidableSprite(string i_AssetName, GameScreen i_GameScreen) : base(i_AssetName, i_GameScreen)
        {
            m_CollidedPixelsPositions = new List<Vector2>();
            m_CollidedPixelsIndex = new List<Vector2>();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.OriginalPixels = new Color[this.Texture.Width * this.Texture.Height];
            this.Pixels = new Color[this.Texture.Width * this.Texture.Height];
            this.Texture.GetData<Color>(this.Pixels);
            this.OriginalPixels = (Color[])this.Pixels.Clone();
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

        public override void Draw(GameTime gameTime)
        {
            if (m_CurrTexture == null)
            {
                this.DrawWithAllParameters();
            }
            else
            {
                m_SpriteBatch.Draw(m_CurrTexture, this.PositionForDraw,
                    this.SourceRectangle, this.TintColor,
                   this.Rotation, this.RotationOrigin, this.Scales,
                   SpriteEffects.None, this.LayerDepth);
            }
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
                    Color pixelA = Pixels[(x - Bounds.Left) + (y - Bounds.Top) * Bounds.Width];
                    Color pixelB = i_Source.Pixels[(x - i_Source.Bounds.Left) + (y - i_Source.Bounds.Top) * i_Source.Bounds.Width];

                    if (pixelA.A != 0 && pixelB.A != 0)
                    {
                        ///keep colliding pixels data
                        m_CollidedPixelsPositions.Add(new Vector2(x, y)); 
                        (i_Source as CollidableSprite).LastCollisionPixelsPositions.Add(new Vector2(x, y));
                        m_CollidedPixelsIndex.Add(new Vector2(x - Bounds.Left, y - Bounds.Top));
                        (i_Source as CollidableSprite).LastCollisionPixelsIndex.Add(new Vector2(x - i_Source.Bounds.Left, y - i_Source.Bounds.Top));

                        isPixelsCollided = true;
                    }
                }
            }

            return isPixelsCollided;
        }
    }
}
