using System;
using System.Collections.Generic;
using System.Linq;

namespace Commands
{
    public class Man : ICommand
    {
        public Man(List<string> arguments, bool isFirst)
        {
            Arguments = arguments;
            IsFirstCommand = isFirst;
            Result = new List<string>();
        }

        public List<string> Arguments { get; set; }
        public bool IsFirstCommand { get; set; }
        public List<string> Result { get; set; }
        public void Execute()
        {
            switch(Arguments[0].ToLower())
            {
                case "cat":
                    {
                        Console.WriteLine("cat [FILENAME] - show the contents of a file on the screen\n");
                        Result.Add("cat [FILENAME] - show the contents of a file on the screen\n");
                        break;
                    }
                case "cp":
                    {
                        Console.WriteLine("cp [FILE1] [FILE2] - copy file1 to file2\n");
                        Result.Add("cp [FILE1] [FILE2] - copy file1 to file2\n");
                        break;
                    }
                case "echo":
                    {
                        Console.WriteLine("echo - display argument(s)\n");
                        Result.Add("echo - display argument(s)\n");
                        break;
                    }
                case "exit":
                    {
                        Console.WriteLine("exit - exit interpreter\n");
                        Result.Add("exit - exit interpreter\n");
                        break;
                    }
                case "file":
                    {
                        Console.WriteLine("file [FILENAME]  - display the file type\n");
                        Result.Add("file [FILENAME]  - display the file type\n");
                        break;
                    }
                case "man":
                    {
                        Console.WriteLine("man [COMMANDNAME] - get information about command\n");
                        Result.Add("man [COMMANDNAME] - get information about command\n");
                        break;
                    }
                case "mkdir":
                    {
                        Console.WriteLine("mkdir [DIRECTORY] - create directory.\n");
                        Result.Add("mkdir [DIRECTORY] - create directory.\n");
                        break;
                    }
                case "mv":
                    {
                        Console.WriteLine("mv [FILENAME] [NEWPATH] - move file to new path\n");
                        Result.Add("mv [FILENAME] [NEWPATH] - move file to new path\n");
                        break;
                    }
                case "ps":
                    {
                        Console.WriteLine("ps - get information about active processes\n");
                        Result.Add("ps - get information about active processes\n");
                        break;
                    }
                case "pwd":
                    {
                        Console.WriteLine("pwd - display the current working directory (name and list of files)\n");
                        Result.Add("pwd - display the current working directory (name and list of files)\n");
                        break;
                    }
                case "rm":
                    {
                        Console.WriteLine("rm [FILENAME] - delete file\n");
                        Result.Add("rm [FILENAME] - delete file\n");
                        break;
                    }
                case "wc":
                    {
                        Console.WriteLine("wc [FILENAME] - show on the screen the number of lines, words and bytes in the file\n");
                        Result.Add("wc [FILENAME] - show on the screen the number of lines, words and bytes in the file\n");
                        break;
                    }
            }
        }

        public bool IsCorrectArgs()
        {
            if (IsFirstCommand && Arguments.Count != 1)
            {
                throw new Exception("Incorrect man command args");
            }

            if (!IsFirstCommand && !Arguments.Any())
            {
                throw new Exception("Incorrect man command args");
            }
            return true;
        }
    }
}
