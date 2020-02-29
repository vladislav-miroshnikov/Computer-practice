using System;
using System.IO;

namespace task1
{
    class Program
    {       
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                throw new ArgumentException("Invalid input!");
            }

            FileStream input = new FileStream(args[0], FileMode.Open);
            Picture bmp = new Picture();
            bmp.Reading(input);
            input.Close();
            string filterName = args[1];
            uint height = bmp.ReturnHeight();
            uint width = bmp.ReturnWidth();
            Picture.newPixels = new RGB[height, width];

            switch (filterName)
            {
                case "grey":
                    Filters.Grey(height, width);
                    break;
                case "average":
                    Filters.Average(height, width);
                    break;
                case "gauss3x3":
                    Filters.Gauss3x3(height, width);
                    break;
                case "gauss5x5":
                    Filters.Gauss5x5(height, width);
                    break;
                case "sobelX":
                    Filters.Sobel(height, width, 0);
                    break;
                case "sobelY":
                    Filters.Sobel(height, width, 1);
                    break;
                default:
                    throw new ArgumentException("check name of filter!");

            }

            FileStream output = new FileStream(args[2], FileMode.Create);
            bmp.Writing(output);
            output.Close();
            
        }
    }
}
