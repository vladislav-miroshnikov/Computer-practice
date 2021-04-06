using System;
using System.Collections.Generic;
using System.Linq;
using FutureLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Future.Tests
{
    [TestClass]
    public class FutureUnitTest
    {
        private int[] testArray;

        [TestInitialize]
        public void Init()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            int cap = r.Next(50000);
            List<int> lst = new List<int>();
            for (int i = 0; i < cap; i++)
            {
                lst.Add(r.Next(100));
            }
            testArray = lst.ToArray();
        }

        [TestMethod]
        public void CascadeModelTest()
        {
            CascadeModel cascadeModel = new CascadeModel();
            SingleModel singleModel = new SingleModel();
            int resultSingle = singleModel.ComputeLength(testArray);
            int resultCascade = cascadeModel.ComputeLength(testArray);
            Assert.AreEqual((int)Math.Sqrt(testArray.Sum(x => x * x)), resultSingle);
            Assert.AreEqual((int)Math.Sqrt(testArray.Sum(x => x * x)), resultCascade);
            Assert.AreEqual(resultSingle, resultCascade);
        }

        [TestMethod]
        public void ModifiedCascadeModelTest()
        {
            SingleModel singleModel = new SingleModel();
            ModifiedCascadeModel modifiedCascadeModel = new ModifiedCascadeModel();
            int resultSingle = singleModel.ComputeLength(testArray);
            int resultModifiedCascade = modifiedCascadeModel.ComputeLength(testArray);
            Assert.AreEqual((int)Math.Sqrt(testArray.Sum(x => x * x)), resultSingle);
            Assert.AreEqual((int)Math.Sqrt(testArray.Sum(x => x * x)), resultModifiedCascade);
            Assert.AreEqual(resultSingle, resultModifiedCascade);
        }
    }
}
