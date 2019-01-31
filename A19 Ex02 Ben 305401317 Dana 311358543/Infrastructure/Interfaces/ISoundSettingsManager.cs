using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface ISoundSettingsManager
    {


        void ToggleGameSound(object sender, EventArgs args);

        bool IsGameSoundOn { get; }

        // TODO: cheak if we need to limit the volume 
        void DecreaseBackgroundMusicVolume(object sender, EventArgs args);

        void IncreaseBackgroundMusicVolume(object sender, EventArgs args);

        void DecreaseSoundEffectsVolume(object sender, EventArgs args);

        void IncreaseSoundEffectsVolume(object sender, EventArgs args);


    }
}
