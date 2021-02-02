using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace AudioPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly MediaPlayer mediaPlayer = new MediaPlayer();
        readonly DispatcherTimer timer = new DispatcherTimer();
        int positionSliderIsMoving = 0;
        int isPlaying = 0;
        int playing = -1;
        int repeatType = 0;
        int openError = 0;

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

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongsListBox.Items.Count != 0)
            {
                if (isPlaying == 0)
                {
                    if (SongsListBox.SelectedIndex != -1)
                    {
                        string file = SongsListBox.Items[SongsListBox.SelectedIndex].ToString();
                        mediaPlayer.Open(new Uri(file));
                        mediaPlayer.MediaOpened += (o, args) =>
                        {
                            openError = 0;
                        };
                        mediaPlayer.MediaFailed += (o, args) =>
                        {
                            MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                            openError = 1;
                        };
                        if (openError == 0)
                        {
                            mediaPlayer.Play();
                            isPlaying = 2;
                            playing = Convert.ToInt32(SongsListBox.SelectedIndex);
                            timer.Interval = TimeSpan.FromSeconds(1);
                            timer.Tick += Timer_Tick;
                            timer.Start();
                            PositionSlider.IsEnabled = true;
                            PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                            StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
                        }
                    }
                    else
                    {
                        try
                        {
                            SongsListBox.SelectedIndex = 0;
                            string file = SongsListBox.Items[0].ToString();
                            mediaPlayer.Open(new Uri(file));
                            mediaPlayer.MediaOpened += (o, args) =>
                            {
                                openError = 0;
                            };
                            mediaPlayer.MediaFailed += (o, args) =>
                            {
                                MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                                openError = 1;
                            };
                            if (openError == 0)
                            {
                                mediaPlayer.Play();
                                isPlaying = 2;
                                playing = 0;
                                timer.Interval = TimeSpan.FromSeconds(1);
                                timer.Tick += Timer_Tick;
                                timer.Start();
                                PositionSlider.IsEnabled = true;
                                PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                                StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
                            }
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
                }
                else
                {
                    mediaPlayer.Pause();
                    isPlaying = 1;
                    PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFE3F530");
                }
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            SongsListBox.SelectedIndex = -1;
            mediaPlayer.Close();
            timer.Stop();
            isPlaying = 0;
            playing = -1;
            openError = 0;
            PositionSlider.Value = 0;
            PositionSlider.Maximum = 1;
            PositionSlider.IsEnabled = false;
            PositionLabel.Content = "00:00/00:00";
            PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF12B900");
            StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF590000");
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SongsListBox.Items.Count != 0)
                {
                    if (SongsListBox.SelectedIndex == -1)
                    {
                        SongsListBox.SelectedIndex = 0;
                        string file = SongsListBox.Items[0].ToString();
                        mediaPlayer.Open(new Uri(file));
                        mediaPlayer.MediaOpened += (o, args) =>
                        {
                            openError = 0;
                        };
                        mediaPlayer.MediaFailed += (o, args) =>
                        {
                            MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                            openError = 1;
                        };
                        if (openError == 0)
                        {
                            mediaPlayer.Play();
                            isPlaying = 2;
                            playing = 0;
                            timer.Interval = TimeSpan.FromSeconds(1);
                            timer.Tick += Timer_Tick;
                            timer.Start();
                            PositionSlider.IsEnabled = true;
                            PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                            StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
                        }
                    }
                    else
                    {
                        SongsListBox.SelectedIndex = playing;
                        string file = SongsListBox.Items[playing].ToString();
                        mediaPlayer.Open(new Uri(file));
                        mediaPlayer.MediaOpened += (o, args) =>
                        {
                            openError = 0;
                        };
                        mediaPlayer.MediaFailed += (o, args) =>
                        {
                            MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                            openError = 1;
                        };
                        if (openError == 0)
                        {
                            mediaPlayer.Play();
                            isPlaying = 2;
                            timer.Interval = TimeSpan.FromSeconds(1);
                            timer.Tick += Timer_Tick;
                            timer.Start();
                            PositionSlider.IsEnabled = true;
                            PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                            StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
                        }
                    }
                }
            }
            catch
            {

            }
        }

        private void PrevButton_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SongsListBox.Items.Count != 0)
            {
                if (playing == 0 || playing == -1)
                {
                    try
                    {
                        SongsListBox.SelectedIndex = 0;
                        string file = SongsListBox.Items[0].ToString();
                        mediaPlayer.Open(new Uri(file));
                        mediaPlayer.MediaOpened += (o, args) =>
                        {
                            openError = 0;
                        };
                        mediaPlayer.MediaFailed += (o, args) =>
                        {
                            MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                            openError = 1;
                        };
                        if (openError == 0)
                        {
                            mediaPlayer.Play();
                            isPlaying = 2;
                            playing = 0;
                            timer.Interval = TimeSpan.FromSeconds(1);
                            timer.Tick += Timer_Tick;
                            timer.Start();
                            PositionSlider.IsEnabled = true;
                            PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                            StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
                        }
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
                        mediaPlayer.MediaOpened += (o, args) =>
                        {
                            openError = 0;
                        };
                        mediaPlayer.MediaFailed += (o, args) =>
                        {
                            MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                            openError = 1;
                        };
                        if (openError == 0)
                        {
                            mediaPlayer.Play();
                            isPlaying = 2;
                            timer.Interval = TimeSpan.FromSeconds(1);
                            timer.Tick += Timer_Tick;
                            timer.Start();
                            PositionSlider.IsEnabled = true;
                            PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                            StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongsListBox.Items.Count != 0)
            {
                if (SongsListBox.SelectedIndex != SongsListBox.Items.Count - 1)
                {
                    try
                    {
                        playing++;
                        SongsListBox.SelectedIndex = playing;
                        string file = SongsListBox.Items[SongsListBox.SelectedIndex].ToString();
                        mediaPlayer.Open(new Uri(file));
                        mediaPlayer.MediaOpened += (o, args) =>
                        {
                            openError = 0;
                        };
                        mediaPlayer.MediaFailed += (o, args) =>
                        {
                            MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                            openError = 1;
                        };
                        if (openError == 0)
                        {
                            mediaPlayer.Play();
                            isPlaying = 2;
                            timer.Interval = TimeSpan.FromSeconds(1);
                            timer.Tick += Timer_Tick;
                            timer.Start();
                            PositionSlider.IsEnabled = true;
                            PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                            StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
                        }
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
                        string file = SongsListBox.Items[0].ToString();
                        mediaPlayer.Open(new Uri(file));
                        mediaPlayer.MediaOpened += (o, args) =>
                        {
                            openError = 0;
                        };
                        mediaPlayer.MediaFailed += (o, args) =>
                        {
                            MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                            openError = 1;
                        };
                        if (openError == 0)
                        {
                            mediaPlayer.Play();
                            isPlaying = 2;
                            playing = 0;
                            timer.Interval = TimeSpan.FromSeconds(1);
                            timer.Tick += Timer_Tick;
                            timer.Start();
                            PositionSlider.IsEnabled = true;
                            PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                            StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
                        }
                    }
                    catch
                    {

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

        private void PositionSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            positionSliderIsMoving = 1;
        }

        private void PositionSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            positionSliderIsMoving = 0;
            mediaPlayer.Position = TimeSpan.FromSeconds(PositionSlider.Value);
        }

        private void PositionSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                PositionLabel.Content = String.Format("{0}/{1}", TimeSpan.FromSeconds(PositionSlider.Value).ToString(@"mm\:ss"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            }
            catch
            {

            }
        }

        private void VolumeSlider_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            mediaPlayer.Volume = VolumeSlider.Value;
            VolumeLabel.Content = Math.Round((VolumeSlider.Value * 100), 0) + "%";
        }

        private void SongsListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                string file = SongsListBox.Items[SongsListBox.SelectedIndex].ToString();
                mediaPlayer.Open(new Uri(file));
                mediaPlayer.MediaOpened += (o, args) =>
                {
                    openError = 0;
                };
                mediaPlayer.MediaFailed += (o, args) =>
                {
                    MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                    openError = 1;
                };
                if (openError == 0)
                {
                    mediaPlayer.Play();
                    isPlaying = 2;
                    playing = Convert.ToInt32(SongsListBox.SelectedIndex);
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += Timer_Tick;
                    timer.Start();
                    PositionSlider.IsEnabled = true;
                    PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                    StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
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
                    SongsListBox.SelectedIndex = -1;
                    mediaPlayer.Close();
                    timer.Stop();
                    isPlaying = 0;
                    playing = -1;
                    PositionSlider.Value = 0;
                    PositionSlider.Maximum = 1;
                    PositionSlider.IsEnabled = false;
                    PositionLabel.Content = "00:00/00:00";
                    PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF12B900");
                    StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF590000");
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
                        SongsListBox.SelectedIndex = -1;
                        mediaPlayer.Close();
                        timer.Stop();
                        isPlaying = 0;
                        playing = -1;
                        PositionSlider.Value = 0;
                        PositionSlider.Maximum = 1;
                        PositionLabel.Content = "00:00/00:00";
                        PositionSlider.IsEnabled = false;
                        PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF12B900");
                        StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF590000");
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
                catch (System.ArgumentException)
                {

                }
                catch (System.IO.IOException)
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
                StreamReader sr = new StreamReader(open.FileName);
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
                        SongsListBox.SelectedIndex = -1;
                        mediaPlayer.Close();
                        timer.Stop();
                        isPlaying = 0;
                        playing = -1;
                        PositionSlider.Value = 0;
                        PositionSlider.Maximum = 1;
                        PositionSlider.IsEnabled = false;
                        PositionLabel.Content = "00:00/00:00";
                        PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF12B900");
                        StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF590000");
                        do
                        {
                            SongsListBox.Items.Add(sr.ReadLine());
                        } while (!sr.EndOfStream);
                    }
                }
                else
                {
                    do
                    {
                        string path = sr.ReadLine();
                        SongsListBox.Items.Add(path);
                    } while (!sr.EndOfStream);
                }
            }
            catch (Error)
            {
                MessageBox.Show("An unexpected error occured. Please check that the loaded file was made with this application, then try again.", "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (System.ArgumentException)
            {

            }
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if ((mediaPlayer.Source != null) && mediaPlayer.NaturalDuration.HasTimeSpan && (positionSliderIsMoving == 0))
                {
                    PositionLabel.Content = String.Format("{0}/{1}", mediaPlayer.Position.ToString(@"mm\:ss"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
                    PositionSlider.Minimum = 0;
                    PositionSlider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds - 1;
                    PositionSlider.Value = mediaPlayer.Position.TotalSeconds;
                }
            }
            catch
            {

            }
        }

        private void Media_Ended(object sender, EventArgs e)
        {
            if (repeatType == 0 && openError == 0)
            {
                if (playing < (SongsListBox.Items.Count - 1))
                {
                    playing++;
                    SongsListBox.SelectedIndex = playing;
                    string file = SongsListBox.Items[SongsListBox.SelectedIndex].ToString();
                    mediaPlayer.Open(new Uri(file));
                    mediaPlayer.MediaOpened += (o, args) =>
                    {
                        openError = 0;
                    };
                    mediaPlayer.MediaFailed += (o, args) =>
                    {
                        MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                        openError = 1;
                    };
                    if (openError == 0)
                    {
                        mediaPlayer.Play();
                        isPlaying = 2;
                        timer.Interval = TimeSpan.FromSeconds(1);
                        timer.Tick += Timer_Tick;
                        timer.Start();
                        PositionSlider.IsEnabled = true;
                        PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                        StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
                    }
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
                    PositionSlider.IsEnabled = false;
                    PositionLabel.Content = "00:00/00:00";
                    PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF12B900");
                    StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF590000");
                }
            }
            else if (repeatType == 1 && openError == 0)
            {
                if (playing < (SongsListBox.Items.Count - 1))
                {
                    playing++;
                    SongsListBox.SelectedIndex = playing;
                    string file = SongsListBox.Items[SongsListBox.SelectedIndex].ToString();
                    mediaPlayer.Open(new Uri(file));
                    mediaPlayer.MediaOpened += (o, args) =>
                    {
                        openError = 0;
                    };
                    mediaPlayer.MediaFailed += (o, args) =>
                    {
                        MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                        openError = 1;
                    };
                    if (openError == 0)
                    {
                        mediaPlayer.Play();
                        isPlaying = 2;
                        timer.Interval = TimeSpan.FromSeconds(1);
                        timer.Tick += Timer_Tick;
                        timer.Start();
                        PositionSlider.IsEnabled = true;
                        PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                        StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
                    }
                }
                else
                {
                    playing = 0;
                    SongsListBox.SelectedIndex = playing;
                    string file = SongsListBox.Items[SongsListBox.SelectedIndex].ToString();
                    mediaPlayer.Open(new Uri(file));
                    mediaPlayer.MediaOpened += (o, args) =>
                    {
                        openError = 0;
                    };
                    mediaPlayer.MediaFailed += (o, args) =>
                    {
                        MessageBox.Show("The requested audio file could not be opened. Please check if the file's location has been changed or remove this audio file from the list, then readd it to the list.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                        openError = 1;
                    };
                    if (openError == 0)
                    {
                        mediaPlayer.Play();
                        isPlaying = 2;
                        timer.Interval = TimeSpan.FromSeconds(1);
                        timer.Tick += Timer_Tick;
                        timer.Start();
                        PositionSlider.IsEnabled = true;
                        PlayPauseButton.Background = (Brush)new BrushConverter().ConvertFrom("#FF82EE76");
                        StopButton.Background = (Brush)new BrushConverter().ConvertFrom("#FFB91414");
                    }
                }
            }
            else if (repeatType == 2 && openError == 0)
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
