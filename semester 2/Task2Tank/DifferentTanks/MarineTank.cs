using System;
using AbstractLibrary;

namespace DifferentTanks
{
    public class MarineTank : AbstractTank
    {
        public string Feature { get; private set; }

        public MarineTank(string title, string nation, int yearOfIssue, int crew, float weight, float gunCaliber, string feature) 
            : base(title, nation, yearOfIssue, crew, weight, gunCaliber)
        {
            Feature = feature;
        }

        public override string GetInfo()
        {
            string a = base.GetInfo();
            Console.WriteLine($"Feature is: {Feature}\n");
            return (a + $"\nFeature is: {Feature}\n");
        }
    }
}
