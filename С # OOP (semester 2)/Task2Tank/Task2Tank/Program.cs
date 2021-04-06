using System;
using DifferentTanks;

namespace Task2Tank
{
    class Program
    {
        static void Main(string[] args)
        {
            LandTank object703Tank = new LandTank("Object 703", "USSR", 1954, 5, 60.3F, 122, 2);
            Console.WriteLine("Information about the first tank:\n");
            object703Tank.GetInfo();
            Console.WriteLine("Information about the second tank:\n");
            MarineTank pT76 = new MarineTank("PT-76", "USSR", 1952, 3, 14.5F, 76, "floating tank");
            pT76.GetInfo();
            Console.ReadKey();
        }
        
    }
}
