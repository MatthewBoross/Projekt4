using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32;


namespace AudioPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MediaPlayer mediaPlayer = new MediaPlayer();
        DispatcherTimer timer = new DispatcherTimer();
        int positionSliderIsMoving = 0;
        int isPlaying = 0;
        int playing = -1;
        int repeatType = 0;

        public MainWindow()
        {
            InitializeComponent();
            mediaPlayer.MediaEnded += new EventHandler(Media_Ended);
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (isPlaying == 0)
            {
                if (SongsListBox.SelectedIndex != -1)
                {
                    string file = SongsListBox.Items[SongsListBox.SelectedIndex].ToString();
                    mediaPlayer.Open(new Uri(file));
                    mediaPlayer.MediaFailed += (o, args) =>
                    {
                         MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                    };
                    mediaPlayer.Play();
                    isPlaying = 2;
                    playing = Convert.ToInt32(SongsListBox.SelectedIndex);
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += Timer_Tick;
                    timer.Start();
                    PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                    StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
                }
                else
                {
                    try
                    {
                        SongsListBox.SelectedIndex = 0;
                        string file = SongsListBox.Items[0].ToString();
                        mediaPlayer.Open(new Uri(file));
                        mediaPlayer.MediaFailed += (o, args) =>
                        {
                            MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                        };
                        mediaPlayer.Play();
                        isPlaying = 2;
                        playing = 0;
                        timer.Interval = TimeSpan.FromSeconds(1);
                        timer.Tick += Timer_Tick;
                        timer.Start();
                        PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                        StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
                    }
                    catch
                    {

                    }
                }
            }
            else if (isPlaying == 1)
            {
                mediaPlayer.Play();
                isPlaying = 2;
                PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
            }
            else
            {
                mediaPlayer.Pause();
                isPlaying = 1;
                PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFE3F530");
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            SongsListBox.SelectedIndex = -1;
            mediaPlayer.Close();
            timer.Stop();
            isPlaying = 0;
            playing = -1;
            PositionSlider.Value = 0;
            PositionSlider.Maximum = 1;
            PositionLabel.Content = "00:00/00:00";
            PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF12B900");
            StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF590000");
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SongsListBox.SelectedIndex == -1)
                {
                    SongsListBox.SelectedIndex = 0;
                    string file = SongsListBox.Items[0].ToString();
                    mediaPlayer.Open(new Uri(file));
                    mediaPlayer.MediaFailed += (o, args) =>
                    {
                        MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                    };
                    mediaPlayer.Play();
                    isPlaying = 2;
                    playing = 0;
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += Timer_Tick;
                    timer.Start();
                    PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                    StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
                }
                else
                {
                    SongsListBox.SelectedIndex = playing;
                    string file = SongsListBox.Items[playing].ToString();
                    mediaPlayer.Open(new Uri(file));
                    mediaPlayer.MediaFailed += (o, args) =>
                    {
                        MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                    };
                    mediaPlayer.Play();
                    isPlaying = 2;
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += Timer_Tick;
                    timer.Start();
                    PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                    StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
                }
            }
            catch
            {

            }
        }

        private void PrevButton_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (playing == 0 || playing == -1)
            {
                try
                {
                    SongsListBox.SelectedIndex = 0;
                    string file = SongsListBox.Items[0].ToString();
                    mediaPlayer.Open(new Uri(file));
                    mediaPlayer.MediaFailed += (o, args) =>
                    {
                        MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                    };
                    mediaPlayer.Play();
                    isPlaying = 2;
                    playing = 0;
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += Timer_Tick;
                    timer.Start();
                    PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                    StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
                }
                catch
                {

                }
            }
            else
            {
                try
                {
                    playing--;
                    SongsListBox.SelectedIndex = playing;
                    string file = SongsListBox.Items[SongsListBox.SelectedIndex].ToString();
                    mediaPlayer.Open(new Uri(file));
                    mediaPlayer.MediaFailed += (o, args) =>
                    {
                        MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                    };
                    mediaPlayer.Play();
                    isPlaying = 2;
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += Timer_Tick;
                    timer.Start();
                    PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                    StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
                }
                catch
                {

                }
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            string file = "";
            if (SongsListBox.SelectedIndex != SongsListBox.Items.Count - 1)
            {
                try
                {
                    playing++;
                    SongsListBox.SelectedIndex = playing;
                    file = SongsListBox.Items[SongsListBox.SelectedIndex].ToString();
                    mediaPlayer.Open(new Uri(file));
                    mediaPlayer.MediaFailed += (o, args) =>
                    {
                        MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                    };
                    mediaPlayer.Play();
                    isPlaying = 2;
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += Timer_Tick;
                    timer.Start();
                    PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                    StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
                }
                catch
                {
                    
                }
            }
            else
            {
                try
                {
                    SongsListBox.SelectedIndex = 0;
                    file = SongsListBox.Items[0].ToString();
                    mediaPlayer.Open(new Uri(file));
                    mediaPlayer.MediaFailed += (o, args) =>
                    {
                        MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                    };
                    mediaPlayer.Play();
                    isPlaying = 2;
                    playing = 0;
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += Timer_Tick;
                    timer.Start();
                    PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                    StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
                }
                catch
                {

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

        private void PositionSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            positionSliderIsMoving = 1;
        }

        private void PositionSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            positionSliderIsMoving = 0;
            mediaPlayer.Position = TimeSpan.FromSeconds(PositionSlider.Value);
        }

        private void VolumeSlider_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            mediaPlayer.Volume = VolumeSlider.Value;
        }

        private void SongsListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                string file = SongsListBox.Items[SongsListBox.SelectedIndex].ToString();
                mediaPlayer.Open(new Uri(file));
                mediaPlayer.MediaFailed += (o, args) =>
                {
                    MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                };
                mediaPlayer.Play();
                isPlaying = 2;
                playing = Convert.ToInt32(SongsListBox.SelectedIndex);
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;
                timer.Start();
                PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
            }
            catch
            {

            }
        }

        private void AddSongsButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (String file in openFileDialog.FileNames)
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
            if (Convert.ToInt32(SongsListBox.SelectedIndex) == playing)
            {
                SongsListBox.Items.Remove(SongsListBox.SelectedItem);
                mediaPlayer.Close();
                timer.Stop();
                isPlaying = 0;
                playing = -1;
                PositionSlider.Value = 0;
                PositionSlider.Maximum = 1;
                PositionLabel.Content = "00:00/00:00";
                SongsListBox.SelectedIndex = -1;
                PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF12B900");
                StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF590000");
            }
            else
            {
                SongsListBox.Items.Remove(SongsListBox.SelectedItem);
                SongsListBox.SelectedIndex = playing;
            }
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if ((mediaPlayer.Source != null) && (mediaPlayer.NaturalDuration.HasTimeSpan) && (positionSliderIsMoving == 0))
                {
                    PositionLabel.Content = String.Format("{0}/{1}", mediaPlayer.Position.ToString(@"mm\:ss"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
                    PositionSlider.Minimum = 0;
                    PositionSlider.Maximum = (mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds - 1);
                    PositionSlider.Value = mediaPlayer.Position.TotalSeconds;
                }
                else if ((mediaPlayer.Source != null) && (mediaPlayer.NaturalDuration.HasTimeSpan) && positionSliderIsMoving == 1)
                {
                    int min, sec;
                    min = Convert.ToInt32(PositionSlider.Value) / 60;
                    sec = Convert.ToInt32(PositionSlider.Value) - (min * 60);
                    string minS = (min > 9) ? (min.ToString()) : ("0" + min.ToString());
                    string secS = (sec > 9) ? (sec.ToString()) : ("0" + sec.ToString());
                    PositionLabel.Content = String.Format("{0}:{1}/{2}", minS, secS, mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
                }
            }
            catch
            {

            }
        }

        private void Media_Ended(object sender, EventArgs e)
        {
            if (repeatType == 0)
            {
                if (playing < (SongsListBox.Items.Count - 1))
                {
                    playing++;
                    SongsListBox.SelectedIndex = playing;
                    string file = SongsListBox.Items[SongsListBox.SelectedIndex].ToString();
                    mediaPlayer.Open(new Uri(file));
                    mediaPlayer.MediaFailed += (o, args) =>
                    {
                        MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                    };
                    mediaPlayer.Play();
                    isPlaying = 2;
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += Timer_Tick;
                    timer.Start();
                    PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                    StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
                }
                else
                {
                    SongsListBox.SelectedIndex = -1;
                    mediaPlayer.Close();
                    timer.Stop();
                    isPlaying = 0;
                    playing = -1;
                    PositionSlider.Value = 0;
                    PositionSlider.Maximum = 1;
                    PositionLabel.Content = "00:00/00:00";
                    PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF12B900");
                    StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF590000");
                }
            }
            else if (repeatType == 1)
            {
                if (playing < (SongsListBox.Items.Count - 1))
                {
                    playing++;
                    SongsListBox.SelectedIndex = playing;
                    string file = SongsListBox.Items[SongsListBox.SelectedIndex].ToString();
                    mediaPlayer.Open(new Uri(file));
                    mediaPlayer.MediaFailed += (o, args) =>
                    {
                        MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                    };
                    mediaPlayer.Play();
                    isPlaying = 2;
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += Timer_Tick;
                    timer.Start();
                    PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                    StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
                }
                else
                {
                    playing = 0;
                    SongsListBox.SelectedIndex = playing;
                    string file = SongsListBox.Items[SongsListBox.SelectedIndex].ToString();
                    mediaPlayer.Open(new Uri(file));
                    mediaPlayer.MediaFailed += (o, args) =>
                    {
                        MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                    };
                    mediaPlayer.Play();
                    isPlaying = 2;
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += Timer_Tick;
                    timer.Start();
                    PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                    StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
                }
            }
            else
            {
                mediaPlayer.Position = TimeSpan.FromSeconds(0);
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;
                timer.Start();
                PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
            }
        }


    }
}
