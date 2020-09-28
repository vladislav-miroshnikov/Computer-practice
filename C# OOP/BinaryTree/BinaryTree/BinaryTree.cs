using System;
using System.Diagnostics;

namespace BinaryTree
{
    public class BinaryTree<T> 
    {
        public BinaryTreeNode<T> RootNode { get; set; }
        public BinaryTreeNode<T> Add(BinaryTreeNode<T> node, BinaryTreeNode<T> currentNode = null)
        {
            if (RootNode == null)
            {
                node.ParentNode = null;
                return RootNode = node;
            }

            if (currentNode == null)
            {
                currentNode = RootNode;
            }

            node.ParentNode = currentNode;
            if ((node.Key.CompareTo(currentNode.Key)) == 0)
            {
                currentNode.Value = node.Value; //перезапись
                return currentNode;
            }
            else if ((node.Key.CompareTo(currentNode.Key)) < 0)
            {
                if (currentNode.LeftNode == null)
                {
                    return currentNode.LeftNode = node;
                }
                else
                {
                    return Add(node, currentNode.LeftNode);
                }
            }
            else
            {
                if (currentNode.RightNode == null)
                {
                    return currentNode.RightNode = node;
                }
                else
                {
                    return Add(node, currentNode.RightNode);
                }
            }
        }

        public void Add(int key, T data)
        {
            Add(new BinaryTreeNode<T>(key, data));
        }

        private BinaryTreeNode<T> FindNode(int key, BinaryTreeNode<T> startWithNode = null)
        {
            if (startWithNode == null)
            {
                startWithNode = RootNode;
            }

            if ((key.CompareTo(startWithNode.Key)) == 0)
            {
                return startWithNode;
            }
            else if ((key.CompareTo(startWithNode.Key)) < 0)
            {
                if (startWithNode.LeftNode == null)
                {
                    
                    return null;
                }
                else
                {
                    return FindNode(key, startWithNode.LeftNode);
                }
            }
            else
            {
                if (startWithNode.RightNode == null)
                {                   
                    return null;
                }
                else
                {
                    return FindNode(key, startWithNode.RightNode);
                }
            }           
        }

        public T GetValue(int key)
        {
            if (FindNode(key) != null)
            {
                Console.WriteLine($"value for key {key} - {FindNode(key).Value}");
                return FindNode(key).Value;
            }
            else
            {
                Console.WriteLine($"nothing was found for this Key = {key}");
                return (default);
            }
        }

        public void RemoveByKey(int key)
        {
            BinaryTreeNode<T> node = FindNode(key);
            Remove(node, key);
        }

        private void Remove(BinaryTreeNode<T> node, int key)
        {
            if (node == null)
            {
                Console.WriteLine($"nothing to delete by Key {key}");
                return;
            }

            Side? currentNodeSide = node.nodeSide;
            //The node has no subnodes case
            if (node.LeftNode == null && node.RightNode == null)
            {
                if (currentNodeSide == Side.Left)
                {
                    node.ParentNode.LeftNode = null;
                }
                else if (currentNodeSide == Side.Right)
                {
                    node.ParentNode.RightNode = null;
                }
            }
            //No left node, put the right one in place of the deleted case
            else if (node.LeftNode == null) 
            {
                if (currentNodeSide == Side.Left)
                {
                    node.ParentNode.LeftNode = node.RightNode;
                }
                else if (currentNodeSide == Side.Right)
                {
                    node.ParentNode.RightNode = node.RightNode;
                }

                node.RightNode.ParentNode = node.ParentNode;
            }
            //No right node, put the left one in the place of the deleted case
            else if (node.RightNode == null)
            {
                if (currentNodeSide == Side.Left)
                {
                    node.ParentNode.LeftNode = node.LeftNode;
                }
                else if (currentNodeSide == Side.Right)
                {
                    node.ParentNode.RightNode = node.LeftNode;
                }

                node.LeftNode.ParentNode = node.ParentNode;
            }
            //If both children are present, then the right one is replaced with the left one, 
            //and the left one is inserted into the right one
            else
            {
                switch (currentNodeSide)
                {
                    case Side.Left:
                        node.ParentNode.LeftNode = node.RightNode;
                        node.RightNode.ParentNode = node.ParentNode;
                        Add(node.LeftNode, node.RightNode);
                        break;
                    case Side.Right:
                        node.ParentNode.RightNode = node.RightNode;
                        node.RightNode.ParentNode = node.ParentNode;
                        Add(node.LeftNode, node.RightNode);
                        break;
                    default:
                        BinaryTreeNode<T> auxiliaryLeft = node.LeftNode;
                        BinaryTreeNode<T> auxiliaryRightLeft = node.RightNode.LeftNode;
                        BinaryTreeNode<T> auxiliaryRightRight = node.RightNode.RightNode;
                        node.Key = node.RightNode.Key;
                        node.Value = node.RightNode.Value;
                        node.RightNode = auxiliaryRightRight;
                        node.LeftNode = auxiliaryRightLeft;
                        Add(auxiliaryLeft, node);
                        break;
                }
            }
        }
    }
}