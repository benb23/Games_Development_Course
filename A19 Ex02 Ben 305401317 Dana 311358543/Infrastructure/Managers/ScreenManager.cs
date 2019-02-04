////*** Guy Ronen © 2008-2015 ***//
using System;
using System.Collections.Generic;
using Infrastructure;
using Microsoft.Xna.Framework;

namespace Infrastructure
{
    public class ScreensMananger : CompositeDrawableComponent<GameScreen>, IScreensMananger
    {
        public ScreensMananger(Game i_Game)
            : base(i_Game)
        {
            i_Game.Components.Add(this);
        }

        protected Dictionary<string, GameScreen> m_screens = new Dictionary<string, GameScreen>();

        public GameScreen GetScreen(string i_ScreenName)
        {
            return this.m_screens[i_ScreenName];
        }

        private Stack<GameScreen> m_ScreensStack = new Stack<GameScreen>();

        public GameScreen ActiveScreen
        {
            get { return this.m_ScreensStack.Count > 0 ? this.m_ScreensStack.Peek() : null; }
        }

        public void SetCurrentScreen(GameScreen i_GameScreen)
        {
            this.Push(i_GameScreen);
            i_GameScreen.Activate();
        }

        public void Push(GameScreen i_GameScreen)
        {
            // hello new screen, I am your manager, nice to meet you:
            i_GameScreen.ScreensManager = this;

            if (!this.Contains(i_GameScreen))
            {
                this.Add(i_GameScreen);

                // let me know when you are closed, so i can pop you from the stack:
                i_GameScreen.StateChanged += this.Screen_StateChanged;
            }

            if (this.ActiveScreen != i_GameScreen)
            {
                if (this.ActiveScreen != null)
                {
                    // connect each new screen to the previous one:
                    i_GameScreen.PreviousScreen = this.ActiveScreen;

                    this.ActiveScreen.Deactivate();
                }
            }

            if (this.ActiveScreen != i_GameScreen)
            {
                this.m_ScreensStack.Push(i_GameScreen);
            }

            i_GameScreen.DrawOrder = this.m_ScreensStack.Count;
        }

        private void Screen_StateChanged(object sender, StateChangedEventArgs e)
        {
            switch (e.CurrentState)
            {
                case eScreenState.Activating:
                    break;
                case eScreenState.Active:
                    break;
                case eScreenState.Deactivating:
                    break;
                case eScreenState.Closing:
                    this.Pop(sender as GameScreen);
                    break;
                case eScreenState.Inactive:
                    break;
                case eScreenState.Closed:
                    this.Remove(sender as GameScreen);
                    break;
                default:
                    break;
            }

            this.OnScreenStateChanged(sender, e);
        }

        private void Pop(GameScreen i_GameScreen)
        {
            this.m_ScreensStack.Pop();

            if (this.m_ScreensStack.Count > 0)
            {
                // when one is popped, the previous becomes the active one
                this.ActiveScreen.Activate();
            }
        }

        private new bool Remove(GameScreen i_Screen)
        {
            return base.Remove(i_Screen);
        }

        private new void Add(GameScreen i_Component)
        {
            base.Add(i_Component);
        }

        public void AddScreen(GameScreen i_Screen)
        {
            this.m_screens.Add(i_Screen.ToString(), i_Screen);
        }

        public event EventHandler<StateChangedEventArgs> ScreenStateChanged;

        protected virtual void OnScreenStateChanged(object sender, StateChangedEventArgs e)
        {
            if (this.ScreenStateChanged != null)
            {
                this.ScreenStateChanged(sender, e);
            }
        }

        protected override void OnComponentRemoved(GameComponentEventArgs<GameScreen> e)
        {
            base.OnComponentRemoved(e);

            e.GameComponent.StateChanged -= this.Screen_StateChanged;

            if (this.m_ScreensStack.Count == 0)
            {
                this.Game.Exit();
            }
        }

        public override void Initialize()
        {
            this.Game.Services.AddService(typeof(IScreensMananger), this);
            base.Initialize();
        }
    }
}
