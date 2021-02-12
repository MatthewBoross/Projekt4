using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace AudioPlayer
{
    /// <summary>
    /// Interaction logic for MiniPlayer.xaml
    /// </summary>
    public partial class MiniPlayer : Window
    {
        public readonly MediaPlayer mediaPlayer = new MediaPlayer();
        public readonly DispatcherTimer timer = new DispatcherTimer();
        public readonly Random r = new Random();
        public int positionSliderIsMoving = 0, isPlaying = 0, playing = -1, repeatType = 0, shuffle = 0, shuffleSelection = 0, shuffleFound = 0;

        class Error : Exception
        {
            public Error()
            {

            }
        }

        public MiniPlayer()
        {
            InitializeComponent();
            mediaPlayer.MediaEnded += new EventHandler(Media_Ended);
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            Info nw = new Info();
            nw.Show();
        }

        private void NormalPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.SongsListBox.Items.Clear();
            for (int i = 0; i < SongsListBox.Items.Count; i++)
            {
                mw.SongsListBox.Items.Add(SongsListBox.Items[i]);
            }
            mw.playing = playing;
            mw.SongsListBox.SelectedIndex = SongsListBox.SelectedIndex;
            mw.isPlaying = isPlaying;
            if (mw.isPlaying == 0)
            {
                mw.IsPlayingLabel.Content = "Stopped";
            }
            else if (mw.isPlaying == 1)
            {
                mw.IsPlayingLabel.Content = "Paused";
            }
            else if (mw.isPlaying == 2)
            {
                mw.IsPlayingLabel.Content = "Playing";
            }
            mw.timer.Interval = timer.Interval;
            mw.timer.Tick += mw.Timer_Tick;
            mw.timer.Start();
            mw.PositionSlider.Value = PositionSlider.Value;
            mw.PositionSlider.Maximum = PositionSlider.Maximum;
            mw.PositionSlider.IsEnabled = PositionSlider.IsEnabled;
            mw.PositionLabel.Content = PositionLabel.Content;
            mw.VolumeSlider.Value = VolumeSlider.Value;
            mw.VolumeLabel.Content = VolumeLabel.Content;
            mw.mediaPlayer.Volume = VolumeSlider.Value;
            mw.mediaPlayer.Balance = mediaPlayer.Balance;
            mw.BalanceSlider.Value = mediaPlayer.Balance;
            mw.PlayPauseButton.Background = PlayPauseButton.Background;
            mw.StopButton.Background = StopButton.Background;
            mw.RepeatButton.Background = RepeatButton.Background;
            mw.RepeatButton.Content = RepeatButton.Content;
            mw.repeatType = repeatType;
            if (mw.repeatType == 0)
            {
                mw.IsRepeatedLabel.Content = "No Repeat";
            }
            else if (mw.repeatType == 1)
            {
                mw.IsRepeatedLabel.Content = "Repeat All";
            }
            else if (mw.repeatType == 2)
            {
                mw.IsRepeatedLabel.Content = "Repeat Single Song";
            }
            mw.ShuffleButton.Background = ShuffleButton.Background;
            mw.shuffle = shuffle;
            if (mw.shuffle == 0)
            {
                mw.ShuffleLabel.Content = "No Shuffle";
            }
            else
            {
                mw.ShuffleLabel.Content = "Random Shuffle";
            }
            try
            {
                mw.mediaPlayer.Open(new Uri(SongsListBox.Items[playing].ToString()));
                mw.Show();
                if (mw.isPlaying == 2)
                {
                    mw.mediaPlayer.Play();
                }
                else if (mw.isPlaying == 1)
                {
                    mw.mediaPlayer.Pause();
                }
                mw.mediaPlayer.Position = mediaPlayer.Position;
            }
            catch
            {
                mw.Show();
            }
            mediaPlayer.Close();
            Close();
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
                    PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                }
                else
                {
                    mediaPlayer.Pause();
                    isPlaying = 1;
                    PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFC5C511");
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
            }
            else if (repeatType == 1)
            {
                repeatType = 2;
                RepeatButton.Content = "🔂";
            }
            else
            {
                repeatType = 0;
                RepeatButton.Content = "🔁";
                RepeatButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF9C9C9C");
            }
        }

        private void ShuffleButton_Click(object sender, RoutedEventArgs e)
        {
            if (shuffle == 0)
            {
                shuffle = 1;
                ShuffleButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF6BA1FF");
            }
            else
            {
                shuffle = 0;
                ShuffleButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF9C9C9C");
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
            string file = "The requested audio file (" + SongsListBox.SelectedItem + ") could not be opened. Please check if the file's location or name has been changed.";
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

        private void MiniPlayer_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                StreamWriter sw = new StreamWriter("./autoconfig_do_not_delete_or_modify.txt");
                sw.WriteLine("Audio Player version 1.0. Below is your automatic config. DO NOT DELETE OR MODIFY THIS FILE!");
                sw.WriteLine(mediaPlayer.Volume);
                sw.WriteLine(mediaPlayer.Balance);
                sw.WriteLine(repeatType);
                sw.WriteLine(shuffle);
                if (SongsListBox.Items.Count > 0)
                {
                    sw.WriteLine(SongsListBox.SelectedIndex);
                    for (int i = 0; i < SongsListBox.Items.Count - 1; i++)
                    {
                        sw.WriteLine(SongsListBox.Items[i].ToString());
                    }
                    sw.Write(SongsListBox.Items[SongsListBox.Items.Count - 1].ToString());
                }
                else
                {
                    sw.Write("-1");
                }
                sw.Close();
                sw.Dispose();
            }
            catch
            {
                MessageBoxResult mbr = MessageWindow.Show("Automatic config failed", "The automatic configuration write failed. This could mean the file is under use. Please close all applications that are using the file them try again. Would you like to close the window anyway?", MessageBoxButton.YesNo, MessageWindow.MessageBoxImage.Error);
                if (mbr == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
