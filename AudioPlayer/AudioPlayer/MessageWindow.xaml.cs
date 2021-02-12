using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace AudioPlayer
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        public enum MessageBoxType
        {
            ConfirmationWithYesNo = 0,
            ConfirmationWithYesNoCancel,
            Information,
            Error,
            Warning
        }

        public enum MessageBoxImage
        {
            Warning = 0,
            Question,
            Information,
            Error,
            None
        }

        public MessageWindow()
        {
            InitializeComponent();
        }

        static MessageWindow messageBox;
        static MessageBoxResult result = MessageBoxResult.No;

        public static MessageBoxResult Show(string caption, string text, MessageBoxButton button, MessageBoxImage image)
        {
            messageBox = new MessageWindow
            {
                Message = { Text = text },
                MessageTitle = { Text = caption }
            };
            SetVisibilityOfButtons(button);
            SetImageOfMessageBox(image);
            messageBox.ShowDialog();
            return result;
        }

        private static void SetVisibilityOfButtons(MessageBoxButton button)
        {
            switch (button)
            {
                case MessageBoxButton.OK:
                    {
                        messageBox.CancelButton.Visibility = Visibility.Collapsed;
                        messageBox.NoButton.Visibility = Visibility.Collapsed;
                        messageBox.YesButton.Visibility = Visibility.Collapsed;
                        messageBox.OKButton.Focus();
                        break;
                    }
                case MessageBoxButton.YesNo:
                    {
                        messageBox.CancelButton.Visibility = Visibility.Collapsed;
                        messageBox.OKButton.Visibility = Visibility.Collapsed;
                        messageBox.NoButton.Focus();
                        break;
                    }
                case MessageBoxButton.YesNoCancel:
                    {
                        messageBox.OKButton.Visibility = Visibility.Collapsed;
                        messageBox.CancelButton.Focus();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private static void SetImageOfMessageBox(MessageBoxImage image)
        {
            switch (image)
            {
                case MessageBoxImage.Warning:
                    {
                        messageBox.SetImage("Warning.png");
                        break;
                    }
                case MessageBoxImage.Information:
                    {
                        messageBox.SetImage("Information.png");
                        break;
                    }
                case MessageBoxImage.Error:
                    {
                        messageBox.SetImage("Error.png");
                        break;
                    }
                default:
                    {
                        messageBox.Image.Visibility = Visibility.Collapsed;
                        break;
                    }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == OKButton)
            {
                result = MessageBoxResult.OK;
            }
            else if (sender == YesButton)
            {
                result = MessageBoxResult.Yes;
            }
            else if (sender == NoButton)
            {
                result = MessageBoxResult.No;
            }
            else if (sender == CancelButton)
            {
                result = MessageBoxResult.Cancel;
            }
            else
            {
                result = MessageBoxResult.None;
            }
            messageBox.Close();
            messageBox = null;
        }

        private void SetImage(string imageName)
        {
            string uri = string.Format("./{0}", imageName);
            var uriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
            Image.Source = new BitmapImage(uriSource);
        }
    }
}
