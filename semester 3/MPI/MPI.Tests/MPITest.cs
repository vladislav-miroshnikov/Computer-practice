using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPI.Tests
{
    [TestClass]
    public class MPITest
    {
        private string expt;
        private string actual;
       
        private void Prepare()
        {
            ArrayGenerator.GenerateArray();
            actual = Path.Combine(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\"))) + "actual.txt";
            string args = $"mpiexec -n 4 MPI.exe {ArrayGenerator.FileFirst} {actual}";
            expt = ArrayGenerator.FileSecond;
            string filePath = Path.Combine(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\"))) + @"MPI\bin\Debug\";
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.StandardInput.WriteLine($"cd {filePath}");
            cmd.StandardInput.WriteLine(args);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
        }

        [TestMethod]
        public void MPICheck()
        {
            Prepare();
            Assert.IsTrue(CompareArrays(expt, actual));
        }

        private bool CompareArrays(string fileFirst, string fileSecond)
        {
            if (!File.Exists(fileFirst))
            {
                Console.WriteLine(string.Format("File {0} does not exist.", fileFirst));
                return false;
            }

            if (!File.Exists(fileSecond))
            {
                Console.WriteLine(string.Format("File {0} does not exist.", fileSecond));
                return false;
            }

            string contentsFirst = File.ReadAllText(fileFirst);
            string contentsSecond = File.ReadAllText(fileSecond);

            if (contentsFirst.Length != contentsSecond.Length)
            {
                Console.WriteLine("File sizes are different");
                return false;
            }

            for (int i = 0; i < contentsFirst.Length; i++)
            {
                if (contentsFirst[i] != contentsSecond[i])
                {
                    Console.WriteLine(string.Format("Files are different at position {0}", i));
                    return false;
                }
            }

            Console.WriteLine("Files are the same");
            return true;
        }
    }
}
