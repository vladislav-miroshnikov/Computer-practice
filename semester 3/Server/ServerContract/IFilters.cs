using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerContract
{
    internal interface IFilters
    {
        void Grey(RGB[,] pixels, RGB[,] newPixels, uint height, uint width);
        void Average(RGB[,] pixels, RGB[,] newPixels, uint height, uint width);
        void Gauss3x3Filter(RGB[,] pixels, RGB[,] newPixels, uint height, uint width);
        void Gauss5x5Filter(RGB[,] pixels, RGB[,] newPixels, uint height, uint width);
        void Sobel(RGB[,] pixels, RGB[,] newPixels, uint height, uint width, int number);

    }
}
