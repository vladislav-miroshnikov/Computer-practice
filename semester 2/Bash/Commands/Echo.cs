using System;
using System.Collections.Generic;
using System.Linq;

namespace Commands
{
    public class Echo : ICommand
    {
        public List<string> Arguments { get; set; }
        public bool IsFirstCommand { get; set; }
        public List<string> Result { get; set; }

        public Echo(List<string> arguments, bool isFirst)
        {
            IsFirstCommand = isFirst;
            Arguments = arguments; 
            Result = new List<string>();
        }

        public void Execute()
        {
            foreach (string arg in Arguments)
            {
                Console.WriteLine(arg);
                Result.Add(arg);
            }
        }

        public bool IsCorrectArgs()
        {
            if (!IsFirstCommand && !Arguments.Any())
            {
                throw new Exception("Incorrect echo command args");
            }

            return true;
        }
    }
}
