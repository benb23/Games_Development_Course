 ///*** Guy Ronen © 2008-2011 ***//
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Infrastructure
{
    public abstract class LoadableDrawableComponent : DrawableGameComponent
    {
        public event EventHandler<EventArgs> Disposed;

        protected virtual void OnDisposed(object sender, EventArgs args)
        {
            if (this.Disposed != null)
            {
                this.Disposed.Invoke(sender, args);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.OnDisposed(this, EventArgs.Empty);
        }

        protected string m_AssetName;

        // used to load the sprite:
        protected ContentManager ContentManager
        {
            get { return this.Game.Content; }
        }

        public event EventHandler<EventArgs> PositionChanged;

        protected virtual void OnPositionChanged()
        {
            if (this.PositionChanged != null)
            {
                this.PositionChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> SizeChanged;

        protected virtual void OnSizeChanged()
        {
            if (this.SizeChanged != null)
            {
                this.SizeChanged(this, EventArgs.Empty);
            }
        }

        public string AssetName
        {
            get { return this.m_AssetName; }
            set { this.m_AssetName = value; }
        }

        public LoadableDrawableComponent(
            string i_AssetName, GameScreen i_GameScreen, int i_UpdateOrder, int i_DrawOrder)
            : base(i_GameScreen.Game)
        {
            this.AssetName = i_AssetName;
            this.UpdateOrder = i_UpdateOrder;
            this.DrawOrder = i_DrawOrder;

            // register in the screen:
            i_GameScreen.Add(this);
        }

        //// composite member
        public LoadableDrawableComponent(string i_AssetName, Game i_Game) : base(i_Game)
        {
            this.AssetName = i_AssetName;
        }

        public LoadableDrawableComponent(
            string i_AssetName,
             GameScreen i_GameScreen,
            int i_CallsOrder)
            : this(i_AssetName, i_GameScreen, i_CallsOrder, i_CallsOrder)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            if (this is ICollidable)
            {
                ICollisionsManager collisionMgr =
                    this.Game.Services.GetService(typeof(ICollisionsManager))
                        as ICollisionsManager;

                if (collisionMgr != null)
                {
                    collisionMgr.AddObjectToMonitor(this as ICollidable);
                }
            }

            this.InitBounds(); 
        }

#if DEBUG
        protected bool m_ShowBoundingBox = true;
#else
        protected bool m_ShowBoundingBox = false;
#endif

        public bool ShowBoundingBox
        {
            get { return this.m_ShowBoundingBox; }
            set { this.m_ShowBoundingBox = value; }
        }

        protected abstract void InitBounds();

        public override void Draw(GameTime gameTime)
        {
            this.DrawBoundingBox();
            base.Draw(gameTime);
        }

        protected abstract void DrawBoundingBox();
    }
}