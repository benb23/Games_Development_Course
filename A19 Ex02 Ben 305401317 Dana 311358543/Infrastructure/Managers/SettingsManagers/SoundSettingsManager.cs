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
    public class SoundSettingsManager : GameService, ISoundSettingsManager
    {

        public SoundSettingsManager(Game i_Game) : base (i_Game)
        {
            
        }
        
        public void ToggleGameSound()
        {

        }


    }
}
