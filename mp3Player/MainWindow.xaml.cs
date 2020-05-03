using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        private BackgroundWorker worker = null;
        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SetSelectedItemAsMusicPlayerSource()
        {
            if(audioPlaylistListView.SelectedItem == null)
            {
                return;
            }

            var audioItem = audioPlaylistListView.SelectedItem as ListViewAudioItem;
            MusicPlayerMediaElement.Source = new Uri(audioItem.FileFullPath);
        }

        private void Play()
        {
            SetSelectedItemAsMusicPlayerSource();
            MusicPlayerMediaElement.Play();
            isPaused = false;
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isPaused)
            {
                MusicPlayerMediaElement.Play();
                isPaused = false;
            } else 
            {
                Play();
            }
        }

        private void MusicPlayerMediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            int a = 10;
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            MusicPlayerMediaElement.Stop();
            worker.CancelAsync();
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

            worker.CancelAsync();
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
        }

        private void audioPlaylistListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Play();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MusicPlayerMediaElement.Volume = e.NewValue;
        }

        private void MusicPlayerMediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            audioSlider.Maximum = MusicPlayerMediaElement.NaturalDuration.TimeSpan.TotalSeconds;
            showProgressOnAudioSlider();

            //MusicPlayerMediaElement.NaturalDuration
            //MusicPlayerMediaElement.Position
            //var a = 10;
        }

        private void showProgressOnAudioSlider()
        {
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += ProgressChanged;
            worker.RunWorkerAsync();
            
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ///audioSlider.Value = MusicPlayerMediaElement.Position.TotalSeconds;

            while (true)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }

                Thread.Sleep(100);
                worker.ReportProgress(0);
            }
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            audioSlider.Value = MusicPlayerMediaElement.Position.TotalSeconds;
        }

        private void PreviousBtn_Click(object sender, RoutedEventArgs e)
        {
            if(audioPlaylistListView.Items.Count > 1 && audioPlaylistListView.SelectedIndex > 0)
            {
                audioPlaylistListView.SelectedItem = audioPlaylistListView.Items[audioPlaylistListView.SelectedIndex - 1];
            }

            Play();
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (audioPlaylistListView.Items.Count > 1 && audioPlaylistListView.SelectedIndex < audioPlaylistListView.Items.Count-1)
            {
                audioPlaylistListView.SelectedItem = audioPlaylistListView.Items[audioPlaylistListView.SelectedIndex + 1];
            }

            Play();
        }
    }
}
