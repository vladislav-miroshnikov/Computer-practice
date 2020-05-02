using System;
using System.Collections.Generic;
using System.Linq;

namespace Commands
{
    public class Cp : ICommand
    {
        public List<string> Arguments { get; set; }
        public bool IsFirstCommand { get; set; }
        public List<string> Result { get; set; }

        public Cp(List<string> arguments, bool isFirst)
        {
            Arguments = arguments;
            IsFirstCommand = isFirst;
            Result = new List<string>();
        }

        public void Execute()
        {
            try //прочиать оба файла и проверить что содержимое одинаковое
            {
                System.IO.File.Copy(Arguments[0], Arguments[1]);
                Result.Add(Arguments[1]);
            }
            catch(ArgumentException)
            {
                throw new ArgumentException("Copy error");
            }
           
        }

        public bool IsCorrectArgs()
        {
            if (IsFirstCommand && Arguments.Count != 2)
            {
                throw new Exception("Incorrect cp command args");
            }

            if (!IsFirstCommand && !Arguments.Any())
            {
                throw new Exception("Incorrect cp command args");
            }
            return true;
        }
    }
}
