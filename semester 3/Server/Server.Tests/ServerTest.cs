using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServerContract;
using ServiceReference;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.ServiceModel;
using System.Threading;

namespace Server.Tests
{
    [TestClass]
    public class ServerTest : IServerContractCallback
    {
        private static Bitmap input = FiltersResource.input;
        private static Bitmap average = FiltersResource.average;
        private static Bitmap gauss3x = FiltersResource.gauss3x;
        private static Bitmap gauss5x = FiltersResource.gauss5x;
        private static Bitmap sobelX = FiltersResource.sobelX;
        private static Bitmap sobelY = FiltersResource.sobelY;
        private static Bitmap grey = FiltersResource.grey;
        private static uint height;
        private static uint width;
        private static byte[] origBytes;
        private static ServerContractClientBase client;
        private static AutoResetEvent waitHandler;
        private static Bitmap filteredImage;
        private static Process process;

       [ClassInitialize]
        public static void Init(TestContext context)
        {
            height = (uint)input.Height;
            width = (uint)input.Width;
            waitHandler = new AutoResetEvent(false);
            using (MemoryStream ms = new MemoryStream())
            {
                input.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                origBytes = ms.GetBuffer();
            }
            //launch server 
            string hostPath = Path.Combine(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\")), @"Server\bin\Debug\Server.exe");
            process = new Process();
            process.StartInfo.FileName = hostPath;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.CreateNoWindow = false; 
            process.Start();
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
            process.Kill(); //kill server
            waitHandler.Dispose();
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

        public void ReturnImage(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                filteredImage = (Bitmap)System.Drawing.Image.FromStream(ms);
            }
            waitHandler.Set();
        }

        public void ReturnProgress(int progress)
        {
            return; //emulation
        }

        private void Check(Bitmap expectedImage, Bitmap actualImage)
        {
            RGB[,] expectedArray = ConvertToArray(expectedImage);
            RGB[,] actualArray = ConvertToArray(actualImage);
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    Assert.AreEqual(expectedArray[i, j].RgbRed, actualArray[i, j].RgbRed);
                    Assert.AreEqual(expectedArray[i, j].RgbGreen, actualArray[i, j].RgbGreen);
                    Assert.AreEqual(expectedArray[i, j].RgbBlue, actualArray[i, j].RgbBlue);
                }
            }

        }

        [TestMethod]
        public void GreyTest()
        {
            client = new ServerContractClientBase(new InstanceContext(this));
            client.ApplyFilter(origBytes, "grey");
            waitHandler.WaitOne(60000); // max time of waiting 
            if (filteredImage == null)
            {
                Assert.Fail("A denial of service has occurred (timed out)");
            }
            Check(grey, filteredImage);
            filteredImage = null;
            client.Abort();
        }

        [TestMethod]
        public void AverageTest()
        {
            client = new ServerContractClientBase(new InstanceContext(this));
            client.ApplyFilter(origBytes, "average");
            waitHandler.WaitOne(60000); // max time of waiting 
            if (filteredImage == null)
            {
                Assert.Fail("A denial of service has occurred (timed out)");
            }
            Check(average, filteredImage);
            filteredImage = null;
            client.Abort();
        }

        [TestMethod]
        public void Gauss3x3Test()
        {
            client = new ServerContractClientBase(new InstanceContext(this));
            client.ApplyFilter(origBytes, "gauss3x3");
            waitHandler.WaitOne(60000); // max time of waiting 
            if (filteredImage == null)
            {
                Assert.Fail("A denial of service has occurred (timed out)");
            }
            Check(gauss3x, filteredImage);
            filteredImage = null;
            client.Abort();
        }

        [TestMethod]
        public void Gauss5x5Test()
        {
            client = new ServerContractClientBase(new InstanceContext(this));
            client.ApplyFilter(origBytes, "gauss5x5");
            waitHandler.WaitOne(60000); // max time of waiting 
            if (filteredImage == null)
            {
                Assert.Fail("A denial of service has occurred (timed out)");
            }
            Check(gauss5x, filteredImage);
            filteredImage = null;
            client.Abort();
        }

        [TestMethod]
        public void SobelXTest()
        {
            client = new ServerContractClientBase(new InstanceContext(this));
            client.ApplyFilter(origBytes, "sobelX");
            waitHandler.WaitOne(60000); // max time of waiting 
            if (filteredImage == null)
            {
                Assert.Fail("A denial of service has occurred (timed out)");
            }
            Check(sobelX, filteredImage);
            filteredImage = null;
            client.Abort();
        }


        [TestMethod]
        public void SobelYTest()
        {
            client = new ServerContractClientBase(new InstanceContext(this));
            client.ApplyFilter(origBytes, "sobelY");
            waitHandler.WaitOne(60000); // max time of waiting 
            if (filteredImage == null)
            {
                Assert.Fail("A denial of service has occurred (timed out)");
            }
            Check(sobelY, filteredImage);
            filteredImage = null;
            client.Abort();
        }

    }
}
