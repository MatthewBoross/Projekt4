using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace AudioPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly MediaPlayer mediaPlayer = new MediaPlayer();
        public readonly DispatcherTimer timer = new DispatcherTimer();
        StreamReader sr;
        public readonly Random r = new Random();
        public int positionSliderIsMoving = 0, isPlaying = 0, playing = -1, repeatType = 0, shuffle = 0, shuffleSelection = 0, shuffleFound = 0;

        class Error : Exception
        {
            public Error()
            {

            }
        }

        public MainWindow()
        {
            InitializeComponent();
            mediaPlayer.MediaEnded += new EventHandler(Media_Ended);
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            Info nw = new Info();
            nw.Show();
        }

        private void MiniPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            MiniPlayer mp = new MiniPlayer();
            mp.Show();
            for (int i = 0; i < SongsListBox.Items.Count; i++)
            {
                mp.SongsListBox.Items.Add(SongsListBox.Items[i]);
            }
            mp.playing = playing;
            mp.SongsListBox.SelectedIndex = SongsListBox.SelectedIndex;
            mp.isPlaying = isPlaying;
            mp.timer.Interval = timer.Interval;
            mp.timer.Tick += mp.Timer_Tick;
            mp.timer.Start();
            mp.PositionSlider.Value = PositionSlider.Value;
            mp.PositionSlider.Maximum = PositionSlider.Maximum;
            mp.PositionSlider.IsEnabled = PositionSlider.IsEnabled;
            mp.PositionLabel.Content = PositionLabel.Content;
            mp.VolumeSlider.Value = VolumeSlider.Value;
            mp.VolumeLabel.Content = VolumeLabel.Content;
            mp.mediaPlayer.Volume = VolumeSlider.Value;
            mp.mediaPlayer.Balance = mediaPlayer.Balance;
            mp.PlayPauseButton.Background = PlayPauseButton.Background;
            mp.StopButton.Background = StopButton.Background;
            mp.RepeatButton.Background = RepeatButton.Background;
            mp.RepeatButton.Content = RepeatButton.Content;
            mp.repeatType = repeatType;
            mp.ShuffleButton.Background = ShuffleButton.Background;
            mp.shuffle = shuffle;
            try
            {
                mp.mediaPlayer.Open(new Uri(SongsListBox.Items[playing].ToString()));
                if (mp.isPlaying == 2)
                {
                    mp.mediaPlayer.Play();
                }
                else if (mp.isPlaying == 1)
                {
                    mp.mediaPlayer.Pause();
                }
                mp.mediaPlayer.Position = mediaPlayer.Position;
            }
            catch
            {

            }
            mediaPlayer.Close();
            Close();
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongsListBox.Items.Count != 0)
            {
                if (isPlaying == 0)
                {
                    if (SongsListBox.SelectedIndex != -1)
                    {
                        Selected_Index();
                        Start();
                    }
                    else
                    {
                        First_Index();
                        Start();
                    }
                }
                else if (isPlaying == 1)
                {
                    mediaPlayer.Play();
                    isPlaying = 2;
                    IsPlayingLabel.Content = "Playing";
                    PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                }
                else
                {
                    mediaPlayer.Pause();
                    isPlaying = 1;
                    IsPlayingLabel.Content = "Paused";
                    PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFC5C511");
                }
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongsListBox.Items.Count != 0)
            {
                if (mediaPlayer.Position.TotalSeconds > 3)
                {
                    Start_Again();
                }
                else
                {
                    if (isPlaying != 0)
                    {
                        if (playing == 0)
                        {
                            Start_Again();
                        }
                        else
                        {
                            playing--;
                            SongsListBox.SelectedIndex = playing;
                            mediaPlayer.Open(new Uri(SongsListBox.Items[SongsListBox.SelectedIndex].ToString()));
                            Start();
                        }
                    }
                    else
                    {
                        if (SongsListBox.SelectedIndex == -1 || SongsListBox.SelectedIndex == 0)
                        {
                            First_Index();
                            Start();
                        }
                        else
                        {
                            SongsListBox.SelectedIndex--;
                            playing = SongsListBox.SelectedIndex;
                            mediaPlayer.Open(new Uri(SongsListBox.Items[SongsListBox.SelectedIndex].ToString()));
                            Start();
                        }
                    }
                }
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongsListBox.Items.Count != 0)
            {
                if (isPlaying != 0)
                {
                    if (playing != SongsListBox.Items.Count - 1)
                    {
                        Next_Song();
                        Start();
                    }
                    else
                    {
                        First_Index();
                        Start();
                    }
                }
                else
                {
                    if (SongsListBox.SelectedIndex != SongsListBox.Items.Count - 1)
                    {
                        SongsListBox.SelectedIndex++;
                        playing = SongsListBox.SelectedIndex;
                        mediaPlayer.Open(new Uri(SongsListBox.Items[SongsListBox.SelectedIndex].ToString()));
                        Start();
                    }
                    else
                    {
                        First_Index();
                        Start();
                    }
                }
            }
        }

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (repeatType == 0)
            {
                repeatType = 1;
                RepeatButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF6BA1FF");
                IsRepeatedLabel.Content = "Repeat All";
            }
            else if (repeatType == 1)
            {
                repeatType = 2;
                RepeatButton.Content = "🔂";
                IsRepeatedLabel.Content = "Repeat Single Song";
            }
            else
            {
                repeatType = 0;
                RepeatButton.Content = "🔁";
                RepeatButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF9C9C9C");
                IsRepeatedLabel.Content = "No Repeat";
            }
        }

        private void ShuffleButton_Click(object sender, RoutedEventArgs e)
        {
            if (shuffle == 0)
            {
                shuffle = 1;
                ShuffleButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF6BA1FF");
                ShuffleLabel.Content = "Random Shuffle";
            }
            else
            {
                shuffle = 0;
                ShuffleButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF9C9C9C");
                ShuffleLabel.Content = "No Shuffle";
            }
        }

        private void PositionSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            positionSliderIsMoving = 1;
        }

        private void PositionSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            positionSliderIsMoving = 0;
            mediaPlayer.Position = TimeSpan.FromSeconds(PositionSlider.Value);
            if (PositionSlider.Value == PositionSlider.Maximum)
            {
                MediaEnd();
            }
        }

        private void PositionSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                PositionLabel.Content = string.Format("{0}/{1}", TimeSpan.FromSeconds(PositionSlider.Value).ToString(@"mm\:ss"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            }
            catch
            {

            }
        }

        private void VolumeSlider_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            mediaPlayer.Volume = VolumeSlider.Value;
            VolumeLabel.Content = Math.Round(VolumeSlider.Value * 100) + "%";
        }

        private void BalanceSlider_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            mediaPlayer.Balance = BalanceSlider.Value;
        }

        private void BalanceResetButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Balance = 0;
            BalanceSlider.Value = 0;
        }

        private void SongsListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                DependencyObject obj = (DependencyObject)e.OriginalSource;

                while (obj != null && obj != SongsListBox)
                {
                    if (obj.GetType() == typeof(ListBoxItem))
                    {
                        Selected_Index();
                        Start();
                        break;
                    }
                    obj = VisualTreeHelper.GetParent(obj);
                }
            }
            catch
            {

            }
        }

        private void AddSongsButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "MP3 files (*.mp3)|*.mp3",
                Multiselect = true
            };
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    if (!SongsListBox.Items.Contains(file))
                    {
                        SongsListBox.Items.Add(file);
                    }
                }
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongsListBox.Items.Count != 0)
            {
                MessageBoxResult mbr = MessageBox.Show("This will clear your current playlist. Are you sure? If you would like to first save your playlist, click \"No\", then click \"Save Playlist...\".", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (mbr == MessageBoxResult.Yes)
                {
                    SongsListBox.Items.Clear();
                    Stop();
                }
            }
            else
            {
                MessageBox.Show("Your playlist is empty.", "Empty playlist", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RemoveSongButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongsListBox.Items.Count == 0)
            {
                MessageBox.Show("There are no items to be removed.", "Empty playlist", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (SongsListBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a song from the playlist first.", "Select a song first", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    if (SongsListBox.SelectedIndex == playing)
                    {
                        SongsListBox.Items.Remove(SongsListBox.SelectedItem);
                        Stop();
                    }
                    else
                    {
                        if (SongsListBox.SelectedIndex > playing)
                        {
                            SongsListBox.Items.Remove(SongsListBox.SelectedItem);
                            SongsListBox.SelectedIndex = playing;
                        }
                        else
                        {
                            playing--;
                            SongsListBox.Items.Remove(SongsListBox.SelectedItem);
                            SongsListBox.SelectedIndex = playing;
                        }
                    }
                }
            }
        }

        private void SavePlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongsListBox.Items.Count == 0)
            {
                MessageBox.Show("You cannot save an empty playlist.", "Empty Playlist", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                try
                {
                    SaveFileDialog sf = new SaveFileDialog
                    {
                        Filter = "Text files (*.txt)|*.txt"
                    };
                    sf.ShowDialog();
                    StreamWriter sw = new StreamWriter(sf.FileName);
                    sw.WriteLine("Audio Player version 1.0. Below is your playlist.");
                    for (int i = 0; i < SongsListBox.Items.Count; i++)
                    {
                        sw.WriteLine(SongsListBox.Items[i].ToString());
                    }
                    sw.Close();
                    MessageBox.Show("Your playlist has been successfully saved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {

                }
            }
        }

        private void LoadPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog
                {
                    Filter = "Text files (*.txt)|*.txt"
                };
                open.ShowDialog();
                sr = new StreamReader(open.FileName);
                string ok = sr.ReadLine();
                if (ok != "Audio Player version 1.0. Below is your playlist.")
                {
                    throw new Error();
                }
                if (SongsListBox.Items.Count != 0)
                {
                    MessageBoxResult mbr = MessageBox.Show("Opening a saved playlist will clear your current playlist. Are you sure you want to open your saved playlist? If you would like to first save your playlist, click \"No\", then click \"Save Playlist...\".", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    if (mbr == MessageBoxResult.Yes)
                    {
                        SongsListBox.Items.Clear();
                        Stop();
                        Load();
                    }
                }
                else
                {
                    Load();
                }
            }
            catch (Error)
            {
                MessageBox.Show("An unexpected error occured. Please check that the loaded file was made with this application, then try again.", "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {

            }
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if ((mediaPlayer.Source != null) && mediaPlayer.NaturalDuration.HasTimeSpan && (positionSliderIsMoving == 0))
                {
                    PositionLabel.Content = string.Format("{0}/{1}", mediaPlayer.Position.ToString(@"mm\:ss"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
                    PositionSlider.Minimum = 0;
                    PositionSlider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                    PositionSlider.Value = mediaPlayer.Position.TotalSeconds;
                }
            }
            catch
            {

            }
        }

        private void Media_Ended(object sender, EventArgs e)
        {
            MediaEnd();
        }

        private void Media_Failed(object sender, EventArgs e)
        {
            MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
            Stop();
        }

        private void MediaEnd()
        {
            try
            {
                if (positionSliderIsMoving == 0)
                {
                    if (repeatType != 2)
                    {
                        if (shuffle == 0)
                        {
                            if (repeatType == 0)
                            {
                                if (playing < (SongsListBox.Items.Count - 1))
                                {
                                    Next_Song();
                                    Start();
                                }
                                else
                                {
                                    Stop();
                                }
                            }
                            else if (repeatType == 1)
                            {
                                if (playing < (SongsListBox.Items.Count - 1))
                                {
                                    Next_Song();
                                    Start();
                                }
                                else
                                {
                                    First_Index();
                                    Start();
                                }
                            }
                        }
                        else
                        {
                            if (SongsListBox.Items.Count == 1)
                            {
                                Start_Again();
                            }
                            else
                            {
                                do
                                {
                                    shuffleSelection = r.Next(0, SongsListBox.Items.Count);
                                    if (shuffleSelection != playing)
                                    {
                                        shuffleFound = 1;
                                        playing = shuffleSelection;
                                        SongsListBox.SelectedIndex = playing;
                                        mediaPlayer.Open(new Uri(SongsListBox.Items[SongsListBox.SelectedIndex].ToString()));
                                        Start();
                                    }
                                    else
                                    {
                                        shuffleFound = 0;
                                    }
                                } while (shuffleFound == 0);
                            }
                        }
                    }
                    else
                    {
                        Start_Again();
                    }
                }
            }
            catch
            {

            }
        }

        private void Selected_Index()
        {
            playing = SongsListBox.SelectedIndex;
            mediaPlayer.Open(new Uri(SongsListBox.Items[SongsListBox.SelectedIndex].ToString()));
        }

        private void First_Index()
        {
            playing = 0;
            SongsListBox.SelectedIndex = 0;
            mediaPlayer.Open(new Uri(SongsListBox.Items[0].ToString()));
        }

        private void Start()
        {
            mediaPlayer.Play();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            isPlaying = 2;
            PositionSlider.IsEnabled = true;
            IsPlayingLabel.Content = "Playing";
            PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
            StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
            mediaPlayer.Volume = VolumeSlider.Value;
            mediaPlayer.MediaFailed += Media_Failed;
        }

        private void Stop()
        {
            playing = -1;
            SongsListBox.SelectedIndex = -1;
            mediaPlayer.Close();
            timer.Stop();
            isPlaying = 0;
            PositionSlider.Value = 0;
            PositionSlider.Maximum = 1;
            PositionSlider.IsEnabled = false;
            PositionLabel.Content = "00:00/00:00";
            IsPlayingLabel.Content = "Stopped";
            PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF12B900");
            StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF590000");
        }

        private void Start_Again()
        {
            mediaPlayer.Position = TimeSpan.FromSeconds(0);
            if (isPlaying != 1)
            {
                PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
            }
        }

        private void Next_Song()
        {
            playing++;
            SongsListBox.SelectedIndex = playing;
            mediaPlayer.Open(new Uri(SongsListBox.Items[SongsListBox.SelectedIndex].ToString()));
        }

        private void Load()
        {
            do
            {
                SongsListBox.Items.Add(sr.ReadLine());
            } while (!sr.EndOfStream);
        }
    }
}
