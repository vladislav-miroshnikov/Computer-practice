using System.Diagnostics;
using System.Threading.Tasks;

namespace WeakReference
{
    public class WeakBinaryTree<T> where T : class
    {
            
        private int time;

        public WeakBinaryTree(int time)
        {
            this.time = time;
        }

        public WeakBinaryTreeNode<T> rootNode { get; set; }
        public WeakBinaryTreeNode<T> Add(WeakBinaryTreeNode<T> node, WeakBinaryTreeNode<T> currentNode = null)
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
                currentNode.Rewriting(node.GetValue()); //перезапись
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

        public async void Add(int key, T data)
        {
            Add(new WeakBinaryTreeNode<T>(key, data));
            await Task.Delay(time);
        }

        private WeakBinaryTreeNode<T> FindNode(int key, WeakBinaryTreeNode<T> startWithNode = null)
        {
            if (startWithNode == null)
            {
                startWithNode = rootNode;
            }

            if (((key.CompareTo(startWithNode.key)) == 0) && (startWithNode.CheckAvailability()))
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
            WeakBinaryTreeNode<T> node = FindNode(key);
            if (node != null)
            {
                return node.GetValue();
            }
            else
            {
                Debug.WriteLine($"nothing was found for this key = {key}");
                return (default);
            }
        }

        public void RemoveByKey(int key)
        {
            WeakBinaryTreeNode<T> node = FindNode(key);
            Remove(node, key);
        }

        private void Remove(WeakBinaryTreeNode<T> node, int key)
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
                        WeakBinaryTreeNode<T> auxiliaryLeft = node.leftNode;
                        WeakBinaryTreeNode<T> auxiliaryRightLeft = node.rightNode.leftNode;
                        WeakBinaryTreeNode<T> auxiliaryRightRight = node.rightNode.rightNode;
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
