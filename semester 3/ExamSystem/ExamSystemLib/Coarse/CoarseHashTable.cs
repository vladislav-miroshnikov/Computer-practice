using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ExamSystemLib.Coarse
{
    public class CoarseHashTable : IExamSystem
    {
        public List<CoarseElement>[] Table { get; private set; }
        private int tableCount;
        private int tableSize;
        private Mutex coarseLock; 

        public CoarseHashTable(int size)
        {
            tableCount = 0;
            tableSize = CheckTableSize(size);
            Table = new List<CoarseElement>[tableSize];
            for (int i = 0; i < tableSize; i++)
            {
                Table[i] = new List<CoarseElement>();
            }
            coarseLock = new Mutex();
        }

        private int CheckTableSize(int size)
        {
            if (size < 512)
            {
                size = 512;
                return size;
            }
            return size;
        }

        private bool PolicyDemandsResize
        {
            get
            {
                return tableCount / Table.Length > 4;
            }
        }

        private void Acquire()
        {
            coarseLock.WaitOne();
        }

        private void Release()
        {
            coarseLock.ReleaseMutex();
        }

        public void Add(long studentId, long courseId)
        {           
            Acquire();
            CoarseElement newElement = new CoarseElement(studentId, courseId);
            try
            {
                int tableIndex = Hash(studentId);
                if (!Table[tableIndex].Any(x => x.StudentId == studentId && x.CourseId == courseId))
                {
                    Table[tableIndex].Add(newElement);
                    tableCount++;
                }
            }
            finally
            {
                Release();
            }
            if (PolicyDemandsResize)
                Resize();
        }

        public void Remove(long studentId, long courseId)
        {
            Acquire();
            try
            {
                int tableIndex = Hash(studentId);
                if (Table[tableIndex].Any(x => x.StudentId == studentId && x.CourseId == courseId)) 
                {
                    CoarseElement removingElement = Table[tableIndex].Find(x => x.StudentId == studentId && x.CourseId == courseId);
                    Table[tableIndex].Remove(removingElement);
                    tableCount--;
                }
            }
            finally
            {
                Release();
            }
        }

        public bool Contains(long studentId, long courseId)
        {
            Acquire();
            try
            {
                int tableIndex = Hash(studentId);
                return Table[tableIndex].Any(x => x.StudentId == studentId && x.CourseId == courseId);
            }
            finally
            {
                Release();
            }
        }

        private int Hash(long studentId)
        {
            return (int)studentId.GetHashCode() % tableSize; 
        }

        private void Resize()
        {
            int oldCapacity = Table.Length;
            Acquire();
            try
            {
                if (oldCapacity != Table.Length)
                {
                    return; 
                }
                int newCapacity = 2 * oldCapacity;
                List<CoarseElement>[] oldTable = Table;
                Table = new List<CoarseElement>[newCapacity];
                for (int i = 0; i < newCapacity; i++)
                    Table[i] = new List<CoarseElement>();
                foreach (List<CoarseElement> list in oldTable)
                {
                    foreach (CoarseElement el in list)
                    {
                        Table[Hash(el.StudentId)].Add(el);
                    }
                }
            }
            finally
            {
                Release();
            }
        }
    }
}
