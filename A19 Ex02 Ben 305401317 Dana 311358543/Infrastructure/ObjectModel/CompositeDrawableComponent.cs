////*** Guy Ronen © 2008-2011 ***//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure
{
    /// <summary>
    /// A class to assist with being able to nest game components inside of each other, provides support for all of the
    /// same functionality the game object performs on components with the addition of being neutral to where it resides
    /// in the hierarchy.
    /// </summary>
    public abstract class CompositeDrawableComponent<ComponentType> :
        DrawableGameComponent, ICollection<ComponentType>
        where ComponentType : IGameComponent
    {
        // the entire collection, for general collection methods (count, foreach, etc.):
        private Collection<ComponentType> m_Components = new Collection<ComponentType>();

        #region Selective Collections
        // selective holders for specific operations each frame:
        private List<ComponentType> m_UninitializedComponents = new List<ComponentType>();
        protected List<IUpdateable> m_UpdateableComponents = new List<IUpdateable>();
        protected List<IDrawable> m_DrawableComponents = new List<IDrawable>();
        protected List<Sprite> m_Sprites = new List<Sprite>();
        #endregion //Selective Collections

        #region Events

        public event EventHandler<GameComponentEventArgs<ComponentType>> ComponentAdded;

        public event EventHandler<GameComponentEventArgs<ComponentType>> ComponentRemoved;

        #endregion //Events

        #region Add/Remove
        protected virtual void OnComponentAdded(GameComponentEventArgs<ComponentType> e)
        {
            if (this.m_IsInitialized)
            {
                this.InitializeComponent(e.GameComponent);
            }
            else
            {
                this.m_UninitializedComponents.Add(e.GameComponent);
            }

            // If the new component implements IUpdateable:
            // 1. find a spot for it on the updateable list 
            // 2. hook it's UpdateOrderChanged event
            IUpdateable updatable = e.GameComponent as IUpdateable;
            if (updatable != null)
            {
                this.insertSorted(updatable);
                updatable.UpdateOrderChanged += new EventHandler<EventArgs>(this.childUpdateOrderChanged);
            }

            // If the new component implements IDrawable:
            // 1. find a spot for it on the drawable lists (IDrawble/Sprite) 
            // 2. hook it's DrawOrderChanged event
            IDrawable drawable = e.GameComponent as IDrawable;
            if (drawable != null)
            {
                this.insertSorted(drawable);
                drawable.DrawOrderChanged += new EventHandler<EventArgs>(this.childDrawOrderChanged);
            }

            // raise the Added event:
            if (this.ComponentAdded != null)
            {
                this.ComponentAdded(this, e);
            }
        }

        protected virtual void OnComponentRemoved(GameComponentEventArgs<ComponentType> e)
        {
            if (!this.m_IsInitialized)
            {
                this.m_UninitializedComponents.Remove(e.GameComponent);
            }

            IUpdateable updatable = e.GameComponent as IUpdateable;
            if (updatable != null)
            {
                this.m_UpdateableComponents.Remove(updatable);
                updatable.UpdateOrderChanged -= this.childUpdateOrderChanged;
            }

            Sprite sprite = e.GameComponent as Sprite;
            if (sprite != null)
            {
                this.m_Sprites.Remove(sprite);
                sprite.DrawOrderChanged -= this.childDrawOrderChanged;
            }
            else
            {
                IDrawable drawable = e.GameComponent as IDrawable;
                if (drawable != null)
                {
                    this.m_DrawableComponents.Remove(drawable);
                    drawable.DrawOrderChanged -= this.childDrawOrderChanged;
                }
            }

            // raise the Removed event:
            if (this.ComponentRemoved != null)
            {
                this.ComponentRemoved(this, e);
            }
        }

        /// <summary>
        /// When the update order of a component in this manager changes, will need to find a new place for it
        /// on the list of updateable components.
        /// </summary>
        private void childUpdateOrderChanged(object sender, EventArgs e)
        {
            IUpdateable updatable = sender as IUpdateable;
            this.m_UpdateableComponents.Remove(updatable);
            this.insertSorted(updatable);
        }

        /// <summary>
        /// When the draw order of a component in this manager changes, will need to find a new place for it
        /// on the list of drawable components.
        /// </summary>
        private void childDrawOrderChanged(object sender, EventArgs e)
        {
            IDrawable drawable = sender as IDrawable;

            Sprite sprite = sender as Sprite;
            if (sprite != null)
            {
                this.m_Sprites.Remove(sprite);
            }
            else
            {
                this.m_DrawableComponents.Remove(drawable);
            }

            this.insertSorted(drawable);
        }

        public CompositeDrawableComponent(Game i_Game)
            : base(i_Game)
        {
        }

        private void insertSorted(IUpdateable i_Updatable)
        {
            int idx = this.m_UpdateableComponents.BinarySearch(i_Updatable, UpdateableComparer.Default);
            if (idx < 0)
            {
                idx = ~idx;
            }

            this.m_UpdateableComponents.Insert(idx, i_Updatable);
        }

        private void insertSorted(IDrawable i_Drawable)
        {
            Sprite sprite = i_Drawable as Sprite;
            if (sprite != null)
            {
                int idx = this.m_Sprites.BinarySearch(sprite, DrawableComparer<Sprite>.Default);
                if (idx < 0)
                {
                    idx = ~idx;
                }

                this.m_Sprites.Insert(idx, sprite);
            }
            else
            {
                int idx = this.m_DrawableComponents.BinarySearch(i_Drawable, DrawableComparer<IDrawable>.Default);
                if (idx < 0)
                {
                    idx = ~idx;
                }

                this.m_DrawableComponents.Insert(idx, i_Drawable);
            }
        }
        #endregion //Add/Remove

        #region Compoiste Drawbale Component
        private bool m_IsInitialized;

        /// <summary>
        /// initialize any component that haven't been initialized yet
        /// and remove it from the list of uninitialized components
        /// </summary>
        public override void Initialize()
        {
            if (!this.m_IsInitialized)
            {
                // Initialize any un-initialized game components
                while (this.m_UninitializedComponents.Count > 0)
                {
                    this.InitializeComponent(this.m_UninitializedComponents[0]);
                }

                base.Initialize();
                this.m_IsInitialized = true;
            }
        }

        private void InitializeComponent(ComponentType i_Component)
        {
            if (i_Component is Sprite)
            {
                (i_Component as Sprite).SpriteBatch = this.m_SpriteBatch;
            }

            i_Component.Initialize();
            this.m_UninitializedComponents.Remove(i_Component);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.m_SpriteBatch = new SpriteBatch(this.GraphicsDevice);

            foreach (Sprite sprite in this.m_Sprites)
            {
                sprite.SpriteBatch = this.m_SpriteBatch;
            }
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.m_UpdateableComponents.Count; i++)
            {
                IUpdateable updatable = this.m_UpdateableComponents[i];
                if (updatable.Enabled)
                {
                    updatable.Update(gameTime);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            this.m_SpriteBatch.Begin(
                                this.SpritesSortMode, 
                                this.BlendState, 
                                this.SamplerState,
                                this.DepthStencilState, 
                                this.RasterizerState, 
                                this.Shader, 
                                this.TransformMatrix);

            foreach (Sprite sprite in this.m_Sprites)
            {
                if (sprite.Visible)
                {
                    sprite.Draw(gameTime);
                }
            }

            this.m_SpriteBatch.End();

            foreach (IDrawable drawable in this.m_DrawableComponents)
            {
                if (drawable.Visible)
                {
                    drawable.Draw(gameTime);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose of components in this manager
                for (int i = 0; i < this.Count; i++)
                {
                    IDisposable disposable = this.m_Components[i] as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }

            base.Dispose(disposing);
        }
        #endregion //Compoiste Drawbale Component

        #region ICollection<ComponentType> Implementations

        public virtual void Add(ComponentType i_Component)
        {
            this.InsertItem(this.m_Components.Count, i_Component);
        }

        protected virtual void InsertItem(int i_Idx, ComponentType i_Component)
        {
            if (this.m_Components.IndexOf(i_Component) != -1)
            {
                throw new ArgumentException("Duplicate components are not allowed in the same GameComponentManager.");
            }

            if (i_Component != null)
            {
                this.m_Components.Insert(i_Idx, i_Component);

                this.OnComponentAdded(new GameComponentEventArgs<ComponentType>(i_Component));
            }
        }

        public void Clear()
        {
            for (int i = 0; i < this.Count; i++)
            {
                this.OnComponentRemoved(new GameComponentEventArgs<ComponentType>(this.m_Components[i]));
            }

            this.m_Components.Clear();
        }

        public bool Contains(ComponentType i_Component)
        {
            return this.m_Components.Contains(i_Component);
        }

        public void CopyTo(ComponentType[] io_ComponentsArray, int i_ArrayIndex)
        {
            this.m_Components.CopyTo(io_ComponentsArray, i_ArrayIndex);
        }

        public int Count
        {
            get { return this.m_Components.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public virtual bool Remove(ComponentType i_Component)
        {
            bool removed = this.m_Components.Remove(i_Component);

            if (i_Component != null && removed)
            {
                this.OnComponentRemoved(new GameComponentEventArgs<ComponentType>(i_Component));
            }

            return removed;
        }

        public IEnumerator<ComponentType> GetEnumerator()
        {
            return this.m_Components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.m_Components).GetEnumerator();
        }

        #endregion ICollection<ComponentType> Implementations

        #region SpriteBatch Advanced Support
        protected SpriteBatch m_SpriteBatch;

        public SpriteBatch SpriteBatch
        {
            get { return this.m_SpriteBatch; }
            set { this.m_SpriteBatch = value; }
        }

        protected BlendState m_BlendState = BlendState.AlphaBlend;

        public BlendState BlendState
        {
            get { return this.m_BlendState; }
            set { this.m_BlendState = value; }
        }

        protected SpriteSortMode m_SpritesSortMode = SpriteSortMode.Deferred;

        public SpriteSortMode SpritesSortMode
        {
            get { return this.m_SpritesSortMode; }
            set { this.m_SpritesSortMode = value; }
        }

        protected SamplerState m_SamplerState = null;

        public SamplerState SamplerState
        {
            get { return this.m_SamplerState; }
            set { this.m_SamplerState = value; }
        }

        protected DepthStencilState m_DepthStencilState = null;

        public DepthStencilState DepthStencilState
        {
            get { return this.m_DepthStencilState; }
            set { this.m_DepthStencilState = value; }
        }

        protected RasterizerState m_RasterizerState = null;

        public RasterizerState RasterizerState
        {
            get { return this.m_RasterizerState; }
            set { this.m_RasterizerState = value; }
        }

        protected Effect m_Shader = null;

        public Effect Shader
        {
            get { return this.m_Shader; }
            set { this.m_Shader = value; }
        }

        protected Matrix m_TransformMatrix = Matrix.Identity;

        public Matrix TransformMatrix
        {
            get { return this.m_TransformMatrix; }
            set { this.m_TransformMatrix = value; }
        }
        #endregion SpriteBatch Advanced Support

        #region Helping Properties
        protected Vector2 CenterOfViewPort
        {
            get
            {
                return new Vector2((float)Game.GraphicsDevice.Viewport.Width / 2, (float)Game.GraphicsDevice.Viewport.Height / 2);
            }
        }

        public ContentManager ContentManager
        {
            get { return this.Game.Content; }
        }
        #endregion Helping Properties
    }

    /// <summary>
    /// A comparer designed to assist with sorting IUpdateable interfaces.
    /// </summary>
    public sealed class UpdateableComparer : IComparer<IUpdateable>
    {
        /// <summary>
        /// A static copy of the comparer to avoid the GC.
        /// </summary>
        public static readonly UpdateableComparer Default;

        static UpdateableComparer()
        {
            Default = new UpdateableComparer();
        }

        private UpdateableComparer()
        {
        }

        public int Compare(IUpdateable x, IUpdateable y)
        {
            const int k_XBigger = 1;
            const int k_Equal = 0;
            const int k_YBigger = -1;

            int retCompareResult = k_YBigger;

            if (x == null && y == null)
            {
                retCompareResult = k_Equal;
            }
            else if (x != null)
            {
                if (y == null)
                {
                    retCompareResult = k_XBigger;
                }
                else if (x.Equals(y))
                {
                    return k_Equal;
                }
                else if (x.UpdateOrder > y.UpdateOrder)
                {
                    return k_XBigger;
                }
            }

            return retCompareResult;
        }
    }
    /// <summary>
    /// A comparer designed to assist with sorting IDrawable interfaces.
    /// </summary>
    public sealed class DrawableComparer<TDrawble> : IComparer<TDrawble>
        where TDrawble : class, IDrawable
    {
        /// <summary>
        /// A static copy of the comparer to avoid the GC.
        /// </summary>   
        public static readonly DrawableComparer<TDrawble> Default;

        static DrawableComparer()
        {
            Default = new DrawableComparer<TDrawble>();
        }

        private DrawableComparer()
        {
        }

        #region IComparer<IDrawable> Members

        public int Compare(TDrawble x, TDrawble y)
        {
            const int k_XBigger = 1;
            const int k_Equal = 0;
            const int k_YBigger = -1;

            int retCompareResult = k_YBigger;

            if (x == null && y == null)
            {
                retCompareResult = k_Equal;
            }
            else if (x != null)
            {
                if (y == null)
                {
                    retCompareResult = k_XBigger;
                }
                else if (x.Equals(y))
                {
                    return k_Equal;
                }
                else if (x.DrawOrder > y.DrawOrder)
                {
                    return k_XBigger;
                }
            }

            return retCompareResult;
        }

        #endregion
    }

    /// <summary>
    /// Arguments used with events from the GameComponentCollection.
    /// </summary>
    /// <typeparam name="ComponentType"></typeparam>
    public class GameComponentEventArgs<ComponentType> : EventArgs
        where ComponentType : IGameComponent
    {
        private ComponentType m_Component;

        public GameComponentEventArgs(ComponentType gameComponent)
        {
            this.m_Component = gameComponent;
        }

        public ComponentType GameComponent
        {
            get { return this.m_Component; }
        }
    }
}