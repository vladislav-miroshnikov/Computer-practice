using System;
using System.Collections.Generic;
using System.Threading;

namespace ProducersConsumers
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the producer-consumer program " +
                "To end the program after the start at any time, press any button");
            List<int> list = new List<int>();
            Mutex mutex = new Mutex();
            Console.WriteLine("Enter a number of producers:");
            int numberOfProducers = GetNumber();
            Console.WriteLine("Enter a number of consumers:");
            int numberOfConsumers = GetNumber();
            Producer[] producers = new Producer[numberOfProducers];
            Consumer[] consumers = new Consumer[numberOfConsumers];
            for(int i = 0; i < producers.Length; i++)
            {
                producers[i] = new Producer($"producer {i + 1}", mutex, list);
            }
            for (int i = 0; i < consumers.Length; i++)
            {
                consumers[i] = new Consumer($"consumer {i + 1}", mutex, list);
            }
            Console.ReadKey();
            for (int i = 0; i < producers.Length; i++)
            {
                producers[i].Exit();
            }
            for (int i = 0; i < consumers.Length; i++)
            {
                consumers[i].Exit();
            }
            Console.WriteLine("\nProgram ended");
            Console.ReadLine();
        }

        static int GetNumber()
        {
            int result;
            while(true)
            {
                bool a = int.TryParse(Console.ReadLine(), out result);

                if (!a)
                {
                    Console.WriteLine("You entered not a number, try again");
                }
                else if(result != 0 && result < 65)
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
