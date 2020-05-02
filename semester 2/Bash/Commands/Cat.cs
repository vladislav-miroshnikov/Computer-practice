using System;
using System.Collections.Generic;
using System.Linq;

namespace Commands
{
    public class Cat : ICommand
    {
        public List<string> Arguments { get ; set; }
        public bool IsFirstCommand { get ; set; }
        public List<string> Result { get; set; }

        public Cat(List<string> arguments, bool isFirst)
        {
            Arguments = arguments;
            IsFirstCommand = isFirst;
            Result = new List<string>();
        }
        
        public void Execute()
        {
            string[] lines;
            try
            {
                lines = System.IO.File.ReadAllLines(Arguments[0]);
            }
            catch
            {
                throw new ArgumentException("Incorrect path to file");
            }
            foreach (string temp in lines)
            {
                Console.WriteLine(temp);
            }
            Console.WriteLine();
            Result.AddRange(lines);
        }

        public bool IsCorrectArgs()
        {
            if (!IsFirstCommand && !Arguments.Any())
            {
                throw new Exception("Incorrect cat command args");
            }

            if (IsFirstCommand && Arguments.Count != 1)
            {
                throw new Exception("Incorrect cat command args");
            }

            return true;
        }
    }
}
