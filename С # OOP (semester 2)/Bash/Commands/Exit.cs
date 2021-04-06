using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace Commands
{
    public class Exit : ICommand
    {
        public List<string> Arguments { get; set; }
        public bool IsFirstCommand { get; set; }
        public List<string> Result { get; set; }

        public Exit(List<string> arguments)
        {
            Arguments = arguments;
        }

        public void Execute()
        {
            try
            {
                Environment.Exit(0);
            }
            catch(SecurityException e)
            {
                throw e;
            }
        }

        public bool IsCorrectArgs()
        {
            if (Arguments.Any())
            {
                throw new Exception("Incorrect exit command args");
            }
            return true;
        }
    }
}
