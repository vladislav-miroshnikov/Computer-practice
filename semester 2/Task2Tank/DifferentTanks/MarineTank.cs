using System;
using AbstractLibrary;

namespace DifferentTanks
{
    public class MarineTank : AbstractTank
    {
        public string feature { get; set; }

        public MarineTank(string _title, string _nation, int _yearOfIssue, int _crew, float _weight, float _gunCaliber, string _feature) 
            : base(_title, _nation, _yearOfIssue, _crew, _weight, _gunCaliber)
        {
            feature = _feature;
        }

        public override void GetInfo()
        {
            base.GetInfo();
            Console.WriteLine($"Feature is : {feature}\n");
        }
    }
}
