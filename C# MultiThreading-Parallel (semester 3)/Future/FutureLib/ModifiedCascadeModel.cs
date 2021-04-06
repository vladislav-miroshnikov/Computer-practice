using System;
using System.Collections.Generic;
using System.Linq;

namespace FutureLib
{
    public class ModifiedCascadeModel : IVectorLengthComputer
    {
        public int ComputeLength(int[] a)
        {
            try
            {
                List<int> list = a.ToList();

                if (list.Count % 2 == 1)
                {
                    list.Add(0);
                }

                int number = list.Count / (int)Math.Sqrt(list.Count);
                List<Future<int>> futures = new List<Future<int>>();
                for(int i = 0; i < number; i++)
                {
                    int id = i;
                    futures.Add(new Future<int>(() => CalculateSequential(list, id, number)));
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        private int CalculateSequential(List<int> list, int id, int number)
        {
            int result = 0;
            for (int i = id * list.Count / number; (i < list.Count) && (i < (id + 1) * list.Count / number); i++)
            {
                result += list[i] * list[i];
            }
            return result;
        }

        private int Sum(int a, int b)
        {
            return a + b;
        }
    }
}
