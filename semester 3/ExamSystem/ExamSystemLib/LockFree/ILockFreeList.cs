namespace ExamSystemLib.LockFree
{
    internal interface ILockFreeList
    {
        void Add(LockFreeNode item);
        bool Contains(LockFreeNode item);
        void Remove(LockFreeNode item);

    }
}
