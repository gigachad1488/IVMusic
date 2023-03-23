using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IVMusic
{
    class PlayList
    {
        public List<Sound> sounds = new List<Sound>();
        public Image image;
        public string Title { get; set; }

        public PlayList()
        {

        }

        public List<Sound> GetPlayList()
        {
            return this.sounds;
        }

        public void AddSound(Sound sound)
        {
            sounds.Add(sound);
        }

        public void PlayRandomSong()
        {
            Random rand = new Random();
            sounds[rand.Next(0, sounds.Count)].Play();
        }
    }
}
