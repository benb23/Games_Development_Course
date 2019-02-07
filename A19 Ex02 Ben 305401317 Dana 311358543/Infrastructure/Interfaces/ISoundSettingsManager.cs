using System;

namespace Infrastructure
{
    public interface ISoundSettingsManager
    {
        void ToggleGameSound_Click(object sender, EventArgs args);

        void DecreaseBackgroundMusic_Click(object sender, EventArgs args);

        void IncreaseBackgroundMusic_Click(object sender, EventArgs args);

        void DecreaseSoundEffects_Click(object sender, EventArgs args);

        void IncreaseSoundEffects_Click(object sender, EventArgs args);
    }
}
