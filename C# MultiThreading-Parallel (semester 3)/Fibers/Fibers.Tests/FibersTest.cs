using FibersLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fibers.Tests
{
    [TestClass]
    public class FibersTest
    {
        private int numProc;

        [TestInitialize]
        public void ProcessesInit()
        {
            numProc = 3;
            for (int i = 0; i < numProc; i++)
            {
                Process process = new Process();
                ProcessManager.AddNewProcess(process);
            }
        }

        [TestMethod]
        public void TestWithoutPriorities()
        {
            Assert.AreEqual(numProc, ProcessManager.Fibers.Count);
            ProcessManager.Start(false);
            ProcessManager.Dispose();
            Assert.AreEqual(0, ProcessManager.Fibers.Count);
        }

        [TestMethod]
        public void TestWithPriorities()
        {
            Assert.AreEqual(numProc, ProcessManager.Fibers.Count);
            ProcessManager.Start(true);
            ProcessManager.Dispose();
            Assert.AreEqual(0, ProcessManager.Fibers.Count);
        }
    }
}
