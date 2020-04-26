using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mp3Player
{
    public class ListViewAudioItem
    {
        public string FileName { get; set; }

        public string FileFullPath { get; set; }

        public string FileDurationString { get; set; }

        public int FileDurationInSeconds{ get; set; }
    }
}
