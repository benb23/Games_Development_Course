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
            this.ScreenName = i_String;
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
            this.m_Game = i_Game;
        }

        public MenuScreen(Game i_Game, float i_OffsetX, float i_GapBetweenItems) : base(i_Game)
        {
            this.m_Game = i_Game;
            this.m_GapBetweenItems = i_GapBetweenItems;
            this.m_OffsetX = i_OffsetX;
        }

        public MenuScreen(Game i_Game, float i_OffsetX, float i_OffsetY, float i_GapBetweenItems) : base(i_Game)
        {
            this.m_Game = i_Game;
            this.m_GapBetweenItems = i_GapBetweenItems;
            this.m_OffsetX = i_OffsetX;
            this.m_OffsetY = i_OffsetY;
        }

        public bool IsUsingKeyboard
        {
            set { this.m_IsUsingKeyboardArrows = value; }
        }

        public bool IsUsingMouse
        {
            get { return this.m_IsUsingMouse; }
            set { this.m_IsUsingMouse = value; }
        }

        public override void Initialize()
        {
            this.m_Game.Window.ClientSizeChanged += this.WindowSize_Changed;
            this.initFirstItemPosition();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.initMenu();
            base.LoadContent();
        }

        private void WindowSize_Changed(object sender, EventArgs e)
        {
            this.initFirstItemPosition();
            this.initItemsPositions();
        }

        private void initFirstItemPosition()
        {
            this.m_firstItemPosition = new Vector2((this.m_Game.Window.ClientBounds.Width / 3) - this.m_OffsetX, (this.m_Game.Window.ClientBounds.Height / 2.5f) + this.m_OffsetY);
        }

        private void initMenu()
        {
            this.initItemsPositions();
            this.updateFirstItemAsActive();
        }

        private void initItemsPositions()
        {
            if (this.m_MenuItems != null)
            {
                foreach (MenuItem item in this.m_MenuItems)
                {
                    item.Position = this.m_firstItemPosition + (item.ItemNumber * (new Vector2(0, this.m_GapBetweenItems + item.Texture.Height)));
                    if (!this.m_IsUsingKeyboardArrows && item is ClickItem)
                    {
                        (item as ClickItem).IsUsingKeyboard = false;
                    }
                }
            }
        }

        private void updateFirstItemAsActive()
        {
            if (this.m_MenuItems != null && this.m_IsUsingKeyboardArrows)
            {
                    this.m_currItemNumber = 0;
                    this.m_MenuItems[(int)this.m_currItemNumber].IsActive = true;
                    this.m_MenuItems[(int)this.m_currItemNumber].TintColor = this.m_MenuItems[(int)this.m_currItemNumber].ActiveColor;
            }
        }

        public void AddMenuItem(MenuItem i_Item)        
        {
            this.m_MenuItems.Add(i_Item);
        }

        private void useKeyboardToNavigateMenu()
        {
            if (this.InputManager.KeyPressed(Keys.Down))
            {
                this.m_currItemNumber = (this.m_currItemNumber + 1) % this.m_MenuItems.Count;
            }
            else if (this.InputManager.KeyPressed(Keys.Up))
            {
                if (this.m_currItemNumber == 0)
                {
                    this.m_currItemNumber = this.m_MenuItems.Count - 1;
                }
                else
                {
                    this.m_currItemNumber = (this.m_currItemNumber - 1) % this.m_MenuItems.Count;
                }
            }
        }

        private void useMouseToNavigateMenu()
        {
            bool foundActiveItem = false;

            foreach (MenuItem item in this.m_MenuItems)
            {
                if (item.isMouseHoverItem())
                {
                    this.m_currItemNumber = item.ItemNumber;
                    foundActiveItem = true;
                    break;
                }
                else if (item.IsActive == true && this.m_currItemNumber != item.ItemNumber)
                {
                    item.IsActive = false;
                }
            }

            if (!foundActiveItem && !this.m_IsUsingKeyboardArrows)
            {
                this.m_currItemNumber = null;
            }
        }

        private void updateCurrActiveItem()
        {
            if (this.m_IsUsingKeyboardArrows)
            {
                this.useKeyboardToNavigateMenu();
            }

            if (this.m_IsUsingMouse)
            {
                this.useMouseToNavigateMenu();
            }
        }

        public override void Update(GameTime gameTime)
        {
            this.updateCurrActiveItem();

            if (this.m_currItemNumber != this.m_PrevItemNumber)
            {
                if (this.m_PrevItemNumber != null)
                {
                    this.m_MenuItems[(int)this.m_PrevItemNumber].IsActive = false;
                }

                if (this.m_currItemNumber != null)
                {
                    this.m_MenuItems[(int)this.m_currItemNumber].IsActive = true;
                }
            }

            this.m_PrevItemNumber = this.m_currItemNumber;
            base.Update(gameTime);
        }
    }
}
