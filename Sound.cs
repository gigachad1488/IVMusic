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
        public string Name { get; set; }
        private string Path { get; set; }

        Mp3FileReader reader;
        WaveOut wave;

        public Sound()
        {

        }
        public Sound(string path)
        {
            Path = path;
            Name = path.Substring(path.LastIndexOf('\\') + 1, path.Length - path.LastIndexOf('\\') - 4);
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
                if (reader == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
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
                InitSound(Path);
                wave.Play();
            }
        }

        public void Stop()
        {
            if (wave != null && wave.PlaybackState == PlaybackState.Playing)
                wave.Stop();
        }
    }
}
