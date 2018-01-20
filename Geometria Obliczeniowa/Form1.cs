using System;
using System.Drawing;
using System.Windows.Forms;

namespace cw1
{
    public partial class Form1 : Form
    {
        private readonly Graphics _graphics; //do tworzenia bitmapy i rysowania
        private readonly Random _random;
        private int k, n, size;
        private double _sum;

        //tabele punktów na wierzchołki figur
        private Point[] _firstPolygon;
        private Point[] _secondPolygon;
        //tabela punktów na losowe
        private Point[] _randomPoints;

        public Form1()
        {
            InitializeComponent();
            size = 599;
            _graphics = pictureBox1.CreateGraphics(); //tworzymy mapę
            _random = new Random();
            _graphics.Clear(Color.White); //czyścimy
            
        }

        //Wykonywanie programu po naciśnięciu guzika
        private void button1_Click(object sender, EventArgs e)
        {
            _graphics.Clear(Color.White);
            CreatePolygon(ref _firstPolygon, 0, 200);
            CreatePolygon(ref _secondPolygon, 200, size - 100);
            CreatePoints();
            Estimate();
        }
        //Metoda generująca współrzędne wierzchołków fi
        private void CreatePolygon(ref Point[] points,int dlimit, int ulimit) //dlimit i ulimit to zakresy w jakich generuje się dana figura, podawane przy wykonywaniu dla niej metody (będzie niżej ;d)
        {
            var size = _random.Next(3, 6); //losowanie ilości wierzchołków
            points = new Point[size];

            for (int i = 0; i < size; i++)
            {
                //punkty x i y dla każdego wierzchołka
                var x = _random.Next(30, this.size-150);
                var y = _random.Next(dlimit, ulimit);

                //stworzenie nowego punktu wierzchołka i przypisanie go do tymczasowej dla danego wielokąta tablicy
                var point = new Point(x, y);
                points[i] = point;
            }
            //utworzenie figury na podstawie punktów w tymczasowej tablicy "points"
            _graphics.DrawPolygon(new Pen(Color.Red), points);
        }

        //Generowanie losowych punktów, myślę że tłumaczyć jak działa nie trzeba ;d
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
                _randomPoints[i] = point; //przypisanie punktu do tabeli z wygenerowanymi punktami
                _graphics.FillRectangle(new SolidBrush(Color.Blue), point.X, point.Y, 1, 1); //narysowanie go
            }
        }

        //Wykonanie obliczeń
        private void Estimate()
        {
            _sum = 0.0;
            textBox1.Text = "";
            //metoda obliczenia ilości punktów w wielokącie
            Calculate(_firstPolygon, textBox1); 
            Calculate(_secondPolygon, textBox2);

            //Obliczanie pola na podstawie ilości punktów
            textBox3.Text = Area().ToString(); 
        }

        //Kombajn do policzenia tego wszystkiego, wykonywany jako metoda oddzielnie dla każdej figury
        private void Calculate(Point[] points, TextBox textBox)
        {

            int i;
            n = points.Length;
            int Points = 0;
            int maxX = int.MinValue;  // maxX - najbardziej na prawo wysunięty punkt

            //szukanie maxX
            for (i = 0; i < n; i++)
            {
                if (points[i].X > maxX)
                    maxX = points[i].X;
            }
            int rx = maxX + 1; 

         
            foreach (var point in _randomPoints)
            {

                int ry = point.Y; 
                int c = 0; // c- iloś przecięć
                for (i = 0; i < points.Length; i++) 
                {
                   
                    if (IsPart(points[i].X, points[i].Y, points[(i + 1) % n].X, points[(i + 1) % n].Y, point.X, point.Y) == 1)
                    {
                        c++;
                    }
                    
                    if (IsCrossing(points[i].X, points[i].Y, points[(i + 1) % n].X, points[(i + 1) % n].Y, point.X, point.Y, rx, ry))
                        c++; 
                }
              

               
                if (c % 2 != 0)
                {
                    Points++;
                }
            }
    
            _sum += Points;
            textBox.Text = Points.ToString(); 

        }
        //już chyba wiadomo co to, generalnie dość prosty wzorek, zrobiony na podstanie tego wzoru: "http://www.algorytm.org/geometria-obliczeniowa/przynaleznosc-punktu-do-odcinka.html" (to jest to co chciał zebym mu rysował ;d)
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

        //Wyznacznik macierzy dla trzech punktów, bo się przydaje ;d
        private static int Det(int xx, int xy, int yx, int yy, int zx, int zy)
        {
            return (xx * yy + yx * zy + zx * xy - zx * yy - xx * zy - yx * xy);
        }

        //jak było mówione, sprawdzanie czy ta pozioma prosta przecina się z bokiem. zrobione na podstawie tego "http://www.algorytm.org/geometria-obliczeniowa/przecinanie-sie-odcinkow.html"
        private bool IsCrossing( int ax, int ay, int bx, int by, int px, int py, int rx, int ry)
        {
                //porównywanie znaków wyznaczników, w taki sposób jak było powiedziane na tej stronce. jeśli nie są równe to zwraca true - odcinki przecinają się
                return (Math.Sign(Det(px, py, rx, ry, ax, ay)) != Math.Sign(Det(px, py, rx, ry, bx, by))) &&
                       (Math.Sign(Det(ax, ay, bx, by, px, py)) != Math.Sign(Det(ax, ay, bx, by, rx, ry)));
        }

        //liczenie pola na podstawie sumy punktów w figurach, wzór z wykładu.
        private double Area()
        {
            return _sum / 10000 * (500 * 500);
        }

        #region MyRegion
        private void button4_Click(object sender, EventArgs e)
        {
            //Litwo, Ojczyzno moja!
        }

        private void label1_Click(object sender, EventArgs e)
        {
            //ty jesteś jak zdrowie
        }

        private void label2_Click(object sender, EventArgs e)
        {
            //Ile cię trzeba cenić
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //ten tylko się dowie
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //Kto cię stracił
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //Dziś piękność twą w całej ozdobie
        }

        private void label3_Click(object sender, EventArgs e)
        {
            //Widzę i opisuję, bo tęsknię po tobie.
        }
        #endregion



    }
}
