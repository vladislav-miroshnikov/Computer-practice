using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bash;
using Commands;
using System.IO;
using System.Text;
using System.Collections.Generic;
using File = Commands.File;

namespace BashUnitTest
{
    [TestClass]
    public class BashUnitTest
    {
        private static string filePath;
        private static string fileText;

        [ClassInitialize]
        public static void CreateFileForTests(TestContext context)
        {
            filePath = Directory.GetCurrentDirectory() + @"\testFile.txt";
            fileText = "If you've always wanted to live like a Viking, now's your chance.\n " +
                    "The trailer for the upcoming Assassin's Creed Valhalla dropped Thursday, showing an English king setting up a declaration of war against the Norse people.\n" +
                    "The game is slated for the end of 2020.";
            using (FileStream file = System.IO.File.Create(filePath))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(fileText);
                file.Write(info, 0, info.Length);
            }
        }

        [ClassCleanup]
        public static void Clean()
        {
            System.IO.File.Delete(filePath);
        }

        [TestMethod]
        public void ParserTest()
        {
            Instruction instruction = Parser.Parse("    echo   somepath |   cat  |  echo");
            Assert.IsTrue(instruction.Type == Instruction.InstructionType.Command);
            Assert.IsTrue(instruction.Commands[0] is Echo);
            Assert.IsTrue(instruction.Commands[1] is Cat);
            Assert.IsTrue(instruction.Commands[2] is Echo);
            Assert.AreEqual("somepath", instruction.Commands[0].Arguments[0]);
            Assert.IsTrue(instruction.Commands[0].IsFirstCommand == true);
            Assert.IsTrue(instruction.Commands[1].IsFirstCommand == false);
            Assert.IsTrue(instruction.Commands[2].IsFirstCommand == false);
            instruction = Parser.Parse("   $a     =     56");
            Assert.IsTrue(instruction.Type == Instruction.InstructionType.Variable);
            Assert.AreEqual("a", instruction.VariableName);
            Assert.AreEqual("56", instruction.VariableValue);
        }

        [TestMethod]
        public void BashTest()
        {
            Bash.Bash bash = new Bash.Bash();
            bash.Start("$a = 3");
            bash.Start("$b = 5");
            bash.Start($"    echo     {filePath}   |     file");
            Assert.AreEqual(filePath, bash.Instruction.Commands[0].Arguments[0]);
            Assert.AreEqual(filePath, bash.Instruction.Commands[0].Result[0]);
            Assert.AreEqual(filePath, bash.Instruction.Commands[1].Arguments[0]);
            Assert.AreEqual(".txt", bash.Instruction.Commands[1].Result[0]);
            bash.Start("echo $b");
            Assert.AreEqual("5", bash.Instruction.Commands[0].Result[0]);
        }

        [TestMethod]
        public void InstructionTest()
        {
            Instruction instructionFirst = new Instruction(Instruction.InstructionType.Command,
                new List<ICommand> { new Echo(new List<string> { }, true) });
            Assert.IsTrue(instructionFirst.Type == Instruction.InstructionType.Command);
            Assert.IsTrue(instructionFirst.Commands[0] is Echo);
            Instruction instructionSecond = new Instruction(Instruction.InstructionType.Variable,
                "a", "Hello");
            Assert.IsTrue(instructionSecond.Type == Instruction.InstructionType.Variable);
            Assert.AreEqual("a", instructionSecond.VariableName);
            Assert.AreEqual("Hello", instructionSecond.VariableValue);
        }

        #region AreaCommandsTests

        [TestMethod]
        public void CatTest()
        {
            Cat cat = new Cat(new List<string> { filePath }, true);
            Assert.AreEqual(filePath, cat.Arguments[0]);
            Assert.AreEqual(true, cat.IsFirstCommand);
            Assert.AreEqual(true, cat.IsCorrectArgs());
            cat.Execute();
            string[] linesArray = fileText.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < linesArray.Length; i++)
            {
                Assert.AreEqual(linesArray[i], cat.Result[i]);
            }
        }

        [TestMethod]
        public void CpTest()
        {
            string newPath = Directory.GetCurrentDirectory() + @"\newFile.txt";
            Cp cp = new Cp(new List<string> { filePath, newPath }, true);
            Assert.AreEqual(filePath, cp.Arguments[0]);
            Assert.AreEqual(newPath, cp.Arguments[1]);
            Assert.AreEqual(true, cp.IsFirstCommand);
            Assert.AreEqual(true, cp.IsCorrectArgs());
            cp.Execute();
            Assert.AreEqual(newPath, cp.Result[0]);
            string newText = System.IO.File.ReadAllText(newPath);
            Assert.AreEqual(fileText, newText);
            System.IO.File.Delete(newPath);
        }

        [TestMethod]
        public void EchoTest()
        {
            Echo echo = new Echo(new List<string> { "It's good" }, true);
            Assert.AreEqual("It's good", echo.Arguments[0]);
            Assert.AreEqual(true, echo.IsFirstCommand);
            Assert.AreEqual(true, echo.IsCorrectArgs());
            echo.Execute();
            Assert.AreEqual("It's good", echo.Result[0]);
        }

        [TestMethod]
        public void ExitTest()
        {
            Exit exit = new Exit(new List<string> { });
            Assert.AreEqual(0, exit.Arguments.Count);
            Assert.AreEqual(true, exit.IsCorrectArgs());
        }

        [TestMethod]
        public void FileTest()
        {
            File file = new File(new List<string> { filePath }, true);
            Assert.AreEqual(filePath, file.Arguments[0]);
            Assert.AreEqual(true, file.IsFirstCommand);
            Assert.AreEqual(true, file.IsCorrectArgs());
            file.Execute();
            Assert.AreEqual(".txt", file.Result[0]);
        }

        [TestMethod]
        public void ManTest()
        {
            Man man = new Man(new List<string> { "wc" }, true);
            Assert.AreEqual("wc", man.Arguments[0]);
            Assert.AreEqual(true, man.IsFirstCommand);
            Assert.AreEqual(true, man.IsCorrectArgs());
            man.Execute();
            Assert.AreEqual("wc [FILENAME] - show on the screen the number of lines, words and bytes in the file\n", man.Result[0]);
        }

        [TestMethod]
        public void MkdirTest()
        {
            string directory = Directory.GetCurrentDirectory() + @"\newDirectory";
            Mkdir mkdir = new Mkdir(new List<string> { directory }, true);
            Assert.AreEqual(directory, mkdir.Arguments[0]);
            Assert.AreEqual(true, mkdir.IsFirstCommand);
            Assert.AreEqual(true, mkdir.IsCorrectArgs());
            mkdir.Execute();
            Assert.AreEqual(directory, mkdir.Result[0]);
            Assert.IsTrue(Directory.Exists(directory) == true);
            Directory.Delete(directory);
        }

        [TestMethod]
        public void MvTest()
        {
            string path = Directory.GetCurrentDirectory() + @"\moveFile.txt";
            using (System.IO.File.Create(path))
            {

            }
            string directory = Directory.GetCurrentDirectory() + @"\newDirectory";
            Directory.CreateDirectory(directory);
            Mv mv = new Mv(new List<string> { path, directory + @"\moveFile.txt" }, true);
            Assert.AreEqual(path, mv.Arguments[0]);
            Assert.AreEqual(directory + @"\moveFile.txt", mv.Arguments[1]);
            Assert.AreEqual(true, mv.IsFirstCommand);
            Assert.AreEqual(true, mv.IsCorrectArgs());
            Console.WriteLine(mv.Arguments[0]);
            Console.WriteLine(mv.Arguments[1]);
            mv.Execute();
            Assert.AreEqual(directory + @"\moveFile.txt", mv.Result[0]);
            Assert.IsTrue(System.IO.File.Exists(directory + @"\moveFile.txt"));
            System.IO.File.Delete(directory + @"\moveFile.txt");
            Directory.Delete(directory);
        }

        [TestMethod]
        public void PsTest()
        {
            Ps ps = new Ps(new List<string> { }, true);
            Assert.AreEqual(0, ps.Arguments.Count);
            Assert.AreEqual(true, ps.IsFirstCommand);
            Assert.AreEqual(true, ps.IsCorrectArgs());
            ps.Execute();
        }

        [TestMethod]
        public void PwdTest()
        {
            Pwd pwd = new Pwd(new List<string> { }, true);
            Assert.AreEqual(0, pwd.Arguments.Count);
            Assert.AreEqual(true, pwd.IsFirstCommand);
            Assert.AreEqual(true, pwd.IsCorrectArgs());
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
            pwd.Execute();
            int i = 0;
            foreach (string file in files)
            {
                Assert.AreEqual(file, pwd.Result[i]);
                i++;
            }
        }

        [TestMethod]
        public void RmTest()
        {
            string path = Directory.GetCurrentDirectory() + @"\rmFile.txt";
            using (System.IO.File.Create(path))
            {

            }
            Rm rm = new Rm(new List<string> { path }, true);
            Assert.AreEqual(path, rm.Arguments[0]);
            Assert.AreEqual(true, rm.IsFirstCommand);
            Assert.AreEqual(true, rm.IsCorrectArgs());
            rm.Execute();
            Assert.AreEqual(path, rm.Result[0]);
            Assert.AreEqual(false, System.IO.File.Exists(path));
        }

        [TestMethod]
        public void WcTest()
        {
            Wc wc = new Wc(new List<string> { filePath }, true);
            Assert.AreEqual(filePath, wc.Arguments[0]);
            Assert.AreEqual(true, wc.IsFirstCommand);
            Assert.AreEqual(true, wc.IsCorrectArgs());
            wc.Execute();
            string[] lines = System.IO.File.ReadAllLines(filePath);
            Assert.AreEqual(lines.Length.ToString(), wc.Result[0]);

            int numberOfWords = 0;
            foreach (string tmp in lines)
            {
                string[] words = tmp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                numberOfWords += words.Length;
            }
            Assert.AreEqual(numberOfWords.ToString(), wc.Result[1]);
            byte[] bytes = System.IO.File.ReadAllBytes(filePath);
            Assert.AreEqual(bytes.Length.ToString(), wc.Result[2]);

        }

        [TestMethod]
        public void CmdCommandTest()
        {
            CmdCommands cmd = new CmdCommands(new List<string> { "vk.me" }, "ping");
            Assert.AreEqual("ping", cmd.Name);
            Assert.AreEqual("vk.me", cmd.Arguments[0]);
            Assert.AreEqual(true, cmd.IsCorrectArgs());
            cmd.Execute();
            Assert.AreEqual("vk.me", cmd.Result[0]);
        }
        
        #endregion
    }
}
