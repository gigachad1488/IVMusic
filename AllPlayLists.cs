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

        public static List<Sound> GetPlayList(int i)
        {
            return playlists[i].GetPlayList();
        }

        public static void AddPlayList(PlayList playlist) 
        {
            playlists.Add(playlist);
        }
    }
}
