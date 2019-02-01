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
    public class SoundManager : GameService, ISoundMananger
    {
        Dictionary<string, SoundEffectInstance> m_SoundsEffects = new Dictionary<string, SoundEffectInstance>();
        Dictionary<string, Song> m_Songs = new Dictionary<string, Song>();
        private bool m_isGameSoundOn = true;

        public bool IsGameSoundOn
        {
            get { return m_isGameSoundOn; }
            set { m_isGameSoundOn = value; }
        }

        public Dictionary<string, SoundEffectInstance> SoundEffects
        {
            get { return m_SoundsEffects; }
        }

        public Dictionary<string, Song> Songs
        {
            get { return m_Songs; }
        }

        public SoundManager(Game i_Game) : base(i_Game)
        {

        }

        public void PlaySoundEffect(string i_SoundEffect)
        {
            if (m_isGameSoundOn)
            {
                this.m_SoundsEffects[i_SoundEffect].Play();
            }
        }

        protected override void RegisterAsService()
        {
            Game.Services.AddService(typeof(ISoundMananger), this);
        }

        public void AddSoundEffect(SoundEffect i_SoundEffect, string i_SoundName)
        {
            m_SoundsEffects.Add(i_SoundName, i_SoundEffect.CreateInstance());
        }
       
        public void AddSong(Song i_Song, string i_SongName)
        {
            m_Songs.Add(i_SongName, i_Song);
        }

        public void RemoveSoundEffect(string i_SoundName)
        {
            m_SoundsEffects.Remove(i_SoundName);
        }

        public void RemoveSong(string i_SongName)
        {
            m_Songs.Remove(i_SongName);
        }
        
        public Song GetSong(string i_Name)
        {
            return m_Songs[i_Name];
        }

        public SoundEffectInstance GetSoundEffect(string i_Name)
        {
            return m_SoundsEffects[i_Name];
        }
        
    }
}
