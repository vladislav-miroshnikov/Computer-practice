using System;
using System.Collections.Generic;

namespace CurvesMathLibrary
{
    public class Ellipse : AbstractCurve
    {

        //general view of the curve : x^2/a^2 + y^2/b^2 = 1

        private float a;
        private float b;

        public Ellipse(float a, float b, float start , float end) :
            base(start, end)
        {
          
            this.a = (float)Math.Pow(a,2);
            this.b = (float)Math.Pow(b, 2); 
            Equation = "x^2/" + this.a + " + y^2/" + this.b + " = 1";
        }

        public override List<float> SolveTheEquation(float x)
        {
            List<float> result = new List<float>();
            float y = (float)Math.Sqrt(b - (b / a) * x * x);
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
