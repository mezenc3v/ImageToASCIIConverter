using System;
using System.Linq;
using System.Windows.Media.Imaging;

namespace ImageToASCIIConverter
{
    public static class ImageConverter
    {
        private const int DataFormatSize = 4;

        public static string ToAscii(BitmapSource sourceImage, int resolution = 10000)
        {
            var imageAsText = "";

            double[,] grayscaleArray = ImageToGrayscale(sourceImage);
            double[,] grayscaleArrayResized = ResizeImage(grayscaleArray, resolution);

            double averageColor = grayscaleArrayResized.Cast<double>().Sum() / grayscaleArrayResized.Length;
            var chars = new char[] { '.', '*' };
            var counter = 0;

            foreach (var pixel in grayscaleArrayResized)
            {
                if (counter == grayscaleArrayResized.GetLength(1))
                {
                    imageAsText += "\r\n";
                    counter = 0;
                }
                if (pixel < averageColor)
                {
                    imageAsText += chars[0];
                }
                else
                {
                    imageAsText += chars[1];
                }
                counter++;
            }

            return imageAsText;
        }

        private static double[,] ImageToGrayscale(BitmapSource image)
        {
            var grayscaleImage = new double[image.PixelHeight, image.PixelWidth];
            var stride = image.PixelWidth * DataFormatSize;
            var size = image.PixelHeight * stride;
            var data = new byte[size];

            image.CopyPixels(data, stride, 0);

            for (int x = 0; x < image.PixelWidth; x++)
            {
                for (int y = 0; y < image.PixelHeight; y++)
                {
                    var pixelIndex = y * stride + DataFormatSize * x;

                    grayscaleImage[y, x] = 0.299 * data[pixelIndex + 1]
                        + 0.5870 * data[pixelIndex + 2]
                        + 0.1140 * data[pixelIndex + 3];
                }
            }

            return grayscaleImage;
        }

        private static double[,] ResizeImage(double[,] image, int resolution)
        {
            var ratio = image.Length / (double)resolution;
            var height = image.GetLength(1) / Math.Sqrt(ratio);
            var width = image.GetLength(0) / Math.Sqrt(ratio);
            var resizedImage = new double[(int)width, (int)height];
            var ratioX = image.GetLength(0) / width;
            var ratioY = image.GetLength(1) / height;
            double stepX = 0;
            double stepY = 0;
            for (int x = 0; x < (int)width; x++)
            {
                for (int y = 0; y < (int)height; y++)
                {
                    resizedImage[x, y] = image[(int)stepX, (int)stepY];
                    stepY += ratioY;
                }
                stepX += ratioX;
                stepY = 0;
            }

            return resizedImage;
        }
    }
}
