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
            this.key = key;
            this.value = value;
        }

        public T value { get; set; }
        public int key { get; set; }
        public BinaryTreeNode<T> leftNode { get; set; }
        public BinaryTreeNode<T> rightNode { get; set; }
        public BinaryTreeNode<T> parentNode { get; set; }
      
        public Side? nodeSide  //nullable value type, determine the position
        {
            get
            {
                if (parentNode == null)
                {
                    return null;
                }
                else if(parentNode.leftNode == this)
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
