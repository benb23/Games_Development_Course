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
        private GameScreen m_NextScreen;
        private bool m_IsExitItem;

        public ClickItem(string i_AssetName, GameScreen i_GameScreen, int i_ItemNumber, GameScreen i_NextScreen) : base(i_AssetName, i_GameScreen, i_ItemNumber)
        {
            this.m_NextScreen = i_NextScreen;
        }

        //exit item
        public ClickItem(string i_AssetName, GameScreen i_GameScreen, int i_ItemNumber) : base(i_AssetName, i_GameScreen, i_ItemNumber)
        {
            this.m_IsExitItem = true;

    }

    public void ItemClicked()
        {
            if (!m_IsExitItem)
            {
                this.GameScreen.ScreensManager.SetCurrentScreen(m_NextScreen);
            }
            else
            {
                this.Game.Exit();
            }
        }
    }
}
