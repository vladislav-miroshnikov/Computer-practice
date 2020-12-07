using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExamSystemLib.LockFree;
using ExamSystemLib.Coarse;
using System.Threading;
using ExamSystemLib;

namespace ExamSystem.Tests
{
    [TestClass]
    public class SetsTest
    {
        Random random = new Random();
        object locker = new object();

        [TestMethod]
        public void LockFreeTableTest()
        {
            LockFreeTable lockFreeTable = new LockFreeTable(512);

            Thread[] threads = new Thread[25];
            for (int i = 0; i < threads.Length; i++) 
            { 
                threads[i] = new Thread(new ParameterizedThreadStart(MakeRequest));
            }
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Start(lockFreeTable);
            }
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }
        }

        private void MakeRequest(object param)
        {
            IExamSystem system = (IExamSystem)param;

            for (int i = 0; i < 1000; i++)
            {          
                int studentId = random.Next();
                int courseId = random.Next();
                Monitor.Enter(locker);
                system.Add(studentId, courseId);
                Assert.IsTrue(system.Contains(studentId, courseId));
                system.Remove(studentId, courseId);
                Assert.IsTrue(!system.Contains(studentId, courseId));
                Monitor.Exit(locker);
            }
        }

        [TestMethod]
        public void CoarseTableTest()
        {
            CoarseHashTable coarseHashTable = new CoarseHashTable(512);
            Thread[] threads = new Thread[25];
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(MakeRequest));

            }
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Start(coarseHashTable);
            }
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }
        }

    }
}
