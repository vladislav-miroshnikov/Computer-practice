using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace ServerContract
{
    public class Filters : IFilters
    {
        private OperationContext context;

        public Filters(OperationContext context)
        {
            this.context = context;
        }

        private RGB[,] Convolution(RGB[,] pixels, RGB[,] newPixels, int[] matrix, int i, int j, int size, int stride, int divisionRatio)
        {
            int colorRed = 0, colorGreen = 0, colorBlue = 0;
            if (matrix == null)
            {
                colorRed = colorGreen = colorBlue = ((pixels[i, j].RgbRed + pixels[i, j].RgbGreen + pixels[i, j].RgbBlue) / divisionRatio);
            }

            else
            {

                for (int u = 0; u < size * size; u++)
                {
                    RGB pix = pixels[i - stride + u / size, j - stride + u % size];
                    colorRed += pix.RgbRed * matrix[u] / divisionRatio;
                    colorGreen += pix.RgbGreen * matrix[u] / divisionRatio;
                    colorBlue += pix.RgbBlue * matrix[u] / divisionRatio;
                }

            }

            if (colorRed < 0) colorRed = 0;
            if (colorGreen < 0) colorGreen = 0;
            if (colorBlue < 0) colorBlue = 0;
            if (colorRed > 255) colorRed = 255;
            if (colorGreen > 255) colorGreen = 255;
            if (colorBlue > 255) colorBlue = 255;
            newPixels[i, j].RgbRed = (byte)colorRed;
            newPixels[i, j].RgbGreen = (byte)colorGreen;
            newPixels[i, j].RgbBlue = (byte)colorBlue;
            return newPixels;
        }

        private int GetCallBack(int progress)
        {
            try
            {
                context.GetCallbackChannel<ICallBackContract>().ReturnProgress(progress);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
            return 0;
        }

        public RGB[,] Grey(RGB[,] pixels, RGB[,] newPixels, uint height, uint width)
        {
            int progress = 0;

            ParallelOptions parallelOptions = new ParallelOptions();
            CancellationTokenSource cancellation = new CancellationTokenSource();

            parallelOptions.CancellationToken = cancellation.Token;

            try
            {

                ParallelLoopResult loopResult = Parallel.For
                (0,
                height,
                parallelOptions,
                i =>
                {
                    for (var j = 0; j < width; j++)
                    {
                        if (cancellation.IsCancellationRequested)
                        {
                            break;
                        }
                        Convolution(pixels, newPixels, null, (int)i, j, 0, 0, 3);
                    }
                    Interlocked.Increment(ref progress);


                    if (GetCallBack((int)(progress / (double)height * 100)) == 1) 
                    {
                        cancellation.Cancel();
                    }

                });
            }
            catch (OperationCanceledException cancel)
            {
                return null;
            }
            finally
            {
                cancellation.Dispose();
            }
            return newPixels;
        }

        public RGB[,] Average(RGB[,] pixels, RGB[,] newPixels, uint height, uint width)
        {
            int[] matrix = new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            int progress = 2;

            ParallelOptions parallelOptions = new ParallelOptions();
            CancellationTokenSource cancellation = new CancellationTokenSource();

            parallelOptions.CancellationToken = cancellation.Token;
            try
            {
                ParallelLoopResult loopResult = Parallel.For
                (1,
                height - 1,
                parallelOptions,
                i =>
                {
                    for (var j = 1; j < width - 1; j++)
                    {
                        if (cancellation.IsCancellationRequested)
                        {
                            break;
                        }
                        Convolution(pixels, newPixels, matrix, (int)i, j, 3, 1, 9);
                    }
                    Interlocked.Increment(ref progress);

                    if (GetCallBack((int)(progress / (double)height * 100)) == 1)
                    {
                        cancellation.Cancel();
                    }
                });
            }
            catch (OperationCanceledException cancel)
            {
                return null;
            }
            finally
            {
                cancellation.Dispose();
            }
            return newPixels;
        }

        public RGB[,] Gauss3x3Filter(RGB[,] pixels, RGB[,] newPixels, uint height, uint width)
        {
            int[] matrix = new int[9] { 1, 2, 1, 2, 4, 2, 1, 2, 1 };
            int progress = 2;

            ParallelOptions parallelOptions = new ParallelOptions();
            CancellationTokenSource cancellation = new CancellationTokenSource();

            parallelOptions.CancellationToken = cancellation.Token;
            try
            {
                ParallelLoopResult loopResult = Parallel.For
                (1,
                height - 1,
                parallelOptions,
                i =>
                {
                    for (var j = 1; j < width - 1; j++)
                    {
                        if (cancellation.IsCancellationRequested)
                        {
                            break;
                        }
                        Convolution(pixels, newPixels, matrix, (int)i, j, 3, 1, 16);
                    }
                    Interlocked.Increment(ref progress);

                    if (GetCallBack((int)(progress / (double)height * 100)) == 1)
                    {
                        cancellation.Cancel();
                    }
                });
            }
            catch (OperationCanceledException cancel)
            {
                return null;
            }
            finally
            {
                cancellation.Dispose();
            }
            return newPixels;
        }

        public RGB[,] Gauss5x5Filter(RGB[,] pixels, RGB[,] newPixels, uint height, uint width)
        {
            int[] matrix = new int[25] { 1, 4, 6, 4, 1, 4, 16, 24, 16, 4, 6, 24, 36, 24, 6, 4, 16, 24, 16, 4, 1, 4, 6, 4, 1 };
            int progress = 4;

            ParallelOptions parallelOptions = new ParallelOptions();
            CancellationTokenSource cancellation = new CancellationTokenSource();

            parallelOptions.CancellationToken = cancellation.Token;

            try
            {
                ParallelLoopResult loopResult = Parallel.For
                (2,
                height - 2,
                parallelOptions,
                i =>
                {
                    for (var j = 2; j < width - 2; j++)
                    {
                        if (cancellation.IsCancellationRequested)
                        {
                            break;
                        }
                        Convolution(pixels, newPixels, matrix, (int)i, j, 5, 2, 256);
                    }
                    Interlocked.Increment(ref progress);

                    if (GetCallBack((int)(progress / (double)height * 100)) == 1)
                    {
                        cancellation.Cancel();
                    }
                });
            }
            catch (OperationCanceledException cancel)
            {
                return null;
            }
            finally
            {
                cancellation.Dispose();
            }
            return newPixels;
        }

        public RGB[,] Sobel(RGB[,] pixels, RGB[,] newPixels, uint height, uint width, int number)
        {
            int[] matrix = new int[9];
            if (number == 0)  //sobelX
            {
                matrix[0] = 1;
                matrix[1] = 2;
                matrix[2] = 1;
                matrix[3] = 0;
                matrix[4] = 0;
                matrix[5] = 0;
                matrix[6] = -1;
                matrix[7] = -2;
                matrix[8] = -1;
            }

            else if (number == 1)  //sobelY
            {
                matrix[0] = -1;
                matrix[1] = 0;
                matrix[2] = 1;
                matrix[3] = -2;
                matrix[4] = 0;
                matrix[5] = 2;
                matrix[6] = -1;
                matrix[7] = 0;
                matrix[8] = 1;
            }


            int progress = 2;

            ParallelOptions parallelOptions = new ParallelOptions();
            CancellationTokenSource cancellation = new CancellationTokenSource();

            parallelOptions.CancellationToken = cancellation.Token;
            try
            {
                ParallelLoopResult loopResult = Parallel.For
                (1,
                height - 1,
                parallelOptions,
                i =>
                {

                    for (var j = 1; j < width - 1; j++)
                    {
                        if (cancellation.IsCancellationRequested)
                        {
                            break;
                        }
                        Convolution(pixels, newPixels, matrix, (int)i, j, 3, 1, 1);
                    }
                    Interlocked.Increment(ref progress);

                    if (GetCallBack((int)(progress / (double)height * 100)) == 1)
                    {
                        cancellation.Cancel();
                    }
                });
            }
            catch (OperationCanceledException cancel)
            {
                return null;
            }
            finally
            {
                cancellation.Dispose();
            }


            return newPixels;
        }
    }
}
