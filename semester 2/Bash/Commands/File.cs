using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Commands
{
    public class File : ICommand
    {

        public List<string> Arguments { get; set; }
        public bool IsFirstCommand { get; set; }
        public List<string> Result { get; set; }

        public File(List<string> arguments, bool isFirst)
        {
            Arguments = arguments;
            IsFirstCommand = isFirst;
            Result = new List<string>();
        }

        public void Execute()
        {
            try
            {
                string type = Path.GetExtension(Arguments[0]);
                Console.WriteLine(type);
                Result.Add(type);
            }
            catch(ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public bool IsCorrectArgs()
        {
            if (IsFirstCommand && Arguments.Count == 0)
            {
                throw new Exception("Incorrect file command args");
            }

            if (!IsFirstCommand && !Arguments.Any())
            {
                throw new Exception("Incorrect file command args");
            }
            return true;
        }
    }
}
