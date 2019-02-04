using System;
using Microsoft.Xna.Framework;

namespace Infrastructure
{
    public class MenuItem : Sprite 
    {
        private enum eMenuItemType
        {
            RegularButton,
            ToggleButton,
            VolumeButton
        }

        public event EventHandler<EventArgs> ActiveChanged;

        protected ISoundMananger m_SoundManager;
        private bool m_isSoundOn = true;
        private string m_SoundOnHover = "MenuMove";
        private int m_ItemNumber;
        private bool m_IsActive;
        private Color m_ActiveColor = new Color(255, 74, 47);

        public Color ActiveColor
        {
            get { return this.m_ActiveColor;  }
            set { this.m_ActiveColor = value; }
        }
        
        public MenuItem(string i_AssetName, GameScreen i_GameScreen, int i_ItemNumber) : base(i_AssetName, i_GameScreen)
        {
            this.m_GameScreen = i_GameScreen;
            this.m_ItemNumber = i_ItemNumber;
        }

        public int ItemNumber
        {
            get { return this.m_ItemNumber; }
        }

        public bool IsActive
        {
            get { return this.m_IsActive; }
            set
            {
                this.m_IsActive = value;

                if (this.ActiveChanged != null)
                {
                    this.ItemActiveChanged(this, null);
                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            this.ActiveChanged += new EventHandler<EventArgs>(this.ItemActiveChanged);

            if(this.m_isSoundOn)
            {
                this.m_SoundManager = this.m_GameScreen.Game.Services.GetService(typeof(ISoundMananger)) as ISoundMananger;
            }

            this.initAnimations();
        }

        public bool isMouseHoverItem()
        {
            return this.Bounds.Contains(new Vector2(this.m_GameScreen.InputManager.MouseState.X, this.m_GameScreen.InputManager.MouseState.Y));
        }

        private void initAnimations()
        {
            PulseAnimator pulsAnimator = new PulseAnimator("ActiveItem", TimeSpan.Zero, (float)1.1, 1);
            this.Animations.Add(pulsAnimator);
        }

        private void ItemActiveChanged(object sender, EventArgs e)
        {
            if (this.m_IsActive)
            {
                if (!this.Animations["ActiveItem"].Enabled)
                {
                    this.Animations["ActiveItem"].Restart();
                }

                this.TintColor = this.m_ActiveColor;

                if (this.m_isSoundOn)
                {
                    this.m_SoundManager.PlaySoundEffect(this.m_SoundOnHover);
                }
            }
            else
            {
                if (this.Animations["ActiveItem"].Enabled)
                {
                    this.Animations["ActiveItem"].Pause();
                    this.Animations["ActiveItem"].Reset();
                }

                this.TintColor = Color.White;
            }
        }
    }
}
