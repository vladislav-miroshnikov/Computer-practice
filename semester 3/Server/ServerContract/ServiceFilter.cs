using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Drawing;
using System.IO;
using System.Configuration;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

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
            Bitmap newImage = new Bitmap(inputImage.Width, inputImage.Height);
            RGB[,] pixels = ConvertToArray(inputImage);
            RGB[,] newPixels = new RGB[inputImage.Height, inputImage.Width];
            switch(filterName)
            {
                case "grey":
                    newPixels = new Filters(OperationContext.Current).Grey(pixels, newPixels, (uint)inputImage.Height, (uint)inputImage.Width);              
                    break;
                case "average":
                    newPixels = new Filters(OperationContext.Current).Average(pixels, newPixels, (uint)inputImage.Height, (uint)inputImage.Width);
                    break;
                case "gauss3x3":
                    newPixels = new Filters(OperationContext.Current).Gauss3x3Filter(pixels, newPixels, (uint)inputImage.Height, (uint)inputImage.Width);
                    break;
                case "gauss5x5":
                    newPixels = new Filters(OperationContext.Current).Gauss5x5Filter(pixels, newPixels, (uint)inputImage.Height, (uint)inputImage.Width);
                    break;
                case "sobelX":
                    newPixels = new Filters(OperationContext.Current).Sobel(pixels, newPixels, (uint)inputImage.Height, (uint)inputImage.Width, 0);
                    break;
                case "sobelY":
                    newPixels = new Filters(OperationContext.Current).Sobel(pixels, newPixels, (uint)inputImage.Height, (uint)inputImage.Width, 1);
                    break;
            }
            if(newPixels == null)
            {
                Console.WriteLine("Error filtering picture");
                return;
            }
            newImage = ConvertToBitmap(newPixels, newImage);
            byte[] newBytes = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                newImage.Save(memoryStream, ImageFormat.Bmp);
                newBytes = memoryStream.GetBuffer();
            }
            try
            {
                OperationContext.Current.GetCallbackChannel<ICallBackContract>().ReturnImage(newBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
