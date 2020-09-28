namespace BinaryTree
{
    public enum Side
    {
        Left,
        Right
    }

    public class BinaryTreeNode<T>
    {
        public BinaryTreeNode(int key, T value)
        {
            Key = key;
            Value = value;
        }

        public T Value { get; set; }
        public int Key { get; set; }
        public BinaryTreeNode<T> LeftNode { get; set; }
        public BinaryTreeNode<T> RightNode { get; set; }
        public BinaryTreeNode<T> ParentNode { get; set; }
      
        public Side? nodeSide  //nullable value type, determine the position
        {
            get
            {
                if (ParentNode == null)
                {
                    return null;
                }
                else if(ParentNode.LeftNode == this)
                {
                    return Side.Left;
                }
                else
                {
                    return Side.Right;
                }
            }
        }

    }
}
