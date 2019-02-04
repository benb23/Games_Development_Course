////*** Guy Ronen © 2008-2011 ***//
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure
{
    public enum eScreenState
    {
        Activating,
        Active,
        Deactivating,
        Inactive,
        Closing,
        Closed
    }

    public class StateChangedEventArgs : EventArgs
    {
        protected eScreenState m_PrevState;

        public eScreenState PrevState
        {
            get { return this.m_PrevState; }
            set { this.m_PrevState = value; }
        }

        protected eScreenState m_CurrentState;

        public eScreenState CurrentState
        {
            get { return this.m_CurrentState; }
            set { this.m_CurrentState = value; }
        }

        public StateChangedEventArgs()
        {
        }

        public StateChangedEventArgs(eScreenState i_PrevState, eScreenState i_CurrState)
        {
            this.m_PrevState = i_PrevState;
            this.m_CurrentState = i_CurrState;
        }
    }

    public abstract class GameScreen : CompositeDrawableComponent<IGameComponent>
    {
        protected bool m_initialized;

        ////CTOR:
        public GameScreen(Game i_Game)
            : base(i_Game)
        {
            this.Enabled = false;
            this.Visible = false;
        }

        protected eScreenState m_State = eScreenState.Inactive;

        public eScreenState State
        {
            get { return this.m_State; }
            set
            {
                if (this.m_State != value)
                {
                    StateChangedEventArgs args = new StateChangedEventArgs(this.m_State, value);
                    this.m_State = value;
                    this.OnStateChanged(args);
                }
            }
        }

        public event EventHandler<StateChangedEventArgs> StateChanged;

        private void OnStateChanged(StateChangedEventArgs args)
        {
            switch (args.CurrentState)
            {
                case eScreenState.Activating:
                    this.OnActivating();
                    break;
                case eScreenState.Active:
                    this.OnActivated();
                    break;
                case eScreenState.Deactivating:
                    break;
                case eScreenState.Closing:
                    break;
                case eScreenState.Inactive:
                case eScreenState.Closed:
                    this.OnDeactivated();
                    break;
                default:
                    break;
            }

            if (this.StateChanged != null)
            {
                this.StateChanged(this, args);
            }
        }

        ////PROPS:
        protected IScreensMananger m_ScreensManager;

        public IScreensMananger ScreensManager
        {
            get { return this.m_ScreensManager; }
            set { this.m_ScreensManager = value; }
        }

        protected bool m_IsModal = true;

        public bool IsModal // background screen should not be updated
        {
            get { return this.m_IsModal; }
            set { this.m_IsModal = value; }
        }

        protected bool m_IsOverlayed;

        public bool IsOverlayed // background screen should be drawn
        {
            get { return this.m_IsOverlayed; }
            set { this.m_IsOverlayed = value; }
        }

        protected GameScreen m_PreviousScreen;

        public GameScreen PreviousScreen
        {
            get { return this.m_PreviousScreen; }
            set { this.m_PreviousScreen = value; }
        }

        protected bool m_HasFocus;

        public bool HasFocus // should handle input
        {
            get { return this.m_HasFocus; }
            set { this.m_HasFocus = value; }
        }

        protected float m_BlackTintAlpha = 0;

        public float BlackTintAlpha
        {
            get { return this.m_BlackTintAlpha; }
            set
            {
                if (this.m_BlackTintAlpha < 0 || this.m_BlackTintAlpha > 1)
                {
                    throw new ArgumentException("value must be between 0 and 1", "BackgroundDarkness");
                }

                this.m_BlackTintAlpha = value;
            }
        }

        private IInputManager m_InputManager;
        private IInputManager m_DummyInputManager = new DummyInputManager();

        public IInputManager InputManager
        {
            get { return this.HasFocus ? this.m_InputManager : this.m_DummyInputManager; }
        }

        public override void Initialize()
        {
            this.m_InputManager = Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            if (this.m_InputManager == null)
            {
                this.m_InputManager = this.m_DummyInputManager;
            }
            else
            {
                this.m_InputManager.Initialize();
            }

            base.Initialize();
        }

        internal virtual void Activate()
        {
            if (this.State == eScreenState.Inactive
                || this.State == eScreenState.Deactivating
                || this.State == eScreenState.Closed
                || this.State == eScreenState.Closing)
            {
                this.State = eScreenState.Activating;

                if (this.m_ActivationLength == TimeSpan.Zero)
                {
                    this.State = eScreenState.Active;
                }
            }
        }

        protected virtual void OnActivating()
        {
            this.Enabled = true;
            this.Visible = true;
            this.HasFocus = true;
        }

        protected virtual void OnActivated()
        {
            if (this.PreviousScreen != null)
            {
                this.PreviousScreen.HasFocus = !this.HasFocus;
            }

            this.m_TransitionPosition = 1;
        }

        protected internal virtual void Deactivate()
        {
            if (this.State == eScreenState.Active
                || this.State == eScreenState.Activating)
            {
                this.State = eScreenState.Deactivating;

                if (this.m_DeactivationLength == TimeSpan.Zero)
                {
                    this.State = eScreenState.Inactive;
                }
            }
        }

        protected void ExitScreen() 
        {
            this.State = eScreenState.Closing;
            if (this.DeactivationLength == TimeSpan.Zero)
            {
                this.State = eScreenState.Closed;
            }
        }

        protected virtual void OnDeactivated()
        {
            this.Enabled = false;
            this.Visible = false;
            this.HasFocus = false;

            this.m_TransitionPosition = 0;
        }

        private Texture2D m_GradientTexture;
        private Texture2D m_BlankTexture;

        protected override void LoadContent()
        {
            base.LoadContent();

            this.m_GradientTexture = this.ContentManager.Load<Texture2D>(@"Screens\gradient");
            this.m_BlankTexture = this.ContentManager.Load<Texture2D>(@"Screens\blank");
        }

        public override void Draw(GameTime gameTime)
        {
            bool fading = this.UseFadeTransition
                && this.TransitionPosition > 0
                && this.TransitionPosition < 1;

            if (this.PreviousScreen != null
                && this.IsOverlayed)
            {
                this.PreviousScreen.Draw(gameTime);

                if (!fading && (this.BlackTintAlpha > 0 || this.UseGradientBackground))
                {
                    this.FadeBackBufferToBlack((byte)(this.m_BlackTintAlpha * byte.MaxValue));
                }
            }

            base.Draw(gameTime);

            if (fading)
            {
                this.FadeBackBufferToBlack(this.TransitionAlpha);
            }
        }

        protected bool m_UseGradientBackground = false;

        public bool UseGradientBackground
        {
            get { return this.m_UseGradientBackground; }
            set { this.m_UseGradientBackground = value; }
        }

        public void FadeBackBufferToBlack(byte i_Alpha)
        {
            Viewport viewport = this.GraphicsDevice.Viewport;

            Texture2D background = this.UseGradientBackground ? this.m_GradientTexture : this.m_BlankTexture;

            this.SpriteBatch.Begin();
            this.SpriteBatch.Draw(
                             background,
                             new Rectangle(0, 0, viewport.Width, viewport.Height),
                             new Color((byte)0, (byte)0, (byte)0, i_Alpha));
            this.SpriteBatch.End();
        }

        #region Transitions Support
        /// <summary>
        /// Indicates how long the screen takes to
        /// transition on when it is activated.
        /// </summary>
        public TimeSpan ActivationLength
        {
            get { return this.m_ActivationLength; }
            protected set { this.m_ActivationLength = value; }
        }

        private TimeSpan m_ActivationLength = TimeSpan.Zero;

        /// <summary>
        /// Indicates how long the screen takes to
        /// transition off when it is deactivated.
        /// </summary>
        public TimeSpan DeactivationLength
        {
            get { return this.m_DeactivationLength; }
            protected set { this.m_DeactivationLength = value; }
        }

        private TimeSpan m_DeactivationLength = TimeSpan.Zero;

        private float m_TransitionPosition = 0;
        /// <summary>
        /// Gets the current position of the screen transition, ranging
        /// from 0 (deactive, no transition) to 1 (active, no transition).
        /// </summary>
        
        public float TransitionPosition
        {
            get { return this.m_TransitionPosition; }
            protected set { this.m_TransitionPosition = value; }
        }
        /// <summary>
        /// There are two possible reasons why a screen might be transitioning
        /// off. It could be temporarily going away to make room for another
        /// screen that is on top of it, or it could be going away for good.
        /// This property indicates whether the screen is exiting for real:
        /// if set, the screen will automatically remove itself as soon as the
        /// transition finishes.
        /// </summary>
        
        public bool IsClosing
        {
            get { return this.m_IsClosing; }
            protected internal set { this.m_IsClosing = value; }
        }

        private bool m_IsClosing = false;

        public override void Update(GameTime gameTime)
        {
            bool doUpdate = true;
            switch (this.State)
            {
                case eScreenState.Activating:
                case eScreenState.Deactivating:
                case eScreenState.Closing:
                    this.UpdateTransition(gameTime);
                    break;
                case eScreenState.Active:
                    break;
                case eScreenState.Inactive:
                case eScreenState.Closed:
                    doUpdate = false;
                    break;
                default:
                    break;
            }

            if (doUpdate)
            {
                base.Update(gameTime);

                if (this.PreviousScreen != null && !this.IsModal)
                {
                    this.PreviousScreen.Update(gameTime);
                }
            }
        }

        /// <summary>
        /// Helper for updating the screen transition position.
        /// </summary>
        private void UpdateTransition(GameTime i_GameTime)
        {
            bool transionEnded = false;

            int direction = this.State == eScreenState.Activating ? 1 : -1;

            TimeSpan transitionLength = this.State == eScreenState.Activating ? this.m_ActivationLength : this.m_DeactivationLength;

            // How much should we move by?
            float transitionDelta;

            if (transitionLength == TimeSpan.Zero)
            {
                transitionDelta = 1;
            }
            else
            {
                transitionDelta = (float)(
                    i_GameTime.ElapsedGameTime.TotalMilliseconds
                    / transitionLength.TotalMilliseconds);
            }

            // Update the transition position.
            this.m_TransitionPosition += transitionDelta * direction;

            // Did we reach the end of the transition?
            if (((direction < 0) && (this.m_TransitionPosition <= 0)) ||
                ((direction > 0) && (this.m_TransitionPosition >= 1)))
            {
                this.m_TransitionPosition = MathHelper.Clamp(this.m_TransitionPosition, 0, 1);
                transionEnded = true;
            }

            if (transionEnded)
            {
                this.OnTransitionEnded();
            }
        }

        private void OnTransitionEnded()
        {
            switch (this.State)
            {
                case eScreenState.Inactive:
                case eScreenState.Activating:
                    this.State = eScreenState.Active;
                    break;
                case eScreenState.Active:
                case eScreenState.Deactivating:
                    this.State = eScreenState.Inactive;
                    break;
                case eScreenState.Closing:
                    this.State = eScreenState.Closed;
                    break;
            }
        }

        protected byte TransitionAlpha
        {
            get { return (byte)(byte.MaxValue * this.m_TransitionPosition * this.m_BlackTintAlpha); }
        }

        protected bool m_UseFadeTransition = true;

        public bool UseFadeTransition
        {
            get { return this.m_UseFadeTransition; }
            set { this.m_UseFadeTransition = value; }
        }

        #endregion Transitions Support
    }
}
