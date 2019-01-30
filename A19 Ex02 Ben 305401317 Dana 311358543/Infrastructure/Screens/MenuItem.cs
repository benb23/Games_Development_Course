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
    public class MenuItem : Sprite 
    {
        
        private enum eMenuItemType
        {
            RegularButton,
            ToggleButton,
            VolumeButton
        }

        private GameScreen m_GameScreen;
        public event EventHandler<EventArgs> ActiveChanged;
        ISoundMananger m_SoundManager;
        private bool m_isSoundOn = true;
        private string m_SoundOnHover = "MenuMove";


        public bool IsSoundOn
        {
            set { m_isSoundOn = value; }
        }

        // private eMenuItemType m_ItemType;
        private int m_ItemNumber;
        bool m_IsActive;
        //bool m_IsMouseHover;
        
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
            if (m_ItemNumber == 0)
            {
                m_IsActive = true;
            }

            ActiveChanged += new EventHandler<EventArgs>(OnActiveChanged);
            if(m_isSoundOn)
            {
                m_SoundManager = m_GameScreen.Game.Services.GetService(typeof(ISoundMananger)) as ISoundMananger;
            }

            base.Initialize();
        }

        public bool isMouseHoverItem()
        {
            return this.Bounds.Contains(new Vector2(m_GameScreen.InputManager.MouseState.X, m_GameScreen.InputManager.MouseState.Y));
        }

        private void OnActiveChanged(object sender, EventArgs e)
        {
            if (m_IsActive)
            {
                this.TintColor = new Color(255, 74, 47);
                if (m_isSoundOn)
                {
                    m_SoundManager.GetSoundEffect(m_SoundOnHover).Play();
                }
                
            }
            else
            {
                this.TintColor = Color.White;
            }
        }

    }
}
