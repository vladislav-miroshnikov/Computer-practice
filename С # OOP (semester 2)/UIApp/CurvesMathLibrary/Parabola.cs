using System;
using System.Collections.Generic;

namespace CurvesMathLibrary
{
    public class Parabola : AbstractCurve
    {
        //general view of the curve : y^2 = p*x
        private float a;

        public Parabola(float a, float start, float end) 
           : base(start, end)
        {
            this.a = a;
            Equation = "y^2 = " + a + "x";
        }

        public override List<float> SolveTheEquation(float x)
        {
            List<float> result = new List<float>();
            float y = (float)Math.Sqrt(a * x);
            if (y == 0)
            {
                result.Add(y);
            }          
            else 
            {
                result.Add(y);
                result.Add(-y);
            }
            return result;
        }
    }
}
