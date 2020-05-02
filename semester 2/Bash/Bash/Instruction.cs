using System.Collections.Generic;
using Commands;

namespace Bash
{
    public class Instruction
    {
        public enum InstructionType
        {
            Variable,
            Command,
        }

        public InstructionType Type { get; }

        public string VariableName { get; }
        public string VariableValue { get; }

        public List<ICommand> Commands { get; }

        public Instruction(InstructionType type, List<ICommand> commandsList)
        {
            Type = type;
            Commands = commandsList;
        }

        public Instruction(InstructionType type, string variableName, string variableValue)
        {
            Type = type;
            VariableName = variableName;
            VariableValue = variableValue;
        }
    }
}
