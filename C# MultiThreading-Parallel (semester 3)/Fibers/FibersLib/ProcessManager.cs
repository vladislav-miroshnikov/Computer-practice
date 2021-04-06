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
            /*We distinguish 3 abstract priority classes: low, medium, high.
            The next step is to compute the lower bound for the high priority abstact class in terms 
            of real priorities.Then count the number of processes in this high priority abstract class 
            and calculate the ratio of high priorities to other classes*/
            int maxPriority = Fibers.Values.OrderByDescending(x => x.Priority).First().Priority;
            int highPriorityClassCount = 0; 
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
                            if (highSliceDict.Count() == Fibers.Count) //the case when there is nothing outside the window
                            {
                                nextFiber = highSliceDict.Where((x) => x.Key != currentFiber).First().Key;
                            }
                            else
                            {
                                Random random = new Random();
                                int type = random.Next(0, 2); //choose how we will switch: outside the window or within the window
                                if (type == 0)
                                {
                                    nextFiber = highSliceDict.Where((x) => x.Key != currentFiber).First().Key;
                                }
                                else
                                {
                                    //we take the elements outside our window
                                    var behindWindow = Fibers.OrderByDescending(x => x.Value.Priority).Where(s => s.Value.Priority < highSliceDict.ElementAt(highSliceDict.Count() - 1).Value.Priority);
                                    int sum = 0;
                                    for (int i = 0; i < behindWindow.Count(); i++)
                                    {
                                        sum += behindWindow.ElementAt(i).Value.Priority;
                                    }
                                    if (sum == 0)
                                    {
                                        //case if everything outside the window with priorities 0: choose any
                                        nextFiber = behindWindow.ElementAt(random.Next(windowSize, behindWindow.Count())).Key;
                                    }
                                    else
                                    {
                                        //Here we create a probability distribution: our fibers are ordered in descending order of priorities, 
                                        //among them some will be chosen, but the higher the priority of the fiber,
                                        //the higher the probability of its selection

                                        //Thus, from time to time there will be switches to fibers outside our window
                                        int index = 0; 
                                        int checkSum = 0;
                                        int choice = random.Next(0, sum);
                                        while (index != behindWindow.Count())
                                        {
                                            checkSum += behindWindow.ElementAt(index).Value.Priority;
                                            if (checkSum > choice)
                                            {
                                                nextFiber = behindWindow.ElementAt(index).Key;
                                                Console.WriteLine("Switch behind the window");
                                                break;
                                            }
                                            index++;
                                        }
                                    }
                                }
                            }
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
