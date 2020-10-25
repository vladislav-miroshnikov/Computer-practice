using System;
using System.Linq;

namespace FutureLib
{
    public class SingleModel : IVectorLengthComputer
    {
        public int ComputeLength(int[] a)
        {
            return (int)Math.Sqrt(a.Sum(x => x * x));
        }
    }
}
