using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CurvesMathLibrary;

namespace WinForms
{
    public partial class WinForm : Form
    {
        private Graphics graphic;

        private float width;

        private float height;

        private float scale;

        private int pixels;

        public WinForm()
        {
            InitializeComponent();
            PreparatoryCalculations();
        }

        private void PreparatoryCalculations()
        {
            graphic = pictureBox.CreateGraphics();
            height = (float)pictureBox.Height / 2;
            width = (float)pictureBox.Width / 2;
            buttonPlus.Tag = 0.1f;
            buttonMinus.Tag = -0.1f;
            graphic.TranslateTransform(width, height);
            curvesComboBox.Items.AddRange(new AbstractCurve[] {new Parabola(5.5f, -width, width),
                new Parabola(-3.12f, -width, width), new Ellipse(1, 1, -width, width), new Ellipse(3, 5, -width, width),
                new Hyperbola(3, 4, -width, width)});
        }

        #region Events

        private void TrackBarValueChanged(object sender, EventArgs e)
        {
            float newValue = (float)trackBar.Value / 10;
            newValue = (float)Math.Round(newValue, 1);
            if (newValue >= 0.1f)
            {
                currentScale.Text = newValue.ToString();
            }
        }

        private void ChangeValueClick(object sender, EventArgs e)
        {
            float newValue = Convert.ToSingle(currentScale.Text) + (float)((Button)sender).Tag;
            newValue = (float)Math.Round(newValue, 1);
            if (newValue > 0f && newValue <= 3f)
            {
                currentScale.Text = newValue.ToString();
                newValue = newValue * 10;
                trackBar.Value = (int)newValue;
            }
        }

        private void ButtonDrawClick(object sender, EventArgs e)
        {
            graphic.Clear(BackColor);
            scale = Convert.ToSingle(currentScale.Text);
            if (scale != 0)
            {
                CreateCoordinateSystem(StepSelection(scale));
                AbstractCurve curve = (AbstractCurve)curvesComboBox.SelectedItem;
                if (curve != null)
                {
                    DrawCurve(curve);
                }
            }

        }

        #endregion

        private float StepSelection(float scale)
        {
            float step;
            if (scale == 0.1f)
            {
                step = 12f;
            }
            else if (scale == 0.2f)
            {
                step = 5f;
            }
            else if (scale < 1f)
            {
                step = 3f;
            }
            else if (scale < 2f)
            {
                step = 1f;
            }
            else
            {
                step = 0.5f;
            }
            return step;
        }

        #region Drawing Methods
        private void CreateCoordinateSystem(float step)
        {
            DrawNullPoint();
            graphic.DrawLine(Pens.Black, -width, 0, width, 0);
            graphic.DrawLine(Pens.Black, 0, height, 0, -height);

            pixels = (int)(width / 10 * scale);

            for (float i = pixels * step; i <= width - 5; i += pixels * step)
            {
                graphic.DrawLine(Pens.Black, i, -5, i, 5);
                DrawNumberX(i / pixels);
            }

            for (float i = -pixels * step; i > -width; i -= pixels * step)
            {
                graphic.DrawLine(Pens.Black, i, -5, i, 5);
                DrawNumberX(i / pixels);
            }

            graphic.DrawLine(Pens.Black, width, 0, width - 5, -5);
            graphic.DrawLine(Pens.Black, width, 0, width - 5, 5);

            for (float i = pixels * step; i < height; i += pixels * step)
            {
                graphic.DrawLine(Pens.Black, -5, i, 5, i);
                DrawNumberY(i / pixels);
            }
            for (float i = -pixels * step; i > -height + 5; i -= pixels * step)
            {
                graphic.DrawLine(Pens.Black, -5, i, 5, i);
                DrawNumberY(i / pixels);
            }
            graphic.DrawLine(Pens.Black, 0, -height, -5, -height + 5);
            graphic.DrawLine(Pens.Black, 0, -height, 5, -height + 5);      
        }

        private void DrawNullPoint()
        {
            graphic.FillEllipse(Brushes.Black, new Rectangle(-2, -2, 4, 4));
            var font = new Font(Font.FontFamily, 6);
            var size = graphic.MeasureString("0", font);
            var rectangle = new RectangleF(new PointF(-size.Width, 3), size);
            graphic.DrawString("0", font, Brushes.Black, rectangle);
        }

        private void DrawNumberX(float number)
        {
            number = (float)Math.Round(number, 1);
            var font = new Font(Font.FontFamily, 6);
            var size = graphic.MeasureString(number.ToString(), font);
            var rectangle = new RectangleF(new PointF(number * pixels - size.Width / 2, 6), size);
            graphic.DrawString(number.ToString(), font, Brushes.Black, rectangle);
        }

        private void DrawNumberY(float number)
        {
            number = (float)Math.Round(number, 1);
            var font = new Font(Font.FontFamily, 6);
            var size = graphic.MeasureString(number.ToString(), font);
            var rectangle = new RectangleF(new PointF(-5 - size.Width, -number * pixels - size.Height / 2), size);
            graphic.DrawString(number.ToString(), font, Brushes.Black, rectangle);
        }

        private void MakeSplain(PointF[] points)
        {
            if (points.Length != 0)
            {
                graphic.DrawCurve(Pens.Red, points);
            }
        }

        private void DrawCurve(AbstractCurve curve)
        {
            List<PointF>[] points = curve.CreatePoints(pixels);
            
            MakeSplain(points[0].ToArray());
            MakeSplain(points[1].ToArray());
            MakeSplain(points[2].ToArray());
            MakeSplain(points[3].ToArray());
        }

        #endregion

    }
}
