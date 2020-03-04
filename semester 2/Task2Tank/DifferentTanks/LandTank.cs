using System;
using AbstractLibrary;

namespace DifferentTanks
{
    public class LandTank : AbstractTank
    {
        public int numberOfGuns { get; set; }

        public LandTank(string _title, string _nation, int _yearOfIssue, int _crew, float _weight, float _gunCaliber, int _numberOfGuns)
            : base(_title, _nation, _yearOfIssue, _crew, _weight, _gunCaliber)
        {
            numberOfGuns = _numberOfGuns;
        }     
  
        public override void GetInfo()
        {
            base.GetInfo();
            Console.WriteLine($"Number of Guns: {numberOfGuns}\n");
        }

    }
}
