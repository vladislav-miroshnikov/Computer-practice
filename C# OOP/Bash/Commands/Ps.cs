using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Commands
{
    public class Ps : ICommand
    {
        public List<string> Arguments { get; set; }
        public bool IsFirstCommand { get; set; }
        public List<string> Result { get; set; }

        [DllImport("user32.dll")] //for get active system processes, IsIconic is the entry point to user32.dll
        private static extern bool IsIconic(IntPtr arg);

        public Ps(List<string> arguments, bool isFirst)
        {
            Arguments = arguments;
            IsFirstCommand = isFirst;
            Result = new List<string>();
        }

        public void Execute()
        {
            Process[] visibleProcesses = Process.GetProcesses().Where(p => p.MainWindowHandle != IntPtr.Zero 
            && !IsIconic(p.MainWindowHandle)).ToArray();
            foreach (Process temp in visibleProcesses)
            {
                Console.WriteLine($"Process name: {temp.ProcessName}");
                Console.WriteLine($"Process Id: {temp.Id}");
                Console.WriteLine($"Process StartTime: {temp.StartTime}");
                Console.WriteLine($"Process VirtualMemory 64: {temp.VirtualMemorySize64}");
                Console.WriteLine();
            }
        }

        public bool IsCorrectArgs()
        {
            if (Arguments.Any())
            {
                throw new Exception("Incorrect ps command args");
            }

            return true;
        }
    }
}
