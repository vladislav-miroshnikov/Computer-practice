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
            Assert.AreEqual("Object 703", object703Tank.title);
            Assert.AreEqual("USSR", object703Tank.nation);
            Assert.AreEqual(1954, object703Tank.yearOfIssue);
            Assert.AreEqual(5, object703Tank.crew);
            Assert.AreEqual(60.3F, object703Tank.weight);
            Assert.AreEqual(122, object703Tank.gunCaliber);
            Assert.AreEqual(2, object703Tank.numberOfGuns);
        }

        [TestMethod]
        public void MarineTankTest()
        {
            MarineTank pT76 = new MarineTank("PT-76", "USSR", 1952, 3, 14.5F, 76, "floating tank");
            Assert.AreEqual("PT-76", pT76.title);
            Assert.AreEqual("USSR", pT76.nation);
            Assert.AreEqual(1952, pT76.yearOfIssue);
            Assert.AreEqual(3, pT76.crew);
            Assert.AreEqual(14.5F, pT76.weight);
            Assert.AreEqual(76, pT76.gunCaliber);
            Assert.AreEqual("floating tank", pT76.feature);
        }
    }
}
