using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerContract
{
    internal interface IFilters
    {
        RGB[,] Grey(RGB[,] pixels, RGB[,] newPixels, uint height, uint width);
        RGB[,] Average(RGB[,] pixels, RGB[,] newPixels, uint height, uint width);
        RGB[,] Gauss3x3Filter(RGB[,] pixels, RGB[,] newPixels, uint height, uint width);
        RGB[,] Gauss5x5Filter(RGB[,] pixels, RGB[,] newPixels, uint height, uint width);
        RGB[,] Sobel(RGB[,] pixels, RGB[,] newPixels, uint height, uint width, int number);

    }
}
