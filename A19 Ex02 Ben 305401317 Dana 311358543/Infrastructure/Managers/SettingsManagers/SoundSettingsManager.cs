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
        private ISoundMananger m_SoundManager;

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(ISoundSettingsManager), this);
        }

        public SoundSettingsManager(Game i_Game) : base(i_Game)
        {
            this.m_SoundManager = i_Game.Services.GetService(typeof(ISoundMananger)) as ISoundMananger;
        }

        public void ToggleGameSound_Click(object sender, EventArgs args)
        {
            MediaPlayer.IsMuted = !MediaPlayer.IsMuted;
            this.m_SoundManager.IsGameSoundOn = !this.m_SoundManager.IsGameSoundOn;
        }

        public void DecreaseBackgroundMusic_Click(object sender, EventArgs args)
        {
            MediaPlayer.Volume = MathHelper.Clamp(MediaPlayer.Volume - 0.1f, 0, 1); 
        }

        public void IncreaseBackgroundMusic_Click(object sender, EventArgs args)
        {
            MediaPlayer.Volume = MathHelper.Clamp(MediaPlayer.Volume + 0.1f, 0, 1);
        }

        public void DecreaseSoundEffects_Click(object sender, EventArgs args)
        {
            foreach (SoundEffectInstance effect in this.m_SoundManager.SoundEffects.Values)
            {
                effect.Volume = MathHelper.Clamp(effect.Volume - 0.1f, 0, 1);
            }
        }

        public void IncreaseSoundEffects_Click(object sender, EventArgs args)
        {
            foreach (SoundEffectInstance effect in this.m_SoundManager.SoundEffects.Values)
            {
                effect.Volume = MathHelper.Clamp(effect.Volume + 0.1f, 0, 1);
            }
        }
    }
}
