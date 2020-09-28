using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeakReference;

namespace WeakReferenceUnitTest
{
    [TestClass]
    public class WeakReferenceUnitTest
    {
        private class TestClass
        {
            private string name;
            public TestClass(string name)
            {
                this.name = name;
            }
            public override string ToString()
            {
                return name;
            }
        }

        [TestMethod]
        public void WeakReferenceBinaryTreeTest()
        {
            WeakBinaryTree<TestClass> binaryTree = new WeakBinaryTree<TestClass>(1000);
            binaryTree.Add(20, new TestClass("Hello"));
            binaryTree.Add(11, new TestClass("My"));
            binaryTree.Add(34, new TestClass("Name"));
            binaryTree.Add(24, new TestClass("Beautiful!"));
            binaryTree.Add(3214, new TestClass("Good day"));
            Assert.AreEqual("My", binaryTree.GetValue(11).ToString());
            Assert.AreEqual("Name", binaryTree.GetValue(34).ToString());
            Assert.AreEqual("Beautiful!", binaryTree.GetValue(24).ToString());
            binaryTree.RemoveByKey(20);
            binaryTree.RemoveByKey(11);
            binaryTree.RemoveByKey(34);
            Assert.AreEqual(default, binaryTree.GetValue(11));
            GC.Collect();
            Console.WriteLine("Make delay");
            Thread.Sleep(1800);
            GC.Collect();
            Assert.AreEqual(default, binaryTree.GetValue(24)); //all elements deleted
            Assert.AreEqual(default, binaryTree.GetValue(3214));
        }
    }
}
