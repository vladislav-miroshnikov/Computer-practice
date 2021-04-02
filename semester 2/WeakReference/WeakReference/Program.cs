using System;
using System.Threading;

namespace WeakReference
{

    class Program
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
        static void Main(string[] args)
        {
            WeakBinaryTree<TestClass> binaryTree = new WeakBinaryTree<TestClass>(1500);
            binaryTree.Add(20, new TestClass("a"));
            binaryTree.Add(11, new TestClass("b"));
            binaryTree.Add(34, new TestClass("c"));
            binaryTree.Add(24, new TestClass("c"));
            binaryTree.Add(3214, new TestClass("rrrrrr"));
            binaryTree.GetValue(24);
            binaryTree.RemoveByKey(20);
            binaryTree.RemoveByKey(11);
            binaryTree.RemoveByKey(34);
            binaryTree.GetValue(3214);
            binaryTree.GetValue(24);
            binaryTree.GetValue(1);
            Console.WriteLine("Garbage collection");
            GC.Collect();
            Console.WriteLine("After 2500 milliseconds");
            Thread.Sleep(2500);
            GC.Collect();
           
            binaryTree.GetValue(24);
            binaryTree.GetValue(1);
            Console.ReadKey();
        }
    }
}
