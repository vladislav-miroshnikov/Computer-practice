using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MPI
{
    public static class ArrayGenerator
    {
        public static string FileFirst { get; private set; }
        public static string FileSecond { get; private set; }

        public static void GenerateArray()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Random r = new Random(DateTime.Now.Millisecond);

            int capacity = 1500000;

            List<int> lst = new List<int>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                lst.Add(r.Next(100000));
            }
            string filePath = Path.Combine(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\")));
            FileFirst = filePath + "testUnsort.txt";
            FileSecond = filePath + "testSort.txt";
            File.WriteAllText(FileFirst, string.Join(" ", lst.Select(x => x.ToString())));

            lst.Sort();

            File.WriteAllText(FileSecond, string.Join(" ", lst.Select(x => x.ToString())));
            sw.Stop();
        }     
    }
}
