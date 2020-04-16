using System;
using DifferentTanks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Task2Tank.Tests
{
    [TestClass]
    public class ClassTest
    {
 
        [TestMethod]
        public void LandTankTest()
        {
            LandTank object703Tank = new LandTank("Object 703", "USSR", 1954, 5, 60.3F, 122, 2);
            Assert.AreEqual("Object 703", object703Tank.Title);
            Assert.AreEqual("USSR", object703Tank.Nation);
            Assert.AreEqual(1954, object703Tank.YearOfIssue);
            Assert.AreEqual(5, object703Tank.Crew);
            Assert.AreEqual(60.3F, object703Tank.Weight);
            Assert.AreEqual(122, object703Tank.GunCaliber);
            Assert.AreEqual(2, object703Tank.NumberOfGuns);
            Assert.AreEqual("Name of the tank: Object 703\nCountry of origin: USSR\nYear of issue: 1954\n" +
                "Crew: 5 persons\nTank weight : 60,3 tons\nGun caliber: 122 mm\nNumber of Guns: 2\n", object703Tank.GetInfo());
        }

        [TestMethod]
        public void MarineTankTest()
        {
            MarineTank pT76 = new MarineTank("PT-76", "USSR", 1952, 3, 14.5F, 76, "floating tank");
            Assert.AreEqual("PT-76", pT76.Title);
            Assert.AreEqual("USSR", pT76.Nation);
            Assert.AreEqual(1952, pT76.YearOfIssue);
            Assert.AreEqual(3, pT76.Crew);
            Assert.AreEqual(14.5F, pT76.Weight);
            Assert.AreEqual(76, pT76.GunCaliber);
            Assert.AreEqual("floating tank", pT76.Feature);
            Assert.AreEqual("Name of the tank: PT-76\nCountry of origin: USSR\nYear of issue: 1952\n" +
                "Crew: 3 persons\nTank weight : 14,5 tons\nGun caliber: 76 mm\nFeature is: floating tank\n", pT76.GetInfo());
        }
    }
}
