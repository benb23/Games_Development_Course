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
    public class ClickItem : MenuItem
    {
        //private GameScreen m_NextScreen;
        //private bool m_IsExitItem;
        private string m_ItemName;
        private bool m_IsUsingKeyboard = true;

        public event EventHandler<ScreenEventArgs> ItemClicked;

        public bool IsUsingKeyboard
        {
            set { m_IsUsingKeyboard = value; }
        }
        protected virtual void OnItemClicked(object sender, EventArgs args)
        {
            if (ItemClicked != null)
            {
                ItemClicked.Invoke(sender, new ScreenEventArgs(m_ItemName));
            }
        }

        //public ClickItem(string i_AssetName, GameScreen i_GameScreen, int i_ItemNumber, GameScreen i_NextScreen) : base(i_AssetName, i_GameScreen, i_ItemNumber)
        //{
        //    this.m_NextScreen = i_NextScreen;
        //}

        public ClickItem(string i_ItemName, string i_AssetName, GameScreen i_GameScreen, int i_ItemNumber) : base(i_AssetName, i_GameScreen, i_ItemNumber)
        {
            this.m_ItemName = i_ItemName;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.IsActive)
            {
                if ((this.GameScreen.InputManager.KeyPressed(Keys.Enter) && this.m_IsUsingKeyboard)|| this.GameScreen.InputManager.ButtonPressed(eInputButtons.Left))
                {
                    OnItemClicked(this, EventArgs.Empty);
                }
            }
            base.Update(gameTime);
        }
    }
}
