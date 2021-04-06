using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurvesMathLibrary
{
    public class Hyperbola : AbstractCurve
    {
        private float a;

        private float b;

        public Hyperbola(float a, float b, float start, float end) :
            base(start, end)
        {

            this.a = (float)Math.Pow(a, 2);
            this.b = (float)Math.Pow(b, 2);
            Equation = "x^2/" + this.a + " - y^2/" + this.b + " = 1";
        }

        public override List<float> SolveTheEquation(float x)
        {
            List<float> result = new List<float>();
            float y = (float)Math.Sqrt(-b + (b / a) * x * x);
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
