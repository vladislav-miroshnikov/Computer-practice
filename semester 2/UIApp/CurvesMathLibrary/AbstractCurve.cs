using System.Collections.Generic;
using System.Drawing;

namespace CurvesMathLibrary
{
    public abstract class AbstractCurve
    {

        public string Equation { get; set; }

        public float Start { get; set; }

        public float End { get; set; }

        public abstract List<float> SolveTheEquation(float x);
     
        public AbstractCurve(float start, float end)
        {
            Start = start;
            End = end;
        }

        //for WinForms, in WPF works binding to equation
        public override string ToString()
        {
            return Equation;
        }

        public List<PointF>[] CreatePoints(int pixels)
        {
            List<PointF> result1Quarter = new List<PointF>();
            List<PointF> result2Quarter = new List<PointF>();
            List<PointF> result3Quarter = new List<PointF>();
            List<PointF> result4Quarter = new List<PointF>();

            float step = (End - Start) / pixels / 5000;
            for (float x = Start; x < End; x += step)
            {
                List<float> resultY = SolveTheEquation(x / pixels);
                foreach (var y in resultY)
                {
                    float X = x;
                    float Y = y * pixels;
                    if (Y >= 0 && X >= 0)
                    {
                        result1Quarter.Add(new PointF(X, Y));
                    }
                    if (Y >= 0 && X < 0)
                    {
                        result2Quarter.Add(new PointF(X, Y));
                    }
                    if (Y < 0 && X < 0)
                    {
                        result3Quarter.Add(new PointF(X, Y));
                    }
                    if (Y < 0 && X > 0)
                    {
                        result4Quarter.Add(new PointF(X, Y));
                    }
                }
            }

            List<PointF>[] result = new List<PointF>[] { result1Quarter, result2Quarter, result3Quarter, result4Quarter };
            
            return result;
        }

    }

}
