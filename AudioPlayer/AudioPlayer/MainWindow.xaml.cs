using Microsoft.Win32;
using System;
using System.ComponentModel;
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
        public int positionSliderIsMoving = 0, isPlaying = 0, playing = -1, repeatType = 0, shuffle = 0, shuffleSelection = 0, shuffleFound = 0, saveSuccess = 0;

        class Error : Exception
        {
            public Error()
            {

            }
        }

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                sr = new StreamReader("./autoconfig_do_not_delete.txt");
                string ok = sr.ReadLine();
                if (ok != "Audio Player version 1.0. Below is your automatic config. DO NOT DELETE THIS FILE!")
                {
                    throw new Error();
                }
                VolumeSlider.Value = Convert.ToDouble(sr.ReadLine());
                VolumeLabel.Content = Math.Round(VolumeSlider.Value * 100) + "%";
                mediaPlayer.Volume = VolumeSlider.Value;
                BalanceSlider.Value = Convert.ToDouble(sr.ReadLine());
                mediaPlayer.Balance = BalanceSlider.Value;
                while (!sr.EndOfStream)
                {
                    SongsListBox.Items.Add(sr.ReadLine());
                }
                sr.Close();
                sr.Dispose();
            }
            catch (Error)
            {

            }
            catch
            {

            }
            mediaPlayer.MediaEnded += new EventHandler(Media_Ended);
            Closing += new CancelEventHandler(MainWindow_Closing);
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            Info nw = new Info();
            nw.Show();
        }

        private void MiniPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongsListBox.Items.Count != 0)
            {
                MiniPlayer mp = new MiniPlayer();
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
                    mp.Show();
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
                    mp.Show();
                }
                mediaPlayer.Close();
                Close();
            }
            else
            {
                MessageWindow.Show("Empty playlist", "Please add some songs to your playlist first.", MessageBoxButton.OK, MessageWindow.MessageBoxImage.Information);
            }
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongsListBox.Items.Count != 0)
            {
                if (isPlaying == 0)
                {
                    if (shuffle == 0)
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
                    else
                    {
                        if (SongsListBox.SelectedIndex == -1)
                        {
                            Random_Song();
                        }
                        else
                        {
                            Selected_Index();
                            Start();
                        }
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
                    if (shuffle == 0)
                    {
                        if (isPlaying != 0)
                        {
                            if (playing == 0)
                            {
                                playing = SongsListBox.Items.Count - 1;
                                SongsListBox.SelectedIndex = SongsListBox.Items.Count - 1;
                                mediaPlayer.Open(new Uri(SongsListBox.Items[SongsListBox.Items.Count - 1].ToString()));
                                Start();
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
                            if (SongsListBox.SelectedIndex == -1)
                            {
                                First_Index();
                                Start();
                            }
                            else if (SongsListBox.SelectedIndex == 0)
                            {
                                SongsListBox.SelectedIndex = SongsListBox.Items.Count - 1;
                                playing = SongsListBox.Items.Count - 1;
                                mediaPlayer.Open(new Uri(SongsListBox.Items[SongsListBox.Items.Count - 1].ToString()));
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
                    else
                    {
                        if (SongsListBox.Items.Count == 1)
                        {
                            First_Index();
                            Start();
                        }
                        else
                        {
                            Random_Song();
                        }
                    }
                }
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongsListBox.Items.Count != 0)
            {
                if (shuffle == 0)
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
                else
                {
                    if (SongsListBox.Items.Count == 1)
                    {
                        First_Index();
                        Start();
                    }
                    else
                    {
                        Random_Song();
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

        private void RemoveSongButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongsListBox.Items.Count == 0)
            {
                MessageWindow.Show("Empty playlist", "There are no items to be removed.", MessageBoxButton.OK, MessageWindow.MessageBoxImage.Information);
            }
            else
            {
                if (SongsListBox.SelectedIndex == -1)
                {
                    MessageWindow.Show("Select a song first", "Please select a song from the playlist first.", MessageBoxButton.OK, MessageWindow.MessageBoxImage.Information);
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

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongsListBox.Items.Count != 0)
            {
                MessageBoxResult mbr = MessageWindow.Show("Are you sure?", "This will clear your current playlist. Would you like to save your playlist first?", MessageBoxButton.YesNoCancel, MessageWindow.MessageBoxImage.Warning);
                if (mbr == MessageBoxResult.Yes)
                {
                    Save();
                    if (saveSuccess == 1)
                    {
                        SongsListBox.Items.Clear();
                        Stop();
                    }
                    else
                    {
                        MessageWindow.Show("Save failed", "The save has failed. This could mean the save was aborted or the destination file is currently in use. To solve the later problem, you can try creating a new playlist instead of overwriting an existing one. (Your playlist was not cleared.)", MessageBoxButton.OK, MessageWindow.MessageBoxImage.Information);
                    }
                }
                else if (mbr == MessageBoxResult.No)
                {
                    SongsListBox.Items.Clear();
                    Stop();
                }
            }
            else
            {
                MessageWindow.Show("Empty playlist", "Your playlist is already empty.", MessageBoxButton.OK, MessageWindow.MessageBoxImage.Information);
            }
        }

        private void SavePlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongsListBox.Items.Count == 0)
            {
                MessageWindow.Show("Empty Playlist", "You cannot save an empty playlist.", MessageBoxButton.OK, MessageWindow.MessageBoxImage.Information);
            }
            else
            {
                Save();
                if (saveSuccess == 0)
                {
                    MessageWindow.Show("Save failed", "The save has failed. This could mean the save was aborted or the destination file is currently in use. To solve the later problem, you can try creating a new playlist instead of overwriting an existing one.", MessageBoxButton.OK, MessageWindow.MessageBoxImage.Information);
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
                    MessageBoxResult mbr = MessageWindow.Show("Are you sure?", "Opening a saved playlist will clear your current playlist. Would you like to save your playlist first?", MessageBoxButton.YesNoCancel, MessageWindow.MessageBoxImage.Warning);
                    if (mbr == MessageBoxResult.Yes)
                    {
                        Save();
                        if (saveSuccess == 1)
                        {
                            SongsListBox.Items.Clear();
                            Stop();
                            Load();
                        }
                        else
                        {
                            MessageWindow.Show("Save failed", "The save has failed. This could mean the save was aborted or the destination file is currently in use. To solve the later problem, you can try creating a new playlist instead of overwriting an existing one. (Your playlist was not loaded.)", MessageBoxButton.OK, MessageWindow.MessageBoxImage.Information);
                        }
                    }
                    else if (mbr == MessageBoxResult.No)
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
                MessageWindow.Show("Fatal error", "An unexpected error occured. Please check that the loaded file was made with this application, then try again.", MessageBoxButton.OK, MessageWindow.MessageBoxImage.Error);
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
            string file = "The requested audio file (" + SongsListBox.SelectedItem + ") could not be opened. Please check if the file's location or name has been changed or remove this audio file from the list, then readd it to the list.";
            MessageWindow.Show("An error occured", file, MessageBoxButton.OK, MessageWindow.MessageBoxImage.Error);
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
                            Random_Song();
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
            BalanceSlider.Value = 0;
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

        private void Random_Song()
        {
            if (SongsListBox.Items.Count == 1)
            {
                if (SongsListBox.SelectedIndex == -1)
                {
                    First_Index();
                    Start();
                }
                else
                {
                    Start_Again();
                    Start();
                }
            }
            else
            {
                do
                {
                    shuffleSelection = r.Next(0, SongsListBox.Items.Count);
                    if (isPlaying == 0)
                    {
                        if (shuffleSelection != SongsListBox.SelectedIndex)
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
                    }
                    else
                    {
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
                    }
                } while (shuffleFound == 0);
            }
        }

        public void Save()
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
                sw.Dispose();
                MessageWindow.Show("Success", "Your playlist has been successfully saved.", MessageBoxButton.OK, MessageWindow.MessageBoxImage.Information);
                saveSuccess = 1;
            }
            catch
            {
                saveSuccess = 0;
            }
        }

        private void Load()
        {
            do
            {
                SongsListBox.Items.Add(sr.ReadLine());
            } while (!sr.EndOfStream);
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            StreamWriter sw = new StreamWriter("./autoconfig_do_not_delete.txt");
            sw.WriteLine("Audio Player version 1.0. Below is your automatic config. DO NOT DELETE THIS FILE!");
            sw.WriteLine(VolumeSlider.Value);
            if (SongsListBox.Items.Count > 1)
            {
                sw.WriteLine(BalanceSlider.Value);
                for (int i = 0; i < SongsListBox.Items.Count - 1; i++)
                {
                    sw.WriteLine(SongsListBox.Items[i].ToString());
                }
                sw.Write(SongsListBox.Items[SongsListBox.Items.Count - 1].ToString());
            }
            else if (SongsListBox.Items.Count == 1)
            {
                sw.WriteLine(BalanceSlider.Value);
                sw.Write(SongsListBox.Items[0].ToString());
            }
            else
            {
                sw.Write(BalanceSlider.Value);
            }
            sw.Close();
            sw.Dispose();
        }
    }
}
