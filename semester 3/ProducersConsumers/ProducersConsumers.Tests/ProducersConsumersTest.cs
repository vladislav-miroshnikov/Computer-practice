using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProducersConsumers.Tests
{
    [TestClass]
    public class ProducersConsumersTest
    {
        [TestMethod]
        public void SoloMultithreadingTest()
        {
            List<int> list = new List<int>();
            Mutex mutex = new Mutex();
            Producer producer = new Producer("producer 1", mutex, list);
            Thread.Sleep(100);
            producer.Exit();
            Assert.AreEqual(1, list.Count);

            Consumer consumer = new Consumer("consumer 1", mutex, list);
            Thread.Sleep(100);
            consumer.Exit();
            Assert.AreEqual(0, list.Count);

        }

        [TestMethod]
        public void MultipleMultithreadingTest()
        {
            List<int> list = new List<int>();
            Mutex mutex = new Mutex();
            Producer[] producers = new Producer[3];
            Consumer[] consumers = new Consumer[3];
            for (int i = 0; i < producers.Length; i++)
            {
                producers[i] = new Producer($"producer {i + 1}", mutex, list);
                Thread.Sleep(100);
            }
            Thread.Sleep(100);
            for (int i = 0; i < producers.Length; i++)
            {
                producers[i].Exit();
            }
            Assert.AreEqual(3, list.Count);
            for (int i = 0; i < consumers.Length; i++)
            {
                consumers[i] = new Consumer($"consumer {i + 1}", mutex, list);
                Thread.Sleep(100);
            }
            Thread.Sleep(100);
            for (int i = 0; i < consumers.Length; i++)
            {
                consumers[i].Exit();
            }
            Assert.AreEqual(0, list.Count);            
        }

    }
}
