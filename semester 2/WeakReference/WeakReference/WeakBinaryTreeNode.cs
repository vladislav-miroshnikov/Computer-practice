using System;

namespace WeakReference
{
    public enum Side
    {
        Left,
        Right
    }

    public class WeakBinaryTreeNode<T> where T : class
    {
        public WeakBinaryTreeNode(int key, T value)
        {
            Key = key;
            Value = new WeakReference<T>(value);
        }

        public bool CheckAvailability()
        {
            return Value.TryGetTarget(out T target);
        }

        public T GetValue()
        {
            Value.TryGetTarget(out T target);
            return target;
        }

        public void Rewriting(T newValue)
        {
            Value.SetTarget(newValue);
        }

        public WeakReference<T> Value { get; set; }
        public int Key { get; set; }
        public WeakBinaryTreeNode<T> LeftNode { get; set; }
        public WeakBinaryTreeNode<T> RightNode { get; set; }
        public WeakBinaryTreeNode<T> ParentNode { get; set; }

        public Side? nodeSide  //nullable value type, determine the position
        {
            get
            {
                if (ParentNode == null)
                {
                    return null;
                }
                else if (ParentNode.LeftNode == this)
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
