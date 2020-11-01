using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FibersLib
{
    public static class ProcessManager
    {
        private static SheduleType sheduleType;
        public static Dictionary<Fiber, int> Fibers { get; private set; }
        private static Fiber currentFiber;
        private static Random random;
        private static bool isStarted;
        private static List<Fiber> removingFibers;

        static ProcessManager()
        {
            Fibers = new Dictionary<Fiber, int>();
            random = new Random();
            sheduleType = SheduleType.NonPriority;
            isStarted = false;
            removingFibers = new List<Fiber>();
        }

        public static void AddNewProcess(Process process)
        {
            Fibers.Add(new Fiber(process.Run), process.Priority);
        }

        public static void Start(bool isPriorityEnable)
        {
            if(!isStarted && Fibers.Count != 0)
            {
                if (isPriorityEnable)
                {
                    sheduleType = SheduleType.Priority;
                }

                isStarted = true;
                currentFiber = Fibers.First().Key;
                Switch(false);
            }
            else
            {
                if(isStarted)
                {
                    Console.WriteLine("The start has already been completed!");
                }
                else
                {
                    Console.WriteLine("Incorrect start! You have not added any processes.");
                }
            }
        }

        public static void Switch(bool fiberFinished)
        {
            if(fiberFinished)
            {
                Fibers.Remove(currentFiber);

                if (!currentFiber.IsPrimary)
                {
                    removingFibers.Add(currentFiber);
                }
                Console.WriteLine($"Fiber [{currentFiber.Id}] finished");

                if (Fibers.Count == 0)
                {
                    Console.WriteLine($"Primary fiber [{Fiber.PrimaryId}] finished");
                    Thread.Sleep(1);
                    Fiber.Switch(Fiber.PrimaryId);
                    return;
                }
                currentFiber = Fibers.First().Key;

            }
            currentFiber = ChooseNextFiber();
            Thread.Sleep(1);
            Fiber.Switch(currentFiber.Id);
        }

        private static Fiber ChooseNextFiber()
        {
            Fiber nextFiber = null;
            switch (sheduleType)
            {
                case SheduleType.NonPriority:
                    
                    if(Fibers.Count > 1)
                    {
                        int id = 0;
                        Fibers.TryGetValue(currentFiber, out id);
                        Fibers.Remove(currentFiber);
                        nextFiber = Fibers.ElementAt(random.Next(Fibers.Count)).Key;
                        Fibers.Add(currentFiber, id);
                    }
                    else
                    {
                        nextFiber = currentFiber; 
                    }
                    break;

                case SheduleType.Priority:

                    nextFiber = Fibers.OrderByDescending(x => x.Value).First().Key;
                    if(Fibers.Count > 1)
                    {
                        for (int i = 0; i < Fibers.Count; i++)
                        {
                            if (Fibers.ElementAt(i).Key != nextFiber)
                            {
                                Fibers[Fibers.ElementAt(i).Key] = Fibers.ElementAt(i).Value + 1;
                            }
                        }
                        Fibers[nextFiber] = 0;
                    }

                    break;

            }

            return nextFiber;
        }

        public static void Dispose()
        {
            Fibers.Clear();
            foreach(Fiber fiber in removingFibers)
            {
                Thread.Sleep(1);
                fiber.Delete();
            }
            removingFibers.Clear();
            sheduleType = SheduleType.NonPriority;
            currentFiber = null;
            isStarted = false;
            Console.WriteLine("Dispose successful!");   
        }

    }
}
