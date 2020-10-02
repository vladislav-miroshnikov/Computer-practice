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
        private bool disposed = false;

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
                threads.Add(new Thread(new ThreadStart(RunTask)));
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

        private void RunTask()
        {
            while(!isExit)
            {
                Monitor.Enter(actions);
                if (actions.Count != 0 && !isExit)
                {
                    Action action = actions.Dequeue();
                    Monitor.Exit(actions);
                    Console.Write($"\nThread {Thread.CurrentThread.Name} done task : ");
                    action?.Invoke();
                    Thread.Sleep(10);          
                }
                else if (actions.Count == 0 && !isExit)
                {
                    Monitor.Wait(actions);
                    Monitor.Exit(actions);                    
                }
                else if (isExit)
                {
                    Monitor.Exit(actions);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //according to Microsoft Docs

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            //unmanaged behavior because the thread may eventually fail to exit
            isExit = true;

            lock (actions)
            {
                Monitor.PulseAll(actions);
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            if (disposing)
            {
                threads.Clear();
                actions.Clear();
            }

            disposed = true;
        }

        ~ThreadPool() => Dispose(false);    
    }
}
