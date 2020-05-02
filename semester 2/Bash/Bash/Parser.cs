using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commands;

namespace Bash
{
    public static class Parser
    {
        private delegate bool Check(string input);

        public static Instruction Parse(string input)
        {
            input = input.Trim();
            if (input.First() == '$')
            {
                try
                {
                    return VariableParse(input);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                try
                {
                    return CommandParse(input);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

        }

        private static Instruction VariableParse(string input)
        {
            StringBuilder builder = new StringBuilder(input);
            Check check = str => str.Contains('=') && (str.LastIndexOf('=') == str.IndexOf('=')) 
            && str.IndexOf('=') != str.Length - 1;
            if (check(input) == false)
            {
                throw new Exception("Incorrect syntax");
            }

            char[] arrayName = new char[input.IndexOf('=') - 1];
            builder.CopyTo(1, arrayName, 0, input.IndexOf('=') - 1);

            char[] arrayValue = new char[input.Length - input.IndexOf('=') - 1];
            builder.CopyTo(input.IndexOf('=') + 1, arrayValue, 0, input.Length - input.IndexOf('=') - 1);

            string strVal = new string(arrayValue);
            string strName = new string(arrayName);

            strName = strName.Trim();
            strVal = strVal.Trim();
            check = name =>((name[0] <= 'Z' && name[0] >= 'A') || (name[0] <= 'z' && name[0] >= 'a')
            || (name[0] == '_')) && (name.All(c => !"!@#$%^&*()_+}{:><?|/.,';\\№".Contains(c)));

            if (check(strName) == false)
            {
                throw new Exception("Incorrect variable name");
            }

            return new Instruction(Instruction.InstructionType.Variable, strName, strVal);
        }

        private static Instruction CommandParse(string input)
        {
            List<ICommand> listCommands = new List<ICommand>();
            string[] commandArray = input.Split(new[] { " | " }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < commandArray.Length; ++i)
            {
                try
                {
                    listCommands.Add(GetCommand(commandArray[i], i));
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return new Instruction(Instruction.InstructionType.Command, listCommands);
        }


        private static ICommand GetCommand(string command, int number)
        {
            List<string> args = new List<string>();

            string[] wordsArray = command.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string name = wordsArray[0].ToLower();
            if (number != 0 && wordsArray.Length != 1)
            {
                throw new Exception($"Incorrect arguments for {name}");
            }
           
            for (int i = 1; i < wordsArray.Length; ++i)
            {
                args.Add(wordsArray[i]);
            }

            switch (name)
            {
                case "cat":

                    {
                        ICommand newCommand = new Cat(args, number == 0);
                        return newCommand;
                    }
                case "cp":
                    {
                        ICommand newCommand = new Cp(args, number == 0);
                        return newCommand;
                    }
                case "echo":
                    {
                        ICommand newCommand = new Echo(args, number == 0);
                        return newCommand;
                    }
                case "exit":
                    {
                        ICommand newCommand = new Exit(args);
                        return newCommand;
                    }
                case "file":
                    {
                        ICommand newCommand = new Commands.File(args, number == 0);
                        return newCommand;
                    }
                case "man":
                    {
                        ICommand newCommand = new Man(args, number == 0);
                        return newCommand;
                    }
                case "mkdir":
                    {
                        ICommand newCommand = new Mkdir(args, number == 0);
                        return newCommand;
                    }
                case "mv":
                    {
                        ICommand newCommand = new Mv(args, number == 0);
                        return newCommand;
                    }
                case "ps":
                    {
                        ICommand newCommand = new Ps(args, number == 0);
                        return newCommand;
                    }
                case "pwd":
                    {
                        ICommand newCommand = new Pwd(args, number == 0);
                        return newCommand;
                    }
                case "rm":
                    {
                        ICommand newCommand = new Rm(args, number == 0);
                        return newCommand;
                    }
                case "wc":
                    {
                        ICommand newCommand = new Wc(args, number == 0);
                        return newCommand;
                    }
                default:
                    {
                        ICommand newCommand = new CmdCommands(args, name);
                       
                        return newCommand;
                     
                    }
            }
            throw new Exception("Incorrect command name");
        }

    }
}
