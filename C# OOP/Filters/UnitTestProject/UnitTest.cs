using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using Task1Filters;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest
    {

        private static Bitmap input = UnitTestProject.Resource.input;
        private static Bitmap average = UnitTestProject.Resource.average;
        private static Bitmap gauss3x = UnitTestProject.Resource.gauss3x;
        private static Bitmap gauss5x = UnitTestProject.Resource.gauss5x;
        private static Bitmap sobelX = UnitTestProject.Resource.sobelX;
        private static Bitmap sobelY = UnitTestProject.Resource.sobelY;
        private static Bitmap grey = UnitTestProject.Resource.grey;
        private static uint height;
        private static uint width;
        private static RGB[,] pixelsStandart;
        private static RGB[,] pixelsOriginal;
        private static Picture bmp;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            height = (uint)input.Height;
            width = (uint)input.Width;
            bmp = new Picture();
            pixelsStandart = new RGB[height, width];
            pixelsOriginal = new RGB[height, width];
           bmp.Pixels = new RGB[height, width];
           bmp.Pixels = DownloadImage(input);
           bmp.NewPixels =bmp.Pixels;
        }

        [ClassCleanup]
        public static void Clean()
        {
            input.Dispose();
            average.Dispose();
            gauss3x.Dispose();
            gauss5x.Dispose();
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

        private void Check()
        {
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    Assert.AreEqual(pixelsStandart[i, j].rgbRed,bmp.NewPixels[i, j].rgbRed);
                    Assert.AreEqual(pixelsStandart[i, j].rgbGreen,bmp.NewPixels[i, j].rgbGreen);
                    Assert.AreEqual(pixelsStandart[i, j].rgbBlue,bmp.NewPixels[i, j].rgbBlue);
                }
            }

        }

        [TestMethod]
        public void GreyTest()
        {                       
            Filters.Grey(bmp.Pixels, bmp.NewPixels, height, width);           
            pixelsStandart = DownloadImage(grey);
            Check();
        }

        [TestMethod]
        public void AverageTest()
        {
            Filters.Average(bmp.Pixels, bmp.NewPixels, height, width);
            pixelsStandart = DownloadImage(average);
            Check();
        }

        [TestMethod]
        public void Gauss3x3Test()
        {
            Filters.Gauss3x3Filter(bmp.Pixels, bmp.NewPixels, height, width);
            pixelsStandart = DownloadImage(gauss3x);
            Check();
        }

        [TestMethod]
        public void Gauss5x5Test()
        {
            Filters.Gauss5x5Filter(bmp.Pixels, bmp.NewPixels, height, width);
            pixelsStandart = DownloadImage(gauss5x);
            Check();
        }

        [TestMethod]
        public void SobelXTest()
        {
            Filters.Sobel(bmp.Pixels, bmp.NewPixels, height, width, 0);
            pixelsStandart = DownloadImage(sobelX);
            Check();
        }

        [TestMethod]
        public void SobelYTest()
        {
            Filters.Sobel(bmp.Pixels, bmp.NewPixels, height, width, 1);
            pixelsStandart = DownloadImage(sobelY);
            Check();
        }
    }
}
