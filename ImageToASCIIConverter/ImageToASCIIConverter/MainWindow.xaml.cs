using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ImageToASCIIConverter.Extensions;
using Microsoft.Win32;

namespace ImageToASCIIConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private BitmapImage _image;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ReadImageBtn_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                Filter =
                    "JPG Files (*.jpg)|*.jpg|bmp files (*.bmp)|*.bmp|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)" +
                    "|*.png|GIF Files (*.gif)|*.gif|All files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (fileDialog.ShowDialog() == true)
            {
                try
                {
                    _image = new BitmapImage(new Uri(fileDialog.FileName));
                    SourceImage.Source = _image;

                    LogTextBox.AppendLine("Image was successfully read.", Brushes.DarkGreen);
                }
                catch (NotSupportedException)
                {
                    LogTextBox.AppendLine("ERROR: Provided file is not an Image, please check your submission.", Brushes.Red);
                }
                catch (Exception ex)
                {
                    LogTextBox.AppendLine(ex.Message, Brushes.Red);
                }
            }
        }

        private void SaveTextFileBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_image == null)
            {
                LogTextBox.AppendLine("Open image first, please.", Color.FromRgb(0, 150, 150));
                return;
            }

            var convertedImageText = ImageConverter.ToAscii(_image, (int) ResolutionSlider.Value);

            var fileDialog = new SaveFileDialog
            {
                FileName = "TextImageASCIIFile",
                DefaultExt = ".txt",
                Filter = "Text Files |.txt",
                RestoreDirectory = true
            };

            if (fileDialog.ShowDialog() == true)
            {
                File.WriteAllText(fileDialog.FileName, convertedImageText, Encoding.ASCII);
                LogTextBox.AppendLine("Image converted and file was successfully created.", Brushes.DarkGreen);
            }
        }
    }
}
