using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace mp3Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isPaused = false;
        private List<string> audioFiles = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            MusicPlayerMediaElement.Play();
            isPaused = false;
        }

        private void MusicPlayerMediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            int a = 10;
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            MusicPlayerMediaElement.Stop();
        }

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isPaused)
            {
                MusicPlayerMediaElement.Play();
                isPaused = false;
            }
            else
            {
                MusicPlayerMediaElement.Pause();
                isPaused = true;
            }
        }

        private void OpenFileDialogBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Supported Audio | *.mp3; *.wma | MP3s | *.mp3 | WMAs | *.wma";
            openFileDialog.Multiselect = true;
            openFileDialog.ShowDialog();

            audioFiles = openFileDialog.FileNames.ToList();
            preparePlaylistPanel();
        }

        private void preparePlaylistPanel()
        {
            //List<ListViewAudioItem> preparedAudioList = new List<ListViewAudioItem>();

            foreach (var audioFile in audioFiles)
            {
                TagLib.File f = TagLib.File.Create(audioFile, TagLib.ReadStyle.Average);
                var durationSeconds = (int)f.Properties.Duration.TotalSeconds;


                TimeSpan t = TimeSpan.FromSeconds(durationSeconds);
                var durationString = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                                t.Hours,
                                t.Minutes,
                                t.Seconds);

                var item = new ListViewAudioItem { 
                    FileFullPath = audioFile, 
                    FileName = System.IO.Path.GetFileName(audioFile), 
                    FileDurationInSeconds = durationSeconds,
                    FileDurationString = durationString
                };

                audioPlaylistListView.Items.Add(item);
                //preparedAudioList.
            }

            //audioPlaylistListView.Items.a .AddpreparedAudioList);

            var a = 10;
        }
    }
}
