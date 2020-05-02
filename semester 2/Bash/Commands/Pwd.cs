using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Commands
{
    public class Pwd : ICommand
    {
        public List<string> Arguments { get; set; }
        public bool IsFirstCommand { get; set; }
        public List<string> Result { get; set; }

        public Pwd(List<string> arguments, bool isFirst)
        {
            Arguments = arguments;
            IsFirstCommand = isFirst;
            Result = new List<string>();
        }

        public void Execute()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine(currentDirectory);
            string[] files = Directory.GetFiles(currentDirectory);
            foreach (string temp in files)
            {
                Console.WriteLine(temp);
                Result.Add(temp);
            }

        }

        public bool IsCorrectArgs()
        {
            if (Arguments.Any())
            {
                throw new Exception("Incorrect pwd command args");
            }
            return true;
        }
    }
}
