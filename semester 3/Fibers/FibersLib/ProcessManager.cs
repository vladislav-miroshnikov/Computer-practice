using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FibersLib
{
    public static class ProcessManager
    {
        private static SheduleType sheduleType;
        public static Dictionary<Fiber, Process> Fibers { get; private set; }
        private static Fiber currentFiber;
        private static Random random;
        private static bool isStarted;
        private static List<Fiber> removingFibers;
        private static int windowSize;

        static ProcessManager()
        {
            Fibers = new Dictionary<Fiber, Process>();
            random = new Random();
            sheduleType = SheduleType.NonPriority;
            isStarted = false;
            removingFibers = new List<Fiber>();
        }

        public static void AddNewProcess(Process process)
        {
            Fibers.Add(new Fiber(process.Run), process);
        }

        public static void Start(bool isPriorityEnable)
        {
            if(!isStarted && Fibers.Count != 0)
            {
                if (isPriorityEnable)
                {
                    sheduleType = SheduleType.Priority;
                    windowSize = PriorityInit();
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

        private static int PriorityInit()
        {
            int maxPriority = Fibers.Values.OrderByDescending(x => x.Priority).First().Priority;
            int highPriorityClassCount = 0; //we make a high priority class, average and low, choose a high
            int bottomBorder = maxPriority / 3 * 2 + 1; 
            if(bottomBorder > maxPriority)
            {
                bottomBorder = maxPriority;
            }
            foreach (KeyValuePair<Fiber, Process> pair in Fibers) 
            {
                if (pair.Value.Priority >= bottomBorder)
                {
                    highPriorityClassCount++;
                }
            }

            return Fibers.Count / highPriorityClassCount;
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
                        Process curProcess = null;
                        Fibers.TryGetValue(currentFiber, out curProcess);
                        Fibers.Remove(currentFiber);
                        nextFiber = Fibers.ElementAt(random.Next(Fibers.Count)).Key;
                        Fibers.Add(currentFiber, curProcess);
                    }
                    else
                    {
                        nextFiber = currentFiber; 
                    }
                    break;

                case SheduleType.Priority:
                    
                    var highSliceDict = Fibers.OrderByDescending(x => x.Value.Priority).Take(windowSize);
                    var workingFibers = highSliceDict.Where((x) => x.Key != currentFiber && x.Value.ProcessFlag == true);
                    if (workingFibers.Count() == 0)
                    {
                        if(highSliceDict.Count() == 1)
                        {
                            nextFiber = highSliceDict.First().Key;
                        }
                        else
                        {
                            nextFiber = highSliceDict.Where((x) => x.Key != currentFiber).First().Key;
                        }
                     
                    }
                    else
                    {
                        nextFiber = workingFibers.First().Key;
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
