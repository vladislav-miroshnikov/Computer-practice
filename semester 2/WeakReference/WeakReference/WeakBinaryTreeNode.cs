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
            this.key = key;
            this.value = new WeakReference<T>(value);
        }

        public bool CheckAvailability()
        {
            return value.TryGetTarget(out T target);
        }

        public T GetValue()
        {
            value.TryGetTarget(out T target);
            return target;
        }

        public void Rewriting(T newValue)
        {
            this.value.SetTarget(newValue);
        }

        public WeakReference<T> value { get; set; }
        public int key { get; set; }
        public WeakBinaryTreeNode<T> leftNode { get; set; }
        public WeakBinaryTreeNode<T> rightNode { get; set; }
        public WeakBinaryTreeNode<T> parentNode { get; set; }

        public Side? nodeSide  //nullable value type
        {
            get
            {
                if (parentNode == null)
                {
                    return null;
                }
                else if (parentNode.leftNode == this)
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
