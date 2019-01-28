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
using Infrastructure;

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
        protected Dictionary<string, GameScreen> m_screens = new Dictionary<string, GameScreen>();
        private List<MenuItem> m_MenuItems = new List<MenuItem>();
        // private string m_TitleAsset;
        private Vector2 m_firstItemPosition;
        float m_GapBetweenItems;
        int? m_currItemNumber;

        private bool m_IsUsingKeyboardArrows = true;

        public MenuScreen(Game i_Game, Vector2 i_firstItemPosition, float i_GapBetweenItems) : base(i_Game)
        {
            this.m_GapBetweenItems = i_GapBetweenItems;
            this.m_firstItemPosition = i_firstItemPosition;
        }

        public bool IsUsingKeyboard
        {
            set { m_IsUsingKeyboardArrows = value; }
        }

        public override void Initialize()
        {
            if (m_MenuItems != null)
            {
                foreach (MenuItem item in m_MenuItems)
                {
                    item.Position = m_firstItemPosition + item.ItemNumber * (new Vector2(0, m_GapBetweenItems + 33));   //TODO: change 33
                    if (!m_IsUsingKeyboardArrows && item is ClickItem)
                    {
                        (item as ClickItem).IsUsingKeyboard = false;
                    }
                }
                
                if (m_IsUsingKeyboardArrows)
                {
                    m_currItemNumber = 0;
                }
            }
            base.Initialize();
        }

        // TODO: change the way that item added to list
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
            foreach (MenuItem item in m_MenuItems)
            {
                if (item.isMouseHoverItem())
                {
                    m_currItemNumber = item.ItemNumber;
                }
                else if(item.IsActive == true)
                {
                    item.IsActive = false;
                }
            }
        }

        private void updateCurrActiveItem()
        {
            if (m_IsUsingKeyboardArrows)
            {
                useKeyboardToNavigateMenu();
            }
            useMouseToNavigateMenu();
        }

        public override void Update(GameTime gameTime)
        {
            updateCurrActiveItem();

            if (m_currItemNumber != null)
            {
                if (!m_MenuItems[(int)m_currItemNumber].isMouseHoverItem() && !m_IsUsingKeyboardArrows)
                {
                    m_MenuItems[(int)m_currItemNumber].IsActive = false;
                }
                else
                {
                    m_MenuItems[(int)m_currItemNumber].IsActive = true;
                }
            }
            base.Update(gameTime);
        }

        // mutual menuScreens methods
    }
}
