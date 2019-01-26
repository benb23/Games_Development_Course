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

        GameScreen m_NextScreen;

        public ClickItem(string i_AssetName, GameScreen i_GameScreen, int i_ItemNumber, GameScreen i_NextScreen) : base(i_AssetName, i_GameScreen, i_ItemNumber)
        {
            this.m_NextScreen = i_NextScreen;
        }

        public void ItemClicked()
        {
            this.GameScreen.ScreensManager.SetCurrentScreen(m_NextScreen);
        }
    }
}
