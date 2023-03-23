using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace IVMusic
{
    class Sound
    {
        Mp3FileReader reader;
        WaveOut wave;

        public Sound(string path = "3")
        {
            InitSound(path);
        }

        private void InitSound(string path)
        {
            reader = new Mp3FileReader(path);
            wave = new WaveOut();
            wave.Init(reader);
        }

        public void Play()
        {
            if (wave != null && wave.PlaybackState == PlaybackState.Stopped)
            {
                wave.Play();
            }
        }

        public void Stop()
        {
            if (wave != null && wave.PlaybackState == PlaybackState.Playing)
                wave.Stop();
        }

        public void ChangeVolume(int vol)
        {
            wave.Volume = vol;
        }
    }
}
