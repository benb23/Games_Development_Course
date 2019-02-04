using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure
{
    public class CollidableSprite : Sprite
    {
        protected int m_ScoreValue; 

        public int ScoreValue
        {
            get { return this.m_ScoreValue; }
            set { this.m_ScoreValue = value; }
        }

        private Texture2D m_CurrTexture;
        
        public Texture2D CurrTexture
        {
            get { return this.m_CurrTexture; }
            set { this.m_CurrTexture = value; }
        }

        private Color[] m_Pixels;
        private Color[] m_OriginalPixels;

        public Color[] Pixels
        {
            get { return this.m_Pixels; }
            set { this.m_Pixels = value; }
        }

        public Color[] OriginalPixels
        {
            get { return this.m_OriginalPixels; }
            set { this.m_OriginalPixels = value; }
        }

        protected List<Vector2> m_CollidedPixelsPositions;

        protected List<Vector2> m_CollidedPixelsIndex;

        public Rectangle m_LastCollisionRectangle;

        public List<Vector2> LastCollisionPixelsPositions
        {
            get { return this.m_CollidedPixelsPositions; }
        }

        public List<Vector2> LastCollisionPixelsIndex
        {
            get { return this.m_CollidedPixelsIndex; }
        }

        public CollidableSprite(string i_AssetName, GameScreen i_GameScreen) : base(i_AssetName, i_GameScreen)
        {
            this.m_CollidedPixelsPositions = new List<Vector2>();
            this.m_CollidedPixelsIndex = new List<Vector2>();
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
                return this.checkPixelsCollision(i_Source);
            }
            else
            {
                return this.checkRectangleCollision(i_Source);
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
                pixelsCollided = this.isPixelsCollided(i_Source as IPixelsCollidable);
            }

            return pixelsCollided;
        }

        public override void Draw(GameTime gameTime)
        {
            if (this.m_CurrTexture == null)
            {
                this.DrawWithAllParameters();
            }
            else
            {
                this.m_SpriteBatch.Draw(
                                        this.m_CurrTexture, 
                                        this.PositionForDraw,
                                        this.SourceRectangle, 
                                        this.TintColor,
                                        this.Rotation, 
                                        this.RotationOrigin, 
                                        this.Scales,
                                        SpriteEffects.None, 
                                        this.LayerDepth);
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

            m_LastCollisionRectangle = new Rectangle(left, top, right - left, bottom - top);
            (i_Source as CollidableSprite).m_LastCollisionRectangle = new Rectangle(top, left, right - left, top - bottom); ;

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color pixelA = this.Pixels[(x - this.Bounds.Left) + ((y - this.Bounds.Top) * this.Bounds.Width)];
                    Color pixelB = i_Source.Pixels[(x - i_Source.Bounds.Left) + ((y - i_Source.Bounds.Top) * i_Source.Bounds.Width)];

                    if (pixelA.A != 0 && pixelB.A != 0)
                    {
                        ///keep colliding pixels data
                        this.m_CollidedPixelsPositions.Add(new Vector2(x, y)); 
                        (i_Source as CollidableSprite).LastCollisionPixelsPositions.Add(new Vector2(x, y));
                        this.m_CollidedPixelsIndex.Add(new Vector2(x - this.Bounds.Left, y - this.Bounds.Top));
                        (i_Source as CollidableSprite).LastCollisionPixelsIndex.Add(new Vector2(x - i_Source.Bounds.Left, y - i_Source.Bounds.Top));

                        isPixelsCollided = true;
                    }
                }
            }

            return isPixelsCollided;
        }
    }
}
