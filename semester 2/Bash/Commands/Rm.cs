using System;
using System.Collections.Generic;
using System.Linq;

namespace Commands
{
    public class Rm : ICommand
    {
        public List<string> Arguments { get; set; }
        public bool IsFirstCommand { get; set; }
        public List<string> Result { get; set; }

        public Rm(List<string> arguments, bool isFirst)
        {
            Arguments = arguments;
            IsFirstCommand = isFirst;
            Result = new List<string>();
        }

        public void Execute()
        {
            try
            {
                System.IO.File.Delete(Arguments[0]); //exist
                Result.Add(Arguments[0]);
            }
            catch(ArgumentException e)
            {
                throw e;
            }
            
        }

        public bool IsCorrectArgs()
        {
            if (IsFirstCommand && Arguments.Count != 1)
            {
                throw new Exception("Incorrect rm command args");
            }

            if (!IsFirstCommand && !Arguments.Any())
            {
                throw new Exception("Incorrect rm command args");
            }
            return true;
        }
    }
}
