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

        public Dictionary<string, SoundEffectInstance> SoundEffect
        {
            get { return m_SoundsEffects; }
        }

        public SoundManager(Game i_Game) : base(i_Game)
        {

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
