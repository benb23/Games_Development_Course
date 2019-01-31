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
        ISoundMananger m_SoundManager;

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(ISoundSettingsManager), this);
        }


        public SoundSettingsManager(Game i_Game) : base (i_Game)
        {
            m_SoundManager = i_Game.Services.GetService(typeof(ISoundMananger)) as ISoundMananger;
        }

        public void ToggleGameSound(object sender, EventArgs args)
        {
            MediaPlayer.IsMuted = !MediaPlayer.IsMuted;
            
            m_SoundManager.IsGameSoundOn = !m_SoundManager.IsGameSoundOn;
        }

        private void revertSoundVolumes()
        {
            throw new NotImplementedException();
        }

        // TODO: cheak if we need to limit the volume 
        public void DecreaseBackgroundMusicVolume(object sender, EventArgs args)
        {
            MediaPlayer.Volume = MathHelper.Clamp(MediaPlayer.Volume - 0.1f, 0, 1); 

           
        }

        public void IncreaseBackgroundMusicVolume(object sender, EventArgs args)
        {
            MediaPlayer.Volume = MathHelper.Clamp(MediaPlayer.Volume + 0.1f, 0, 1);
        }

        public void DecreaseSoundEffectsVolume(object sender, EventArgs args)
        {
            foreach (SoundEffectInstance effect in m_SoundManager.SoundEffect.Values)
            {
                effect.Volume = MathHelper.Clamp(effect.Volume - 0.1f, 0, 1);
            }
        }

        public void IncreaseSoundEffectsVolume(object sender, EventArgs args)
        {
            foreach (SoundEffectInstance effect in m_SoundManager.SoundEffect.Values)
            {
                effect.Volume = MathHelper.Clamp(effect.Volume + 0.1f, 0, 1);

            }
        }

    }
}
