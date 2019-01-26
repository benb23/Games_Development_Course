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
    public class ToggleItem : MenuItem
    {

        // seperator texture
        //Texture2D 

        public ToggleItem(string i_AssetName, GameScreen i_GameScreen, int i_ItemNumber, string i_OptionsAssetName, int i_NumOfOptions) 
            : base(i_AssetName, i_GameScreen, i_ItemNumber)
        {
            
        }


    }
}
