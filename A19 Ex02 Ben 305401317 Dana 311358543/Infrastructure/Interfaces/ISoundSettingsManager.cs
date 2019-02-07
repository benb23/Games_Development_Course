using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface ISoundSettingsManager
    {
        void ToggleGameSound_Clicked(object sender, EventArgs args);

        void DecreaseBackgroundMusic_Click(object sender, EventArgs args);

        void IncreaseBackgroundMusic_Click(object sender, EventArgs args);

        void DecreaseSoundEffects_Click(object sender, EventArgs args);

        void IncreaseSoundEffects_Click(object sender, EventArgs args);
    }
}
