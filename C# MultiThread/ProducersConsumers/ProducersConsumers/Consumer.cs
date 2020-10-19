using System;
using System.Collections.Generic;
using System.Threading;

namespace ProducersConsumers
{
    public class Consumer
    {
        private Mutex mutex;
        private string name;
        private static Random random = new Random();
        private Thread ThreadConsumer;
        private volatile bool isExit = false;
        public Consumer(string name, Mutex mutex, List<int> list)
        {
            this.name = name;
            this.mutex = mutex;
            ThreadConsumer = new Thread(new ParameterizedThreadStart(Take));
            ThreadConsumer.Name = this.name;
            ThreadConsumer.Start(list);

        }

        private void Take(object obj)
        {
            List<int> list = (List<int>)obj;
            
            while(!isExit)
            {
                mutex.WaitOne();
                if (list.Count != 0)
                {
                    Console.WriteLine($"{this.name} took element");
                    list.RemoveAt(random.Next(0, list.Count));
                }
                
                mutex.ReleaseMutex();
                Thread.Sleep(700);
            }         
            
        }

        public void Exit()
        {
            isExit = true;
        }

        public void Join()
        {
            ThreadConsumer.Join();
        }

    }
}
