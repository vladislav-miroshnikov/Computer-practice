using System;
using System.Threading.Tasks;
using ExamSystemLib.LockFree;
using ExamSystemLib.Coarse;
using ExamSystemLib;
using System.Diagnostics;

namespace ExamSystem
{
    class Program
    {
        static IExamSystem system;

        static Task[] tasks;
        static Random random = new Random();

        static void Main(string[] args)
        {
            var coarseTable = new CoarseHashTable(1024);
            var lockFreeTable = new LockFreeTable(1024);
            Stopwatch stopwatch = new Stopwatch();
            tasks = new Task[100000];
            Init();
            system = lockFreeTable;
            stopwatch.Start();
            StartTest();
            stopwatch.Stop();
            Console.WriteLine($"LockFree Table running time after 100000 requests: {stopwatch.ElapsedMilliseconds} ms");
            Init();
            system = coarseTable;
            stopwatch.Restart();
            StartTest();
            stopwatch.Stop();
            Console.WriteLine($"Coarse Table running time after 100000 requests: {stopwatch.ElapsedMilliseconds} ms");
            Console.ReadLine();
        }

        public static void Init()
        {

            for (int i = 0; i < tasks.Length; i++)
            {
                int request = random.Next(101);
                if (request == 0)
                {
                    tasks[i] = new Task(() => system.Remove(random.Next(), random.Next()));
                }
                else if (request > 10)
                {
                    tasks[i] = new Task(() => system.Contains(random.Next(), random.Next()));
                }
                else
                {
                    tasks[i] = new Task(() => system.Add(random.Next(), random.Next()));
                }
            }

        }

        public static void StartTest()
        {
            foreach (Task task in tasks)
            {
                task.Start();
            }
            foreach (Task task in tasks)
            {
                task.Wait();
            }

        }

    }
}
