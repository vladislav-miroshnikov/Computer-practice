using System;
using System.Collections.Generic;
using System.Linq;

namespace Commands
{
    public class Mv : ICommand
    {
        public List<string> Arguments { get; set; }
        public bool IsFirstCommand { get; set; }
        public List<string> Result { get; set; }

        public Mv(List<string> arguments, bool isFirst)
        {
            Arguments = arguments;
            IsFirstCommand = isFirst;
            Result = new List<string>();
        }

        public void Execute()
        {
            try
            { 
                System.IO.File.Move(Arguments[0], Arguments[1]);
                Result.Add(Arguments[1]);
            }
            catch(ArgumentException e)
            {
                throw e;
            }
            
        }

        public bool IsCorrectArgs()
        {
            if (IsFirstCommand && Arguments.Count != 2)
            {
                throw new Exception("Incorrect mv command args");
            }

            if (!IsFirstCommand && !Arguments.Any())
            {
                throw new Exception("Incorrect mv command args");
            }

            return true;
        }
    }
}
