using System;
using System.Collections.Generic;
using System.Linq;

namespace FutureLib
{
    public class CascadeModel : IVectorLengthComputer
    {
        public int ComputeLength(int[] a)
        {
            try
            {
                List<Future<int>> futures = new List<Future<int>>();
                List<int> list = a.ToList();

                if (list.Count % 2 == 1)
                {
                    list.Add(0);
                }

                for (int i = 0; i < a.Length; i += 2)
                {
                    int copy = i;
                    futures.Add(new Future<int>(() => Sum(Square(list[copy]), Square(list[copy + 1]))));
                }
                while (true)
                {
                    if (futures.Count == 1)
                    {
                        break;
                    }
                    for (int i = 0; i < futures.Count - 1; i += 2)
                    {
                        int x = futures[i].GetResult();
                        int y = futures[i + 1].GetResult();
                        futures[i].Dispose();
                        futures[i] = new Future<int>(() => Sum(x, y));
                        futures[i + 1].Dispose();
                        futures.RemoveAt(i + 1);
                    }
                }
                int result = (int)Math.Sqrt(futures[0].GetResult());
                futures[0].Dispose();
                futures.Clear();
                return result;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }        
        }

        private int Sum(int a, int b)
        {
            return a + b;
        }

        private int Square(int a)
        {
            return a * a;
        }
    }
}
