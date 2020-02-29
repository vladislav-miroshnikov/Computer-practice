using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace task1.test
{
    [TestClass]
    public class UnitTest1
    {
        private static Bitmap input = task1.test.Resource1.input;
        private static Bitmap average = task1.test.Resource1.average;
        private static Bitmap gauss3 = task1.test.Resource1.gauss3;
        private static Bitmap gauss5 = task1.test.Resource1.gauss5;
        private static Bitmap sobelX = task1.test.Resource1.sobelX;
        private static Bitmap sobelY = task1.test.Resource1.sobelY;
        private static Bitmap grey = task1.test.Resource1.grey;
        private static uint height;
        private static uint width;
        private static RGB[,] pixelsOriginal;
        private static RGB[,] pixelsStandart;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
           
            height = (uint)input.Height;
            width = (uint)input.Width;
            pixelsOriginal = new RGB[height, width];
            pixelsStandart = new RGB[height, width];
            Picture.pixels = DownloadImage(input);
            Picture.newPixels = Picture.pixels;

        }

        [ClassCleanup]
        public static void Clean()
        {
            input.Dispose();
            average.Dispose();
            gauss3.Dispose();
            gauss5.Dispose();
            sobelX.Dispose();
            sobelY.Dispose();
            grey.Dispose();
        }

        private static RGB[,] DownloadImage(Bitmap file)
        {

            RGB struct2 = new RGB();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    pixelsOriginal[i, j] = ConvertStruct(file.GetPixel(j, i), struct2);
                }
            }
            return pixelsOriginal;

        }

        private static RGB ConvertStruct(Color struct1, RGB struct2)
        {
            struct2.rgbRed = struct1.R;
            struct2.rgbGreen = struct1.G;
            struct2.rgbBlue = struct1.B;

            return (struct2);
        }

        private bool CheckPixels()
        {
            bool a = true;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (Picture.newPixels[i, j].rgbRed != pixelsStandart[i, j].rgbRed ||
                        Picture.newPixels[i, j].rgbGreen != pixelsStandart[i, j].rgbGreen ||
                        Picture.newPixels[i, j].rgbBlue != pixelsStandart[i, j].rgbBlue)
                    {
                        a = false;
                    }
                }
            }
            return a;
        }

        [TestMethod]
        public void GreyTest()
        {          
            Filters.Grey(height, width);
            pixelsStandart = DownloadImage(grey);
            bool a = CheckPixels();
            if (a == false)
            {
                throw new Exception("Grey filter error");            
            }
        }

        [TestMethod]
        public void AverageTest()
        {
            Filters.Average(height, width);
            pixelsStandart = DownloadImage(average);
            bool a = CheckPixels();
            if (a == false)
            {
                throw new Exception("Average filter error");
            }
        }

        [TestMethod]
        public void Gauss3x3Test()
        {
            Filters.Gauss3x3(height, width);
            pixelsStandart = DownloadImage(gauss3);
            bool a = CheckPixels();
            if (a == false)
            {
                throw new Exception("Gauss3x3 filter error");
            }
        }

        [TestMethod]
        public void Gauss5x5Test()
        {
            Filters.Gauss5x5(height, width);
            pixelsStandart = DownloadImage(gauss5);
            bool a = CheckPixels();
            if (a == false)
            {
                throw new Exception("Gauss5x5 filter error");
            }
        }

        [TestMethod]
        public void SobelXTest()
        {          
            Filters.Sobel(height, width, 0);
            pixelsStandart = DownloadImage(sobelX);
            bool a = CheckPixels();
            if (a == false)
            {
                throw new Exception("SobelX filter error");
            }
        }

        [TestMethod]
        public void SobelYTest()
        {           
            Filters.Sobel(height, width, 1);
            pixelsStandart = DownloadImage(sobelY);
            bool a = CheckPixels();
            if (a == false)
            {
                throw new Exception("SobelY filter error");
            }
        }

        

    }
}
