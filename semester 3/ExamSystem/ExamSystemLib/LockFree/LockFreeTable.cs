namespace ExamSystemLib.LockFree
{
    public class LockFreeTable : IExamSystem
    {
        private int tableSize;
        public LockFreeList[] Table { get; private set; }

        public LockFreeTable(int size)
        {
            tableSize = CheckTableSize(size);
            Table = new LockFreeList[tableSize];
            for (int i = 0; i < size; i++) 
            {
                Table[i] = new LockFreeList();
            }

        }

        private int CheckTableSize(int size)
        {
            if(size < 512)
            {
                size = 512;
                return size;
            }
            return size;
        }

        private int Hash(long studentId)
        {
            return (int)studentId.GetHashCode() % tableSize; 
        }

        public void Add(long studentId, long courseId)
        {
            Table[Hash(studentId)].Add(new LockFreeNode(studentId, courseId));
        }

        public void Remove(long studentId, long courseId)
        {
            Table[Hash(studentId)].Remove(new LockFreeNode(studentId, courseId));
        }

        public bool Contains(long studentId, long courseId)
        {
            return Table[Hash(studentId)].Contains(new LockFreeNode(studentId, courseId));
        }

    }
}
