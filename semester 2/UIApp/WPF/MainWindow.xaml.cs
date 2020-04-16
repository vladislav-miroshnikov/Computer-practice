using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Drawing;
using CurvesMathLibrary;
using Ellipse = CurvesMathLibrary.Ellipse;

namespace WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {

        private float height; 

        private float width;

        private float scale;

        private int pixels;

        public MainWindow()
        {
            InitializeComponent();
            PreparatoryCalculations();
        }

        private void PreparatoryCalculations()
        {
            height = (float)canvas.Height / 2;
            width = (float)canvas.Width / 2;
            buttonMinus.Tag = -0.1f;
            buttonPlus.Tag = 0.1f;
            List<AbstractCurve> list = new List<AbstractCurve>() { new Parabola(5.5f, -width, width),
                new Parabola(-3.12f, -width, width), new Ellipse(1, 1, -width, width), new Ellipse(3, 5, -width, width),
                new Hyperbola(3, 4, -width, width)};
            curvesComboBox.ItemsSource = list;
        }

        #region Events

        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            float newValue = (float)slider.Value;
            newValue = (float)Math.Round(newValue, 1);
            currentScale.Content = newValue.ToString();
        }

        private void ChangeScaleClick(object sender, RoutedEventArgs e)
        {
            float newValue = Convert.ToSingle(currentScale.Content) + (float)((Button)sender).Tag;
            newValue = (float)Math.Round(newValue, 1);
            if (newValue > 0f && newValue <= 3f)
            {
                currentScale.Content = newValue.ToString();
                slider.Value = newValue;
            }
        }

        private void ButtonDrawClick(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            scale = Convert.ToSingle(currentScale.Content);
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
            DrawNullPoint(width, height, 4);

            DrawLine(0, height, width * 2, height);
            DrawLine(width, 0, width, height * 2);

            DrawLine(width * 2, height, width * 2 - 4, height - 4);
            DrawLine(width * 2, height, width * 2 - 4, height + 4);
            DrawLine(width, 0, width - 4, 4);
            DrawLine(width, 0, width + 4, 4);

            pixels = (int)(width / 10 * scale);

            for (float i = pixels * step; i < width; i += pixels * step)
            {
                float x = width + i;
                DrawLine(x, height - 5, x, height + 5);
                DrawX(i / pixels);
            }

            for (float i = pixels * step; i < height; i += pixels * step)
            {
                float y = height + i;
                DrawLine(width - 5, y, width + 5, y);
                DrawY(i / pixels);
            }

            for (float i = -pixels * step; i > -width; i -= pixels * step)
            {
                float x = width + i;
                DrawLine(x, height - 5, x, height + 5);
                DrawX(i / pixels);
            }

            for (float i = -pixels * step; i > -height; i -= pixels * step)
            {
                float y = height + i;
                DrawLine(width - 5, y, width + 5, y);
                DrawY(i / pixels);
            }
        }

        private void DrawNullPoint(float x, float y, float size)
        {
            System.Windows.Shapes.Ellipse point = new System.Windows.Shapes.Ellipse
            {
                Width = size,
                Height = size,
                Fill = System.Windows.Media.Brushes.Black
            };
            Canvas.SetLeft(point, x - size / 2);
            Canvas.SetTop(point, y - size / 2);
            canvas.Children.Add(point);

            TextBlock textBlock = new TextBlock
            {
                Text = "0",
                FontSize = 10
            };
            Canvas.SetLeft(textBlock, width - 9);
            Canvas.SetTop(textBlock, height + 2);
            canvas.Children.Add(textBlock);
        }

        private void DrawLine(float x1, float y1, float x2, float y2)
        {
            Line line = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = System.Windows.Media.Brushes.Black
            };
            canvas.Children.Add(line);
        }

        private void DrawX(float number)
        {
            number = (float)Math.Round(number, 1);
            TextBlock textBlock = new TextBlock
            {
                Text = number.ToString(),
                FontSize = 10
            };
            Canvas.SetLeft(textBlock, width + number * pixels - 4);
            Canvas.SetTop(textBlock, height + 4);
            canvas.Children.Add(textBlock);
        }

        private void DrawY(float number)
        {
            number = (float)Math.Round(number, 1);
            TextBlock textBlock = new TextBlock
            {
                Text = (-number).ToString(),
                FontSize = 10
            };
            Canvas.SetLeft(textBlock, width + 7);
            Canvas.SetTop(textBlock, height + number * pixels - 7);
            canvas.Children.Add(textBlock);
        }
 
        private void PolylineDraw(List<PointF> points)
        {
            List<PointF> listPoints = points;
            Polyline polyline = new Polyline();
            foreach (PointF temp in listPoints)
            {
                if (Math.Abs(temp.Y) <= height)
                {
                    polyline.Points.Add(new System.Windows.Point(temp.X + width, temp.Y + height));
                }
            }
            polyline.Stroke = System.Windows.Media.Brushes.Red;
            polyline.StrokeThickness = 2;

            canvas.Children.Add(polyline);
        }

        private void DrawCurve(AbstractCurve curve)
        {
            List<PointF>[] points = curve.CreatePoints(pixels);
            PolylineDraw(points[0]);
            PolylineDraw(points[1]);
            PolylineDraw(points[2]);
            PolylineDraw(points[3]);
        }

        #endregion

    }
}
