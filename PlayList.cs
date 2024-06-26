﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Media;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IVMusic
{
    class PlayList
    {
        public List<Sound> sounds = new List<Sound>();
        public BitmapImage image;
        public string Title { get; set; }

        public int Count
        {
            get
            {
                return sounds.Count;
            }
        }

        public PlayList()
        {

        }

        public void AddSound(Sound sound)
        {
            sounds.Add(sound);
        }
     
        public Sound GetSound(int i)
        {
            return sounds[i];
        }

        public void PlayRandomSong()
        {
            Random rand = new Random();
            sounds[rand.Next(0, sounds.Count)].Play();
        }
    }
}
