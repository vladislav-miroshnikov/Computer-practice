using System;
using FibersLib;
using Process = FibersLib.Process;

namespace Fibers
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 8; i++)
            {
                Process process = new Process();
                ProcessManager.AddNewProcess(process);
            }
            ProcessManager.Start(true);
            ProcessManager.Dispose();
            Console.WriteLine("Program ended");
            Console.ReadKey();

        }
    }
}
