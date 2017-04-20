using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace ImageToASCIIConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private string _textFile;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConvertImageBtn_Click(object sender, RoutedEventArgs e)
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
                var image = new BitmapImage(new Uri(fileDialog.FileName));
                var converter = new Converters(image);
                _textFile = converter.GetTextFile();
                SourceImage.Source = image;

                LogTextBox.Text += "Text file successfully created" + "\n";
            }
            else
            {
                LogTextBox.Text += "Error opening image" + "\n";
            }
        }

        private void SaveTextFileBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_textFile != null)
            {
                var fileDialog = new SaveFileDialog
                {
                    FileName = "TextImageASCIIFile",
                    DefaultExt = ".txt",
                    Filter = "Text Files |.txt",
                    RestoreDirectory = true
                };

                if (fileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(fileDialog.FileName, _textFile, Encoding.ASCII);
                    LogTextBox.Text += "Text file successfully saved" + "\n";
                }
                else
                {
                    LogTextBox.Text += "Error saving text file" + "\n";
                }
            }
            else
            {
                LogTextBox.Text += "Text file is empty" + "\n";
            }
        }
    }
}
