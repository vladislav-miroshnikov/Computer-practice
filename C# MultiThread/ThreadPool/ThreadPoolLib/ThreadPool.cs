using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadPoolLib
{
    public class ThreadPool : IDisposable
    {
        private readonly int numberOfThreads;
        public List<Thread> threads { get; private set; }
        private Queue<Action> actions = new Queue<Action>();
        private volatile bool isExit = false;

        public ThreadPool(int number)
        {
            numberOfThreads = number;
            threads = new List<Thread>();
            Initialize();
        }

        private void Initialize()
        {
            for(int i = 0; i < numberOfThreads; i++)
            {
                threads.Add(new Thread(new ThreadStart(Exec)));
                threads[i].Name = $"{i}";
                threads[i].IsBackground = true;
                threads[i].Start();
            }
        }

        public void Enqueue(Action a)
        {
            lock(actions)
            {
                actions.Enqueue(a);
                Monitor.PulseAll(actions);
            }

        }

        
        private void Exec()
        {
            while(!isExit)
            {
                Monitor.Enter(actions);
                if (actions.Count != 0)
                {
                    Action action = actions.Dequeue();
                    Monitor.Exit(actions);
                    Console.Write($"\nThread {Thread.CurrentThread.Name} done task : ");
                    action?.Invoke();
                    Thread.Sleep(10);          
                }
                else
                { 
                    Monitor.Exit(actions);                    
                }
            }
        }

        public void Dispose()
        {
            isExit = true;
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            threads.Clear();
            actions = null;
        }
    }
}
