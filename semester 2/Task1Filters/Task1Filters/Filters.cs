namespace Task1Filters
{
    public class Filters
    {
        private static void Convolution(RGB [,] pixels, RGB[,] newPixels, int[] matrix, int i, int j, int size, int stride, int divisionRatio)
        {
            int colorRed = 0, colorGreen = 0, colorBlue = 0;
            if (matrix == null)
            {
                colorRed = colorGreen = colorBlue = ((pixels[i, j].rgbRed + pixels[i, j].rgbGreen + pixels[i, j].rgbBlue) / divisionRatio);
            }

            else
            {

                for (int u = 0; u < size * size; u++)
                {
                    RGB pix = pixels[i - stride + u / size, j - stride + u % size];
                    colorRed += pix.rgbRed * matrix[u] / divisionRatio;
                    colorGreen += pix.rgbGreen * matrix[u] / divisionRatio;
                    colorBlue += pix.rgbBlue * matrix[u] / divisionRatio;
                }

            }

            if (colorRed < 0) colorRed = 0;
            if (colorGreen < 0) colorGreen = 0;
            if (colorBlue < 0) colorBlue = 0;
            if (colorRed > 255) colorRed = 255;
            if (colorGreen > 255) colorGreen = 255;
            if (colorBlue > 255) colorBlue = 255;
            newPixels[i, j].rgbRed = (byte)colorRed;
            newPixels[i, j].rgbGreen = (byte)colorGreen;
            newPixels[i, j].rgbBlue = (byte)colorBlue;

        }

        public static void Grey(RGB[,] pixels, RGB[,] newPixels, uint height, uint width)
        {

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    Filters.Convolution(pixels, newPixels, null, i, j, 0, 0, 3);
                }
            }
        }

        public static void Average(RGB[,] pixels, RGB[,] newPixels, uint height, uint width)
        {
            int[] matrix = new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            for (var i = 1; i < height - 1; i++)
            {
                for (var j = 1; j < width - 1; j++)
                {
                    Filters.Convolution(pixels, newPixels, matrix, i, j, 3, 1, 9);
                }
            }
        }

        public static void Gauss3x3Filter(RGB[,] pixels, RGB[,] newPixels, uint height, uint width)
        {
            int[] matrix = new int[9] { 1, 2, 1, 2, 4, 2, 1, 2, 1 };
            for (var i = 1; i < height - 1; i++)
            {
                for (var j = 1; j < width - 1; j++)
                {
                    Filters.Convolution(pixels, newPixels, matrix, i, j, 3, 1, 16);
                }
            }
        }

        public static void Gauss5x5Filter(RGB[,] pixels, RGB[,] newPixels, uint height, uint width)
        {
            int[] matrix = new int[25] { 1, 4, 6, 4, 1, 4, 16, 24, 16, 4, 6, 24, 36, 24, 6, 4, 16, 24, 16, 4, 1, 4, 6, 4, 1 };
            for (var i = 2; i < height - 2; i++)
            {
                for (var j = 2; j < width - 2; j++)
                {
                    Filters.Convolution(pixels, newPixels, matrix, i, j, 5, 2, 256);
                }
            }
        }

        public static void Sobel(RGB[,] pixels, RGB[,] newPixels, uint height, uint width, int number)
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

            for (var i = 1; i < height - 1; i++)
            {
                for (var j = 1; j < width - 1; j++)
                {
                    Filters.Convolution(pixels, newPixels, matrix, i, j, 3, 1, 1);
                }
            }

        }
    }
}
