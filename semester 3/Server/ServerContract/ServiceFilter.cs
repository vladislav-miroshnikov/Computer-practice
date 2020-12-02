using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Drawing;
using System.IO;
using System.Configuration;

namespace ServerContract
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerSession)]
    public class ServiceFilter : IServerContract
    {
        public void ApplyFilter(byte[] bytes, string filterName)
        {
            Bitmap inputImage = null;
            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                try
                {
                    inputImage = (Bitmap)Image.FromStream(memoryStream);
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Error decoding image");
                    return;
                }
            }
            Bitmap newImage = null;
            RGB[,] pixels = ConvertToArray(inputImage);
            RGB[,] newPixels = new RGB[inputImage.Height, inputImage.Width];
            switch(filterName)
            {
                case "grey":
                    new Filters().Grey(pixels, newPixels, (uint)inputImage.Height, (uint)inputImage.Width);              
                    break;
                case "average":
                    new Filters().Average(pixels, newPixels, (uint)inputImage.Height, (uint)inputImage.Width);
                    break;
                case "gauss3x3":
                    new Filters().Gauss3x3Filter(pixels, newPixels, (uint)inputImage.Height, (uint)inputImage.Width);
                    break;
                case "gauss5x5":
                    new Filters().Gauss5x5Filter(pixels, newPixels, (uint)inputImage.Height, (uint)inputImage.Width);
                    break;
                case "sobelX":
                    new Filters().Sobel(pixels, newPixels, (uint)inputImage.Height, (uint)inputImage.Width, 0);
                    break;
                case "sobelY":
                    new Filters().Sobel(pixels, newPixels, (uint)inputImage.Height, (uint)inputImage.Width, 1);
                    break;
                default:
                    throw new ArgumentException("Check name of filter!");
            }
            newImage = ConvertToBitmap(newPixels, newImage);
        }

        public List<string> GetListOfFilters()
        {
            List<string> list = new List<string>();
            foreach (object filterName in ConfigurationManager.AppSettings)
            {
                list.Add((string)filterName);
            }

            return list;
        }

        private RGB[,] ConvertToArray(Bitmap image)
        {
            RGB[,] pixels = new RGB[image.Height, image.Width];
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    Color color = image.GetPixel(j, i);
                    pixels[i, j].RgbRed = color.R;
                    pixels[i, j].RgbGreen = color.G;
                    pixels[i, j].RgbBlue = color.B;
                }
            }
            return pixels;
        }

        private Bitmap ConvertToBitmap(RGB[,] array, Bitmap newImage)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++) 
                {
                    newImage.SetPixel(j, i, Color.FromArgb(array[i, j].RgbRed, array[i, j].RgbGreen, array[i, j].RgbBlue));          
                }
            }
            return newImage;
        }

    }
}
