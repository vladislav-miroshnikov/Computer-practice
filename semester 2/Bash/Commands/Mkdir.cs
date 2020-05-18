using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Commands
{
    public class Mkdir : ICommand
    {
        public List<string> Arguments { get; set; }
        public bool IsFirstCommand { get; set; }
        public List<string> Result { get; set; }

        public Mkdir(List<string> arguments, bool isFirst)
        {
            Arguments = ArgumentsConverter(arguments);
            IsFirstCommand = isFirst;
            Result = new List<string>();
        }

        private List<string> ArgumentsConverter(List<string> input)
        {
            List<string> newArguments = new List<string>();
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < input.Count; i++)
            {
                stringBuilder.Insert(stringBuilder.Length, input[i]);

                if (i < input.Count - 1 && input[i + 1].Contains(@":\") == true)
                {
                    newArguments.Add(stringBuilder.ToString());
                    stringBuilder.Remove(0, stringBuilder.Length);
                    continue;
                }

                if (i != input.Count - 1)
                {
                    stringBuilder = stringBuilder.Insert(stringBuilder.Length, " ");
                }

                if (i == input.Count - 1)
                {
                    newArguments.Add(stringBuilder.ToString());
                }
            }

            return (newArguments);
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
