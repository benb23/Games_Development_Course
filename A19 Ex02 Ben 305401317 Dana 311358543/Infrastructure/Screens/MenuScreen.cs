using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure
{
    public class ScreenEventArgs : EventArgs
    {
        public ScreenEventArgs(string i_String)
        {
            ScreenName = i_String;
        }
        public string ScreenName { get; set; }
    }

    public abstract class MenuScreen : GameScreen
    {
        private Game m_Game;
        private List<MenuItem> m_MenuItems = new List<MenuItem>();
        private Vector2 m_firstItemPosition;
        private float m_GapBetweenItems = 15f;
        private float m_OffsetX;
        private float m_OffsetY;
        private int? m_currItemNumber;
        private int? m_PrevItemNumber;

        private bool m_IsUsingKeyboardArrows = true;

        private bool m_IsUsingMouse = true;

        public MenuScreen(Game i_Game) : base(i_Game)
        {
            m_Game = i_Game;
        }

        public MenuScreen(Game i_Game, float i_OffsetX, float i_GapBetweenItems) : base(i_Game)
        {
            m_Game = i_Game;
            this.m_GapBetweenItems = i_GapBetweenItems;
            this.m_OffsetX = i_OffsetX;
        }

        public MenuScreen(Game i_Game, float i_OffsetX, float i_OffsetY, float i_GapBetweenItems) : base(i_Game)
        {
            m_Game = i_Game;
            this.m_GapBetweenItems = i_GapBetweenItems;
            this.m_OffsetX = i_OffsetX;
            this.m_OffsetY = i_OffsetY;
        }

        public bool IsUsingKeyboard
        {
            set { m_IsUsingKeyboardArrows = value; }
        }

        public bool IsUsingMouse
        {
            get { return m_IsUsingMouse; }
            set { m_IsUsingMouse = value; }
        }

        public override void Initialize()
        {
            this.m_Game.Window.ClientSizeChanged += updatePositionsAfterWindowSizeChanged;
            initFirstItemPosition();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            initMenu();
            base.LoadContent();
        }

        private void updatePositionsAfterWindowSizeChanged(object sender, EventArgs e)
        {
            initFirstItemPosition();
            initItemsPositions();
        }

        private void initFirstItemPosition()
        {
            m_firstItemPosition = new Vector2(m_Game.Window.ClientBounds.Width / 3 - m_OffsetX, m_Game.Window.ClientBounds.Height / 2.5f + m_OffsetY);
        }

        private void initMenu()
        {
            initItemsPositions();
            updateFirstItemAsActive();
        }

        private void initItemsPositions()
        {
            if (m_MenuItems != null)
            {
                foreach (MenuItem item in m_MenuItems)
                {
                    item.Position = m_firstItemPosition + item.ItemNumber * (new Vector2(0, m_GapBetweenItems + item.Texture.Height));
                    if (!m_IsUsingKeyboardArrows && item is ClickItem)
                    {
                        (item as ClickItem).IsUsingKeyboard = false;
                    }
                }
            }
        }

        private void updateFirstItemAsActive()
        {
            if (m_MenuItems != null && m_IsUsingKeyboardArrows)
            {
                    m_currItemNumber = 0;
                    m_MenuItems[(int)m_currItemNumber].IsActive = true;
                    m_MenuItems[(int)m_currItemNumber].TintColor = m_MenuItems[(int)m_currItemNumber].ActiveColor;
            }
        }
        public void AddMenuItem(MenuItem i_Item)        
        {
            m_MenuItems.Add(i_Item);
        }

        private void useKeyboardToNavigateMenu()
        {
            if (InputManager.KeyPressed(Keys.Down))
            {
                m_currItemNumber = (m_currItemNumber + 1) % m_MenuItems.Count;
            }
            else if (InputManager.KeyPressed(Keys.Up))
            {
                if (m_currItemNumber == 0)
                {
                    m_currItemNumber = m_MenuItems.Count - 1;
                }
                else
                {
                    m_currItemNumber = (m_currItemNumber - 1) % m_MenuItems.Count;
                }
            }
        }

        private void useMouseToNavigateMenu()
        {
            bool foundActiveItem = false;

            foreach (MenuItem item in m_MenuItems)
            {
                if (item.isMouseHoverItem())
                {
                    m_currItemNumber = item.ItemNumber;
                    foundActiveItem = true;
                    break;
                }
                else if (item.IsActive == true && m_currItemNumber != item.ItemNumber)
                {
                    item.IsActive = false;
                }
            }
            if (!foundActiveItem && !m_IsUsingKeyboardArrows)
            {
                m_currItemNumber = null;
            }
        }

        private void updateCurrActiveItem()
        {
            if (m_IsUsingKeyboardArrows)
            {
                useKeyboardToNavigateMenu();
            }
            if (m_IsUsingMouse)
            {
                useMouseToNavigateMenu();
            }
        }

        public override void Update(GameTime gameTime)
        {
            updateCurrActiveItem();

            if (m_currItemNumber != m_PrevItemNumber)
            {
                if (m_PrevItemNumber != null)
                {
                    m_MenuItems[(int)m_PrevItemNumber].IsActive = false;
                }
                if (m_currItemNumber != null)
                {
                    m_MenuItems[(int)m_currItemNumber].IsActive = true;
                }
            }
            m_PrevItemNumber = m_currItemNumber;
            base.Update(gameTime);
        }
    }
}
