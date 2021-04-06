using FutureLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Future
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] a = GenerateArray();
            CascadeModel cascadeModel = new CascadeModel();
            SingleModel singleModel = new SingleModel();
            ModifiedCascadeModel modifiedCascadeModel = new ModifiedCascadeModel();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int resultSingle = singleModel.ComputeLength(a);
            stopwatch.Stop();
            long timeSingle = stopwatch.ElapsedMilliseconds;

            
            stopwatch.Restart();
            int resultCascade = cascadeModel.ComputeLength(a);
            stopwatch.Stop();
            long timeCascade = stopwatch.ElapsedMilliseconds;
            
            stopwatch.Restart();
            int resultModifiedCascade = modifiedCascadeModel.ComputeLength(a);
            stopwatch.Stop();
            long timeModifiedCascade = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"The single model found a vector length of {resultSingle} in {timeSingle} milliseconds");
            Console.WriteLine($"The cascade model found a vector length of {resultCascade} in {timeCascade} milliseconds");
            Console.WriteLine($"The modified cascade model found a vector length of {resultModifiedCascade} in {timeModifiedCascade} milliseconds");
            Console.ReadLine();
        }


        private static int[] GenerateArray()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            int cap = r.Next(50000);
            List<int> lst = new List<int>();
            for (int i = 0; i < cap; i++)
            {
                lst.Add(r.Next(100));
            }
            return lst.ToArray();
        }
    }
}
