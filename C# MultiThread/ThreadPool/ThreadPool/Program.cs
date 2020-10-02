using System;
using System.Threading;

namespace ThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            Action action = PrintHello;
            Console.WriteLine("Enter a number of threads in your ThreadPool:");
            ThreadPoolLib.ThreadPool threadPool = new ThreadPoolLib.ThreadPool(GetNumber());
            for (int i = 0; i < 20; i++) 
            {
                threadPool.Enqueue(action);
                Thread.Sleep(10);
            }
            threadPool.Dispose();
            Console.WriteLine("\nProgram ended");
            Console.ReadKey();
        }

        static void PrintHello()
        {
            Console.Write("Hello, ThreadPool works good!!!\n");
        }

        static int GetNumber()
        {
            int result;
            while (true)
            {
                bool a = int.TryParse(Console.ReadLine(), out result);

                if (!a)
                {
                    Console.WriteLine("You entered not a number, try again");
                }
                else if (result != 0 && result < 129)
                {
                    return result;
                }
                else
                {
                    Console.WriteLine("Program logic not provided 0 or you entered too large a number , try again");
                }
            }
        }
    }
}
