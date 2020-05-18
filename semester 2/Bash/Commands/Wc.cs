using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commands
{
    public class Wc : ICommand
    {
        public List<string> Arguments { get; set; }
        public bool IsFirstCommand { get; set; }
        public List<string> Result { get; set; }

        public Wc(List<string> arguments, bool isFirst)
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
            int numberOfLines;
            int numberOfWords;
            int numberOfBytes;

            try
            {
                string[] lines = System.IO.File.ReadAllLines(Arguments[0]);
                numberOfLines = lines.Length;

                numberOfWords = 0;
                foreach (string tmp in lines)
                {
                    string[] words = tmp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    numberOfWords += words.Length;
                }

                byte[] bytes = System.IO.File.ReadAllBytes(Arguments[0]);
                numberOfBytes = bytes.Length;
            }
            catch
            {
                throw new ArgumentException("Incorrect path to file");
            }

            Console.WriteLine($"Number of lines: {numberOfLines}");
            Console.WriteLine($"Number of words: {numberOfWords}");
            Console.WriteLine($"Number of bytes: {numberOfBytes}");
            Result.Add(numberOfLines.ToString());
            Result.Add(numberOfWords.ToString());
            Result.Add(numberOfBytes.ToString());
            Console.WriteLine();
        }

        public bool IsCorrectArgs()
        {
            if (!IsFirstCommand && !Arguments.Any())
            {
                throw new Exception("Incorrect wc command args");
            }

            if (IsFirstCommand && Arguments.Count != 1)
            {
                throw new Exception("Incorrect wc command args");
            }

            return true;
        }
    }
}
