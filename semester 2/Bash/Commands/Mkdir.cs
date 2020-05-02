using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Commands
{
    public class Mkdir : ICommand
    {
        public List<string> Arguments { get; set; }
        public bool IsFirstCommand { get; set; }
        public List<string> Result { get; set; }

        public Mkdir(List<string> arguments, bool isFirst)
        {
            Arguments = arguments;
            IsFirstCommand = isFirst;
            Result = new List<string>();
        }

        public void Execute()
        {
            if (Directory.Exists(Arguments[0]))
            {
                Console.WriteLine("That path exists already.");
            }
            else
            {
                Directory.CreateDirectory(Arguments[0]);
                Result.Add(Arguments[0]);
            }        
        }

        public bool IsCorrectArgs()
        {
            if (IsFirstCommand && Arguments.Count != 1)
            {
                throw new Exception("Incorrect mkdir command args");
            }

            if (!IsFirstCommand && !Arguments.Any())
            {
                throw new Exception("Incorrect mkdir command args");
            }
            return true;
        }
    }
}
