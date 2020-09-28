using System;
using AbstractLibrary;

namespace DifferentTanks
{
    public class LandTank : AbstractTank
    {
        public int NumberOfGuns { get; private set; }

        public LandTank(string title, string nation, int yearOfIssue, int crew, float weight, float gunCaliber, int numberOfGuns)
            : base(title, nation, yearOfIssue, crew, weight, gunCaliber)
        {
            NumberOfGuns = numberOfGuns;
        }     
  
        public override string GetInfo()
        {
            string a = base.GetInfo();
            Console.WriteLine($"Number of Guns: {NumberOfGuns}\n");
            return (a + $"\nNumber of Guns: {NumberOfGuns}\n");
        }

    }
}
