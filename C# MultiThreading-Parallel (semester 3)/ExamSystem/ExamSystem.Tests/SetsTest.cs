using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExamSystemLib.LockFree;
using ExamSystemLib.Coarse;
using System.Threading;
using ExamSystemLib;
using System.Threading.Tasks;

namespace ExamSystem.Tests
{
    [TestClass]
    public class SetsTest
    {

        [TestMethod]
        public void LockFreeTableTest()
        {
            LockFreeTable lockFreeTable = new LockFreeTable(512);
            Task[] threads = new Task[25];
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Task(() => MakeRequest(lockFreeTable));
            }
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Start();
            }
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Wait();
                threads[i].Dispose();
            }
        }

        private void MakeRequest(object param)
        {
            IExamSystem system = (IExamSystem)param;
            for (int i = 0; i < 1000; i++)
            {
                int studentId = Math.Abs(Guid.NewGuid().GetHashCode()) * Thread.CurrentThread.ManagedThreadId;
                int courseId = Math.Abs(Guid.NewGuid().GetHashCode());
                system.Add(studentId, courseId);
                Assert.IsTrue(system.Contains(studentId, courseId));
                system.Remove(studentId, courseId);
                Assert.IsTrue(!system.Contains(studentId, courseId));
            }
        }

        [TestMethod]
        public void CoarseTableTest()
        {
            CoarseHashTable coarseHashTable = new CoarseHashTable(512);
            Task[] threads = new Task[25];
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Task(() => MakeRequest(coarseHashTable));
            }
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Start();
            }
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Wait();
                threads[i].Dispose();
            }
        }

    }
}
