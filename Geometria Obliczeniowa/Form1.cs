using System;
using System.Drawing;
using System.Windows.Forms;

namespace cw1
{

    public partial class Form1 : Form
    {
        //bitmap
        private readonly Graphics _graphics;
        private readonly Random _random;
        private int k, n, size;
        private double _sum;

        //vertices arrays
        private Point[] _firstPolygon;
        private Point[] _secondPolygon;
        //points array
        private Point[] _randomPoints;

        public Form1()
        {
            InitializeComponent();
            size = 599;
            _graphics = pictureBox1.CreateGraphics(); //create bitmap
            _random = new Random();
            _graphics.Clear(Color.White);

        }

        //Main button
        private void button1_Click(object sender, EventArgs e)
        {
            _graphics.Clear(Color.White);
            CreatePolygon(ref _firstPolygon, 0, 200);
            CreatePolygon(ref _secondPolygon, 200, size - 100);
            CreatePoints();
            Estimate();
        }
        //Generates polygons 
        private void CreatePolygon(ref Point[] points, int dlimit, int ulimit) //dlimit, ulimit - borders of possible polygon coordinates
        {
            var size = _random.Next(3, 6); //quantity of vertices
            points = new Point[size];

            //vertice coordinates
            for (int i = 0; i < size; i++)
            {
                var x = _random.Next(30, this.size - 150);
                var y = _random.Next(dlimit, ulimit);


                var point = new Point(x, y);
                points[i] = point;
            }

            _graphics.DrawPolygon(new Pen(Color.Red), points);
        }

        //Random points generation
        private void CreatePoints()
        {
            _randomPoints = new Point[10000];
            for (int i = 0; i < 10000; i++)
            {
                var point = new Point
                {
                    X = _random.Next(0, size),
                    Y = _random.Next(0, size)
                };
                _randomPoints[i] = point;
                _graphics.FillRectangle(new SolidBrush(Color.Blue), point.X, point.Y, 1, 1);
            }
        }

        //Wykonanie obliczeń
        private void Estimate()
        {
            _sum = 0;
            //textBox1.Text = "";

            Calculate(_firstPolygon, textBox1);
            Calculate(_secondPolygon, textBox2);

            //count area of polygons
            textBox3.Text = Area().ToString();
        }

        //calculates quantity of points in polygon  
        private void Calculate(Point[] points, TextBox textBox)
        {

            int i;
            n = points.Length;
            int Points = 0;
            int maxX = int.MinValue;  

            //Searching for max X
            for (i = 0; i < n; i++)
            {
                if (points[i].X > maxX)
                    maxX = points[i].X;
            }
            //the most-forward X coordinate
            int rx = maxX + 1;

            //Check each generated point
            foreach (var point in _randomPoints)
            {

                int ry = point.Y;
                int c = 0; // c- quantity of cuts between point testline and polygon side
                for (i = 0; i < points.Length; i++)
                {
                    //Check if point testline is part of polygon side
                    if (IsPart(points[i].X, points[i].Y, points[(i + 1) % n].X, points[(i + 1) % n].Y, point.X, point.Y) == 1)
                    {
                        c++;
                    }
                    //Check if point testline crosses polygon side
                    if (IsCrossing(points[i].X, points[i].Y, points[(i + 1) % n].X, points[(i + 1) % n].Y, point.X, point.Y, rx, ry))
                        c++;
                }


                //check cuts odd
                if (c % 2 != 0)
                {
                    Points++;
                }
            }

            //Sum of points in polygons
            _sum += Points;
            textBox.Text = Points.ToString();

        }
        
        private int IsPart(int ax, int ay, int bx, int by, int cx, int cy)
        {
            int det = Det(ax, ay, bx, by, cx, cy);
            if (det != 0)
                return 0;
            if ((Math.Min(ax, bx) <= cx) && (cx <= Math.Max(ax, bx)) &&
                (Math.Min(ay, by) <= cy) && (cy <= Math.Min(ay, by)))
                return 1;
            return 0;
        }

        //determinant of the matrix
        private static int Det(int xx, int xy, int yx, int yy, int zx, int zy)
        {
            return (xx * yy + yx * zy + zx * xy - zx * yy - xx * zy - yx * xy);
        }

        
        private bool IsCrossing(int ax, int ay, int bx, int by, int px, int py, int rx, int ry)
        {
            
            return (Math.Sign(Det(px, py, rx, ry, ax, ay)) != Math.Sign(Det(px, py, rx, ry, bx, by))) &&
                   (Math.Sign(Det(ax, ay, bx, by, px, py)) != Math.Sign(Det(ax, ay, bx, by, rx, ry)));
        }

        //Estimate polygons area based on points in polygons quantity
        private double Area()
        {
            return _sum / 10000 * (500 * 500);
        }

        #region MyRegion
        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void label2_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label3_Click(object sender, EventArgs e)
        {
            
        }
        #endregion



    }
}
