using System;
using System.Collections.Generic;
using System.Linq;
using static Bash.Instruction.InstructionType;
using Commands;

namespace Bash
{
    public class Bash
    {
        private Dictionary<string, string> Variables { get; }

        public Instruction Instruction { get; private set; }

        public Bash()
        {
            Variables = new Dictionary<string, string>();
        }

        public void Start()
        {
            GetInfo();
            while (true)
            {
                Console.Write($"{Environment.MachineName}>");
                string inputStr = Console.ReadLine();
                try
                {
                    Instruction = Parser.Parse(inputStr);
                    Execute(Instruction);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void Start(string inputStr)
        {
            try
            {
                Instruction = Parser.Parse(inputStr);
                Execute(Instruction);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void GetInfo()
        {
            Console.WriteLine("You can use:\ncat [FILENAME] \ncp [FILE1] [FILE2] \necho \nexit \nfile [FILENAME] \nman [COMMANDNAME]\nmkdir [DIRECTORY] \nmv [FILENAME] [NEWPATH]\n" +
                "ps \npwd \nrm [FILENAME]\nwc [FILENAME] \nUse command man + [COMMANDNAME] to get information about the corresponding command" +
                "\nAlso you can use:\noperator $ - assignment and use of local session variables\noperator | - pipelining of commands. The result of one command becomes an input for another\n" +
                "A command that is not recognized as one of the above leads to an attempt to start by the operating system mechanisms\n " +
                "\nDescription of working with two argument commands:\nConveyor for commands cp and mv.Because they are always with two arguments, " + 
                                        "in the case pipelining the result of the previous command will be written into " + 
                                        "the first argument of cp / mv, and the second argument will need to be entered manually " +
                                        "\nexample: \ncorrect: echo[FILE1] | cp[FILE2]\nincorrect: echo [FILE1] [FILE2] | cp\n");                        
        }

        private void Execute(Instruction instructions)
        {
            switch (instructions.Type)
            {
                case Variable:
                    {
                        try
                        {
                            Variables.Add(instructions.VariableName, instructions.VariableValue);
                        }
                        catch (ArgumentException) //Rewrite
                        {
                            Variables[instructions.VariableName] = instructions.VariableValue;
                        }
                        break;
                    }


                case Command:
                    {
                        try
                        {
                            for (var i = 0; i < instructions.Commands.Count; i++)
                            { 
                                for (int j = 0; j < instructions.Commands[i].Arguments.Count; ++j)
                                {
                                    //case, when the operator $ was not met by the first team
                                    if (instructions.Commands[i].Arguments[j].Contains('$'))
                                    {
                                        string varName = instructions.Commands[i].Arguments[j].Remove(0, 1);
                                        if (Variables.ContainsKey(varName))
                                        {
                                            instructions.Commands[i].Arguments[j] = Variables[varName];
                                        }
                                    }
                                }
                                if (instructions.Commands[i].IsCorrectArgs())
                                {
                                    instructions.Commands[i].Execute();
                                    //pipelining of commands, give to the next command, if it exists, the result of the previous
                                    if (i + 1 < instructions.Commands.Count)
                                    {
                                        //Conveyor for commands cp and mv.Because they are always with two arguments,
                                        //in the case pipelining the result of the previous command will be written into
                                        //the first argument of cp / mv, and the second argument will need to be entered manually
                                        //example: echo[FILE1] | cp[FILE2]
                                        if ((instructions.Commands[i + 1] is Cp || instructions.Commands[i + 1] is Mv) &&
                                            instructions.Commands[i + 1].Arguments.Count != 0)
                                        {
                                            string temp = instructions.Commands[i + 1].Arguments[0];
                                            instructions.Commands[i + 1].Arguments[0] = instructions.Commands[i].Result[0];
                                            instructions.Commands[i + 1].Arguments.Add(temp);
                                        }
                                        else if (!(instructions.Commands[i + 1] is Cp) && !(instructions.Commands[i + 1] is Mv)) 
                                        {
                                            instructions.Commands[i + 1].Arguments = instructions.Commands[i].Result;
                                        }                                      
                                    }
                                } 
                            }
                        }
                        catch (Exception e)
                        {

                            Console.WriteLine(e.Message);
                        }

                        break;
                    }
            }
        }
    }
}
