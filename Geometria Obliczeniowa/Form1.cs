using System;
using System.Drawing;
using System.Windows.Forms;

namespace cw1
{

    public partial class Form1 : Form
    {
        //bitmap
        public readonly Graphics Graphics;
        public readonly Random Random;

        //Size of bitmap
        public new int Size;

        public int PointsQuantity = 10000;
        public double Sum;

        //vertices arrays
        public Point[] firstPolygon;
        public Point[] secondPolygon;
        //points array
        public Point[] RandomPoints;

        public Form1()
        {
            InitializeComponent();
            
            Graphics = pictureBox1.CreateGraphics(); //create bitmap

            Size = pictureBox1.Size.Width;
            Random = new Random();
            Graphics.Clear(Color.White);

        }

        //Main button
        private void button1_Click(object sender, EventArgs e)
        {
            Graphics.Clear(Color.White);
            CreatePolygon(ref firstPolygon, 0, 200);
            CreatePolygon(ref secondPolygon, 200, Size - 100);
            CreatePoints();
            Estimate();
        }
        //Generates polygons 
        private void CreatePolygon(ref Point[] Points, int dlimit, int ulimit) // dlimit (down limit), ulimit (up limit) - borders of possible polygon coordinates
        {
            var verticesQuantity = Random.Next(3, 6); // Quantity of vertices
            Points = new Point[verticesQuantity]; //list with polygons vertices coordinates

            //Get vertices coordinates
            for (int i = 0; i < verticesQuantity; i++)
            {
                var x = Random.Next(30, this.Size - 150);
                var y = Random.Next(dlimit, ulimit);

                var point = new Point(x, y);
                Points[i] = point;
            }

            Graphics.DrawPolygon(new Pen(Color.Red), Points); // Draw polygons
        }

        //Random points generation
        private void CreatePoints()
        {
            RandomPoints = new Point[PointsQuantity];
            for (int i = 0; i < RandomPoints.Length; i++)
            {
                var point = new Point
                {
                    X = Random.Next(0, Size),
                    Y = Random.Next(0, Size)
                };
                RandomPoints[i] = point;
                Graphics.FillRectangle(new SolidBrush(Color.Blue), point.X, point.Y, 1, 1);
            }
        }

        #region Calculations
        private void Estimate()
        {
            Sum = 0;


            Calculate(firstPolygon, textBox1);
            Calculate(secondPolygon, textBox2);

            //count area of polygons
            textBox3.Text = Area().ToString();
        }

        //calculates quantity of points in polygon  
        private void Calculate(Point[] points, TextBox textBox)
        {
            
            int i;
            int  n = points.Length;
            int Points = 0;
            int maxX = int.MinValue;  

            //Searching for max X
            for (i = 0; i < n; i++)
            {
                if (points[i].X > maxX)
                    maxX = points[i].X;
            }
            //the most-forward X coordinate
            Point r = new Point() {X = maxX+1 };

            //Check each generated point
            foreach (var point in RandomPoints)
            {
                r.Y = point.Y;
                
                int c = 0; // c - quantity of cuts between point testline and polygon side
                for (i = 0; i < points.Length; i++)
                {
                    //Check if point testline is part of polygon side
                    if (IsPart(points[i], points[(i + 1) % n], point) == 1)
                    {
                        c++;
                    }
                    //Check if point testline crosses polygon side
                    else if (IsCrossing(points[i], points[(i + 1) % n], point, r))
                        c++;
                    
                }


                //check cuts odd
                if (c % 2 != 0)
                {
                    Points++;
                }
            }

            //Sum of points in polygons
            Sum += Points;
            textBox.Text = Points.ToString();

        }
        
        //Method IsPart compares coordinates of given points and and check is point c is between a and b
        private int IsPart(Point a, Point b, Point c)
        {
            int det = Det(a, b, c);
            if (det != 0)
                return 0;
            if ((Math.Min(a.X, b.X) <= c.X) && (c.X <= Math.Max(a.X, b.X)) &&
                (Math.Min(a.Y, b.Y) <= c.Y) && (c.Y <= Math.Min(a.Y, b.Y)))
                return 1;
            return 0;
        }

        //determinant of the matrix
        private static int Det(Point x, Point y, Point z)
        {
            return (x.X * y.Y + y.X * z.Y + z.X * x.Y - z.X * y.Y - x.X * z.Y - y.X * x.Y);
        }
       

        private bool IsCrossing(Point a, Point b, Point p, Point r)
        {

            return (Math.Sign(Det(p, r, a)) != Math.Sign(Det(p, r, b))) &&
                   (Math.Sign(Det(a, b, p)) != Math.Sign(Det(a, b, r)));
        }


        //Estimate polygons area based on points in polygons quantity
        private double Area()
        {
            return Sum / PointsQuantity * (500 * 500);
        }

        #endregion
       
    }
}
