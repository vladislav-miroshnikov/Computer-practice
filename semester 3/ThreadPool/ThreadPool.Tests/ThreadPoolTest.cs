using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThreadPool.Tests
{
    [TestClass]
    public class ThreadPoolTest
    {
        [TestMethod]
        public void ThreadPoolNumberTest()
        {
            int[] vs = new int[50];
            ThreadPoolLib.ThreadPool threadPool = new ThreadPoolLib.ThreadPool(34);
            for (int i = 0; i < 50; i++)
            {
                threadPool.Enqueue(() => vs[i] = i >> 4);
                Thread.Sleep(10);
            }
            Assert.AreEqual(34, threadPool.threads.Count);
            for (int i = 0; i < 50; i++)
            {
                Assert.AreEqual(i >> 4, vs[i]);
            }
            Thread.Sleep(10);
            threadPool.Dispose();
            Assert.AreEqual(0, threadPool.threads.Count);
        }
    }
}
