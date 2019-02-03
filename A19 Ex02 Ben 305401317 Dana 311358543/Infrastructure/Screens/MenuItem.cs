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



        public Color  ActiveColor
        {
            get { return m_ActiveColor;  }
            set { m_ActiveColor = value; }
        }
        
        public MenuItem(string i_AssetName, GameScreen i_GameScreen, int i_ItemNumber) : base(i_AssetName, i_GameScreen)
        {
            this.m_GameScreen = i_GameScreen;
            this.m_ItemNumber = i_ItemNumber;
        }

        public int ItemNumber
        {
            get { return m_ItemNumber; }
        }

        public bool IsActive
        {
            get { return m_IsActive; }
            set
            {
                m_IsActive = value;

                if (ActiveChanged != null)
                {
                    OnActiveChanged(this, null);
                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            ActiveChanged += new EventHandler<EventArgs>(OnActiveChanged);

            if(m_isSoundOn)
            {
                m_SoundManager = m_GameScreen.Game.Services.GetService(typeof(ISoundMananger)) as ISoundMananger;
            }
            initAnimations();
        }

        public bool isMouseHoverItem()
        {
            return this.Bounds.Contains(new Vector2(m_GameScreen.InputManager.MouseState.X, m_GameScreen.InputManager.MouseState.Y));
        }

        private void initAnimations()
        {
            PulseAnimator pulsAnimator = new PulseAnimator("ActiveItem", TimeSpan.Zero, (float)1.1 ,1);
            this.Animations.Add(pulsAnimator);
        }

        private void OnActiveChanged(object sender, EventArgs e)
        {
            if (m_IsActive)
            {
                if (!this.Animations["ActiveItem"].Enabled)
                {
                    this.Animations["ActiveItem"].Restart();
                }

                this.TintColor = m_ActiveColor;

                if (m_isSoundOn)
                {
                    this.m_SoundManager.PlaySoundEffect(m_SoundOnHover);
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
