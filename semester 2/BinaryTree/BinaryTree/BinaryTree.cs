using System.Diagnostics;

namespace BinaryTree
{
    public class BinaryTree<T> 
    {
        public BinaryTreeNode<T> rootNode { get; set; }
        public BinaryTreeNode<T> Add(BinaryTreeNode<T> node, BinaryTreeNode<T> currentNode = null)
        {
            if (rootNode == null)
            {
                node.parentNode = null;
                return rootNode = node;
            }

            if (currentNode == null)
            {
                currentNode = rootNode;
            }

            node.parentNode = currentNode;
            if ((node.key.CompareTo(currentNode.key)) == 0)
            {
                currentNode.value = node.value; //перезапись
                return currentNode;
            }
            else if ((node.key.CompareTo(currentNode.key)) < 0)
            {
                if (currentNode.leftNode == null)
                {
                    return currentNode.leftNode = node;
                }
                else
                {
                    return Add(node, currentNode.leftNode);
                }
            }
            else
            {
                if (currentNode.rightNode == null)
                {
                    return currentNode.rightNode = node;
                }
                else
                {
                    return Add(node, currentNode.rightNode);
                }
            }
        }

        public BinaryTreeNode<T> Add(int key, T data)
        {
            return Add(new BinaryTreeNode<T>(key, data));
        }

        private BinaryTreeNode<T> FindNode(int key, BinaryTreeNode<T> startWithNode = null)
        {
            if (startWithNode == null)
            {
                startWithNode = rootNode;
            }

            if ((key.CompareTo(startWithNode.key)) == 0)
            {
                return startWithNode;
            }
            else if ((key.CompareTo(startWithNode.key)) < 0)
            {
                if (startWithNode.leftNode == null)
                {
                    
                    return null;
                }
                else
                {
                    return FindNode(key, startWithNode.leftNode);
                }
            }
            else
            {
                if (startWithNode.rightNode == null)
                {                   
                    return null;
                }
                else
                {
                    return FindNode(key, startWithNode.rightNode);
                }
            }           
        }

        public T GetValue(int key)
        {
            if (FindNode(key) != null)
            {
                return FindNode(key).value;
            }
            else
            {
                Debug.WriteLine($"nothing was found for this key = {key}");
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
                Debug.WriteLine($"nothing to delete by key {key}");
                return;
            }

            Side? currentNodeSide = node.nodeSide;
            //The node has no subnodes case
            if (node.leftNode == null && node.rightNode == null)
            {
                if (currentNodeSide == Side.Left)
                {
                    node.parentNode.leftNode = null;
                }
                else if (currentNodeSide == Side.Right)
                {
                    node.parentNode.rightNode = null;
                }
            }
            //No left node, put the right one in place of the deleted case
            else if (node.leftNode == null) 
            {
                if (currentNodeSide == Side.Left)
                {
                    node.parentNode.leftNode = node.rightNode;
                }
                else if (currentNodeSide == Side.Right)
                {
                    node.parentNode.rightNode = node.rightNode;
                }

                node.rightNode.parentNode = node.parentNode;
            }
            //No right node, put the left one in the place of the deleted case
            else if (node.rightNode == null)
            {
                if (currentNodeSide == Side.Left)
                {
                    node.parentNode.leftNode = node.leftNode;
                }
                else if (currentNodeSide == Side.Right)
                {
                    node.parentNode.rightNode = node.leftNode;
                }

                node.leftNode.parentNode = node.parentNode;
            }
            //If both children are present, then the right one is replaced with the left one, 
            //and the left one is inserted into the right one
            else
            {
                switch (currentNodeSide)
                {
                    case Side.Left:
                        node.parentNode.leftNode = node.rightNode;
                        node.rightNode.parentNode = node.parentNode;
                        Add(node.leftNode, node.rightNode);
                        break;
                    case Side.Right:
                        node.parentNode.rightNode = node.rightNode;
                        node.rightNode.parentNode = node.parentNode;
                        Add(node.leftNode, node.rightNode);
                        break;
                    default:
                        BinaryTreeNode<T> auxiliaryLeft = node.leftNode;
                        BinaryTreeNode<T> auxiliaryRightLeft = node.rightNode.leftNode;
                        BinaryTreeNode<T> auxiliaryRightRight = node.rightNode.rightNode;
                        node.key = node.rightNode.key;
                        node.value = node.rightNode.value;
                        node.rightNode = auxiliaryRightRight;
                        node.leftNode = auxiliaryRightLeft;
                        Add(auxiliaryLeft, node);
                        break;
                }
            }
        }

    }
}