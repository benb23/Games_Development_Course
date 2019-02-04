 ///*** Guy Ronen © 2008-2011 ***//
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure
{
    public class Sprite : LoadableDrawableComponent
    {
        protected GameScreen m_GameScreen;
        protected bool m_Initialize;

        protected GameScreen GameScreen
        {
            get { return this.m_GameScreen; }
        }

        protected CompositeAnimator m_Animations;

        public CompositeAnimator Animations
        {
            get { return this.m_Animations; }
            set { this.m_Animations = value; }
        }

        private Texture2D m_Texture;

        public Texture2D Texture
        {
            get { return this.m_Texture; }
            set { this.m_Texture = value; }
        }

        public float Width
        {
            get { return this.m_WidthBeforeScale * this.m_Scales.X; }
            set { this.m_WidthBeforeScale = value / this.m_Scales.X; }
        }

        public float Height
        {
            get { return this.m_HeightBeforeScale * this.m_Scales.Y; }
            set { this.m_HeightBeforeScale = value / this.m_Scales.Y; }
        }

        protected float m_WidthBeforeScale;

        public float WidthBeforeScale
        {
            get { return this.m_WidthBeforeScale; }
            set { this.m_WidthBeforeScale = value; }
        }

        protected float m_HeightBeforeScale;

        public float HeightBeforeScale
        {
            get { return this.m_HeightBeforeScale; }
            set { this.m_HeightBeforeScale = value; }
        }

        protected Vector2 m_Position = Vector2.Zero;

        public Vector2 Position
        {
            get { return this.m_Position; }
            set
            {
                if (this.m_Position != value)
                {
                    this.m_Position = value;
                    this.OnPositionChanged();
                }
            }
        }

        public Vector2 m_PositionOrigin;

        public Vector2 PositionOrigin
        {
            get { return this.m_PositionOrigin; }
            set { this.m_PositionOrigin = value; }
        }

        public Vector2 m_RotationOrigin = Vector2.Zero;

        public Vector2 RotationOrigin
        {
            get { return this.m_RotationOrigin; }
            set { this.m_RotationOrigin = value; }
        }

        protected Vector2 PositionForDraw
        {
            get { return this.Position - this.PositionOrigin + this.RotationOrigin; }
        }

        public Vector2 TopLeftPosition
        {
            get { return this.Position - this.PositionOrigin; }
            set { this.Position = value + this.PositionOrigin; }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)this.TopLeftPosition.X,
                    (int)this.TopLeftPosition.Y,
                    (int)this.Width,
                    (int)this.Height);
            }
        }

        public Rectangle BoundsBeforeScale
        {
            get
            {
                return new Rectangle(
                    (int)this.TopLeftPosition.X,
                    (int)this.TopLeftPosition.Y,
                    (int)this.WidthBeforeScale,
                    (int)this.HeightBeforeScale);
            }
        }

        protected Rectangle m_SourceRectangle;

        public Rectangle SourceRectangle
        {
            get { return this.m_SourceRectangle; }
            set { this.m_SourceRectangle = value; }
        }

        public Vector2 TextureCenter
        {
            get
            {
                return new Vector2((float)(this.m_Texture.Width / 2), (float)(this.m_Texture.Height / 2));
            }
        }

        public Vector2 SourceRectangleCenter
        {
            get { return new Vector2((float)(this.m_SourceRectangle.Width / 2), (float)(this.m_SourceRectangle.Height / 2)); }
        }

        protected float m_Rotation = 0;

        public float Rotation
        {
            get { return this.m_Rotation; }
            set { this.m_Rotation = value; }
        }

        protected Vector2 m_Scales = Vector2.One;

        public Vector2 Scales
        {
            get { return this.m_Scales; }
            set
            {
                if (this.m_Scales != value)
                {
                    this.m_Scales = value;
                    /// Notify the Collision Detection mechanism:
                    this.OnPositionChanged();
                }
            }
        }

        protected Color m_TintColor = Color.White;

        public Color TintColor
        {
            get { return this.m_TintColor; }
            set { this.m_TintColor = value; }
        }

        public float Opacity
        {
            get { return (float)this.m_TintColor.A / (float)byte.MaxValue; }
            set { this.m_TintColor.A = (byte)(value * (float)byte.MaxValue); }
        }

        protected float m_LayerDepth;

        public float LayerDepth
        {
            get { return this.m_LayerDepth; }
            set { this.m_LayerDepth = value; }
        }

        protected SpriteEffects m_SpriteEffects = SpriteEffects.None;

        public SpriteEffects SpriteEffects
        {
            get { return this.m_SpriteEffects; }
            set { this.m_SpriteEffects = value; }
        }

        protected Vector2 m_Velocity = Vector2.Zero;

        public Vector2 Velocity
        {
            get { return this.m_Velocity; }
            set { this.m_Velocity = value; }
        }

        private float m_AngularVelocity = 0;

        public float AngularVelocity
        {
            get { return this.m_AngularVelocity; }
            set { this.m_AngularVelocity = value; }
        }

        public Sprite(string i_AssetName, GameScreen i_GameScreen, int i_UpdateOrder, int i_DrawOrder)
            : base(i_AssetName, i_GameScreen, i_UpdateOrder, i_DrawOrder)
        {
            this.m_GameScreen = i_GameScreen;
        }

        public Sprite(string i_AssetName, GameScreen i_GameScreen, int i_CallsOrder)
            : base(i_AssetName, i_GameScreen, i_CallsOrder)
        {
            this.m_GameScreen = i_GameScreen;
        }

        public Sprite(string i_AssetName, GameScreen i_GameScreen)
            : base(i_AssetName, i_GameScreen, int.MaxValue)
        {
            this.m_GameScreen = i_GameScreen;
        }

        protected override void InitBounds()
        {
            if (this.m_Texture != null)
            {
                this.m_WidthBeforeScale = this.m_Texture.Width;
                this.m_HeightBeforeScale = this.m_Texture.Height;
            }

            this.InitSourceRectangle();

            this.InitOrigins();
        }

        protected virtual void InitOrigins()
        {
        }

        protected virtual void InitSourceRectangle()
        {
            this.m_SourceRectangle = new Rectangle(0, 0, (int)this.m_WidthBeforeScale, (int)this.m_HeightBeforeScale);
        }

        protected SpriteSortMode m_SpriteSortMode;
        protected BlendState m_BlendState;
        private bool m_UseSharedBatch = true; 
        protected SpriteBatch m_SpriteBatch;

        public SpriteBatch SpriteBatch
        {
            set
            {
                this.m_SpriteBatch = value;
                this.m_UseSharedBatch = true;
            }
        }

        protected override void LoadContent()
        {
            this.m_Texture = Game.Content.Load<Texture2D>(this.m_AssetName);

            if (this.m_SpriteBatch == null)
            {
                this.m_SpriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

                if (this.m_SpriteBatch == null)
                {
                    this.m_SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
                    this.m_UseSharedBatch = false;
                }
            }

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.Position += this.Velocity * totalSeconds;
            this.Rotation += this.AngularVelocity * totalSeconds;

            base.Update(gameTime);
            this.Animations.Update(gameTime);
        }

        protected void DrawWithAllParameters()
        {
            this.m_SpriteBatch.Draw(
                                    this.m_Texture, 
                                    this.PositionForDraw,
                                    this.SourceRectangle, 
                                    this.TintColor,
                                    this.Rotation, 
                                    this.RotationOrigin, 
                                    this.Scales,
                                    SpriteEffects.None, 
                                    this.LayerDepth);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!this.m_UseSharedBatch)
            {
                this.m_SpriteBatch.Begin();
            }

            this.DrawWithAllParameters();

            if (!this.m_UseSharedBatch)
            {
                this.m_SpriteBatch.End();
            }

            base.Draw(gameTime);
        }
       
        #region Collision Handlers
        protected override void DrawBoundingBox()
        {
            // not implemented yet
        }

        public Sprite ShallowClone()
        {
            return this.MemberwiseClone() as Sprite;
        }

        public override void Initialize()
        {
            base.Initialize();

            this.m_Animations = new CompositeAnimator(this);
        }

        #endregion //Collision Handlers
    }
}