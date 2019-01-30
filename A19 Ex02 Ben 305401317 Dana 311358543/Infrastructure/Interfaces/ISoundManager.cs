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
    public interface ISoundMananger
    {
        Dictionary<string, SoundEffectInstance> SoundEffect { get; }
        void AddSoundEffect(SoundEffect i_SoundEffect, string i_SoundName);
        void AddSong(Song i_Song, string i_SongName);
        Song GetSong(string i_Name);
        SoundEffectInstance GetSoundEffect(string i_Name);
    }
}
