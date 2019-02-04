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
    public interface IScreensMananger
    {
        GameScreen ActiveScreen { get; }

        void SetCurrentScreen(GameScreen i_NewScreen);

        bool Remove(GameScreen i_Screen);

        void Add(GameScreen i_Screen);

        GameScreen GetScreen(string i_ScreenName);

        void AddScreen(GameScreen i_Screen);
    }
}
