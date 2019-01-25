using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
       // private eMenuItemType m_ItemType;
        private int m_ItemNumber;
        bool m_IsActive;


        public MenuItem(string i_AssetName, MenuScreen i_MenuScreen, int i_ItemNumber) : base(i_AssetName, i_MenuScreen)
        {
            this.m_ItemNumber = i_ItemNumber;
            i_MenuScreen.AddMenuItem(this);
        }

        public int ItemNumber
        {
            get { return m_ItemNumber; }
        }

        public bool isActive
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

            base.Initialize();
        }
        

        private void OnActiveChanged(object sender, EventArgs e)
        {
            if (m_IsActive)
            {
                this.TintColor = Color.Green;
            }
            else
            {
                this.TintColor = Color.White;
            }



        }
    }
}
