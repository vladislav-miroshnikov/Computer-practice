using System;
using System.Collections.Generic;
using System.Threading;

namespace ProducersConsumers
{
    public class Producer
    {
        private Mutex mutex;
        private string name;
        private static Random random = new Random();
        public Thread ThreadProducer { get; private set; }
        private volatile bool isExit = false;
        public Producer(string name, Mutex mutex, List<int> list)
        {
            this.name = name;
            this.mutex = mutex;
            ThreadProducer = new Thread(new ParameterizedThreadStart(Put));
            ThreadProducer.Name = this.name;
            ThreadProducer.Start(list);
        }

        private void Put(object obj)
        {
            List<int> list = (List<int>)obj;
           
            while(!isExit)
            {
                mutex.WaitOne();
                Console.WriteLine($"{this.name} put element");
                list.Add(random.Next(1, 150));
                
                mutex.ReleaseMutex();
                Thread.Sleep(700);
            }
            
        }

        public void Exit()
        {
            isExit = true;
        }

    }
}
