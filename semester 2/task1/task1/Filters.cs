namespace task1
{
    public class Filters
    {
    
        private static void Convolution(int[] matrix, int i, int j, int size, int stride, int divisionRatio)
        {
            int color1 = 0, color2 = 0, color3 = 0;
            if (matrix == null)
            {
                color1 = color2 = color3 = ((Picture.pixels[i,j].rgbRed + Picture.pixels[i,j].rgbGreen + Picture.pixels[i,j].rgbBlue) / divisionRatio);
            }

            else
            {
                
                for (int u = 0; u < size * size; u++) 
                {
                    RGB pix = Picture.pixels[i - stride + u / size, j - stride + u % size];
                    color1 += pix.rgbRed * matrix[u] / divisionRatio;
                    color2 += pix.rgbGreen * matrix[u] / divisionRatio;
                    color3 += pix.rgbBlue * matrix[u] / divisionRatio;
                }
            }

            if (color1 < 0) color1 = 0;
            if (color2 < 0) color2 = 0;
            if (color3 < 0) color3 = 0;
            if (color1 > 255) color1 = 255;
            if (color2 > 255) color2 = 255;
            if (color3 > 255) color3 = 255;
            Picture.newPixels[i, j].rgbRed = (byte)color1;
            Picture.newPixels[i, j].rgbGreen = (byte)color2;
            Picture.newPixels[i, j].rgbBlue = (byte)color3;

        }

        public static void Grey(uint height, uint width)
        {
   
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    Filters.Convolution(null, i, j, 0, 0, 3);
                }
            }
        }

        public static void Average(uint height, uint width)
        {
            int[] matrix = new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            for (var i = 1; i < height - 1; i++) 
            {
                for (var j = 1; j < width - 1; j++)
                {
                    Filters.Convolution(matrix, i, j, 3, 1, 9);
                }
            }
        }
        
        public static void Gauss3x3(uint height, uint width)
        {
            int[] matrix = new int[9] { 1, 2, 1, 2, 4, 2, 1, 2, 1 };
            for (var i = 1; i < height - 1; i++)
            {
                for (var j = 1; j < width - 1; j++)
                {
                    Filters.Convolution(matrix, i, j, 3, 1, 16);
                }
            }
        }

        public static void Gauss5x5(uint height, uint width)
        {
            int[] matrix = new int[25] { 1, 4, 6, 4, 1, 4, 16, 24, 16, 4, 6, 24, 36, 24, 6, 4, 16, 24, 16, 4, 1, 4, 6, 4, 1 };
            for (var i = 2; i < height - 2; i++)
            {
                for (var j = 2; j < width - 2; j++)
                {
                    Filters.Convolution(matrix, i, j, 5, 2, 256);
                }
            }
        }

        public static void Sobel(uint height, uint width, int number)
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
                    Filters.Convolution(matrix, i, j, 3, 1, 1);
                }
            }

        }
    }
}
