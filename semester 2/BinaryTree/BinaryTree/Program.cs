using System;

namespace BinaryTree
{
    class Program
    {
        static void Main(string[] args)
        {
            BinaryTree<int> binaryTree = new BinaryTree<int>();

            binaryTree.Add(2, 4);
            binaryTree.Add(4, 6);
            binaryTree.Add(1, 5);
            binaryTree.Add(22, 8);
            binaryTree.Add(8, 3);
            binaryTree.GetValue(8);
            binaryTree.RemoveByKey(8);
            binaryTree.RemoveByKey(2);
            binaryTree.GetValue(4);
            binaryTree.GetValue(2);

            Console.ReadKey();
        }
    }
}
