using Microsoft.VisualStudio.TestTools.UnitTesting;
using BinaryTree;

namespace BinaryTreeUnitTest
{
    [TestClass]
    public class BinaryTreeUnitTest
    {
        [TestMethod]
        public void BinaryTreeIntTest()
        {
            BinaryTree<int> tree = new BinaryTree<int>();
            tree.Add(5, 6);
            tree.Add(1, 4);
            tree.Add(7, 9);
            tree.Add(2, 112);
            tree.Add(4, 6);
            Assert.AreEqual(9, tree.GetValue(7));
            Assert.AreEqual(112, tree.GetValue(2));
            Assert.AreEqual(6, tree.GetValue(4));
            Assert.AreEqual(4, tree.GetValue(1));
            tree.RemoveByKey(5);
            tree.RemoveByKey(2);
            Assert.AreEqual(default, tree.GetValue(5));
            Assert.AreEqual(4, tree.GetValue(1));
        }

        [TestMethod]
        public void BinaryTreeStringTest()
        {
            BinaryTree<string> tree = new BinaryTree<string>();
            tree.Add(6, "Hello");
            tree.Add(3, "My name is");
            tree.Add(9, "David");
            tree.Add(231, "Dobrik");
            Assert.AreEqual("David", tree.GetValue(9));
            Assert.AreEqual("My name is", tree.GetValue(3));
            tree.RemoveByKey(6);
            Assert.AreEqual(default, tree.GetValue(6));
            Assert.AreEqual("Dobrik", tree.GetValue(231));
        }

        [TestMethod]
        public void BinaryTreeDoubleTest()
        {
            BinaryTree<double> tree = new BinaryTree<double>();
            tree.Add(50, 3.23);
            tree.Add(13, 8.01);
            tree.Add(90, 2342.2);
            tree.Add(900, 11.92929);
            Assert.AreEqual(3.23, tree.GetValue(50));
            Assert.AreEqual(8.01, tree.GetValue(13));
            Assert.AreEqual(2342.2, tree.GetValue(90));
            Assert.AreEqual(11.92929, tree.GetValue(900));
            tree.RemoveByKey(50);
            tree.RemoveByKey(90);
            Assert.AreEqual(default, tree.GetValue(50));
            Assert.AreEqual(default, tree.GetValue(90));
            Assert.AreEqual(8.01, tree.GetValue(13));
            Assert.AreEqual(11.92929, tree.GetValue(900));
        }
    }
}
