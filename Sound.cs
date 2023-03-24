﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NAudio.Wave;

namespace IVMusic
{
    class Sound
    {
        public string Name { get; set; }
        private string Path { get; set; }

        public TimeSpan Duration { get; set; }

        public TimeSpan Time
        {
            get
            {
                return reader.CurrentTime;
            }
            set
            {
                reader.CurrentTime = value;
            }
        }

        Mp3FileReader reader;
        WaveOut wave;

        public Sound()
        {

        }
        public Sound(string path)
        {
            SaveSound(path);
        }

        public void SaveSound(string path)
        {
            Path = path;
            Name = path.Substring(path.LastIndexOf('\\') + 1, path.Length - path.LastIndexOf('\\') - 5);
            reader = new Mp3FileReader(path);
            Duration = reader.TotalTime;

        }

        public float Volume
        {
            get
            {
                return wave.Volume;
            }
            set
            {
                wave.Volume = (float)value;
            }
        }


        public bool IsInit
        {
            get
            {
                if (wave == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }



        private void InitSound()
        {
            wave = new WaveOut();
            wave.Init(reader);
        }

        public void Play()
        {
            if (wave == null)
            {
                InitSound();
            }
            if (wave.PlaybackState == PlaybackState.Stopped)
            {
                wave.Play();
            }
        }

        public void Stop()
        {
            if (wave.PlaybackState == PlaybackState.Playing)
                wave.Stop();
        }

        public void Reset()
        {
            reader.CurrentTime = new TimeSpan(0);
            wave.Dispose();
            wave = null;
        }
    }
}
