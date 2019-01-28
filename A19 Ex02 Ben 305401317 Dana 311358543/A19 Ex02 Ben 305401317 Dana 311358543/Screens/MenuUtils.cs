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

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
    public static class MenuUtils 
    {
        public static void GoToScreen(GameScreen i_SourceScreen, GameScreen i_TargetScreen)
        {
            i_SourceScreen.ExitScreen();
            i_SourceScreen.ScreensManager.SetCurrentScreen(i_TargetScreen);
            if (i_TargetScreen is PlayScreen)
            {
                i_SourceScreen.ScreensManager.SetCurrentScreen(new LevelTransitionScreencs(i_SourceScreen.Game));
            }
        }

        //public static void OnItemClicked(GameScreen i_GameScreen)
        //{
        //    i_GameScreen.Game.Exit();
        //}
    }
}
