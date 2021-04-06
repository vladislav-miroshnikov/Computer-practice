using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Commands
{
    public class CmdCommands : ICommand
    {
        public CmdCommands(List<string> arguments, string name)
        {
            Arguments = arguments;
            Name = name;
            Result = new List<string>();
        }

        public string Name { get; set; }
        public List<string> Arguments { get; set; }
        public bool IsFirstCommand { get; set; }
        public List<string> Result { get; set; }

        public void Execute()
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = Name;
                process.StartInfo.UseShellExecute = false;
                if (Arguments.Count > 0)
                {
                    string arg = Arguments[0];
                    if (Arguments.Count > 1)
                    {
                        for (int i = 1; i < Arguments.Count; i++)
                        {
                            arg += " " + Arguments[i];
                        }
                    }
                    process.StartInfo.Arguments = arg;
                }
                process.Start();
                Result.Add(process.StartInfo.Arguments);

            }
            catch
            {
                throw new Exception($"Failed to run this command {Name}");
            }
        }

        public bool IsCorrectArgs()
        {
            return true;
        }
    }
}
