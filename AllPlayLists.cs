using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVMusic
{
    static class AllPlayLists
    {
        private static List<PlayList> playlists = new List<PlayList>();

        public static int Count
        {
            get
            {
                return playlists.Count;
            }
        }

        public static PlayList GetPlayList(int i)
        {
            return playlists[i];
        }

        public static void AddPlayList(PlayList playlist) 
        {
            playlists.Add(playlist);
        }
    }
}
