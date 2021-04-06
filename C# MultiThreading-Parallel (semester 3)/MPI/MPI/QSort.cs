using System;
using System.Collections.Generic;

namespace MPI
{
    internal static class QSort
    {
        static int Partition<T>(List<T> array, int a, int b) where T : IComparable<T>
        {
            int i = a;
            for (int j = a; j <= b; j++)         
            {
                if (array[j].CompareTo(array[b]) <= 0)  
                {
                    T t = array[i];                  
                    array[i] = array[j];                 
                    array[j] = t;                    
                    i++;                         
                }
            }
            return i - 1;                      
        }

        public static void Quicksort<T>(List<T> array, int firstBorder, int secondBorder) where T : IComparable<T>
        {
            if (firstBorder >= secondBorder)
            {
                return;
            }
            int pivot = Partition(array, firstBorder, secondBorder);
            Quicksort(array, firstBorder, pivot - 1);
            Quicksort(array, pivot + 1, secondBorder);
        }


    }
}
