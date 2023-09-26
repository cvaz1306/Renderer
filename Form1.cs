using System;
using System.Drawing;
using System.Windows.Forms;
using System.Numerics;

namespace Renderer
{
    public partial class Form1 : Form
    {

        public int Hue;
        private Frame frame;
        private Timer animationTimer;
        private int currentFrame = 0;
        Vector3 pointIn3DSpace1 = new Vector3(0, 5, -3);
        Vector3 pointIn3DSpace2 = new Vector3(0, 0, -8);
        Vector3 pointIn3DSpace3 = new Vector3(5, 5, -3);
        Vector3 pointIn3DSpace4 = new Vector3(5, 0, -8);
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public Form1()
        {
            InitializeComponent();

            // Create a frame with 300 rows, 400 columns, and a cell size of 2 pixels
            frame = new Frame(300, 400, 2);

            // Initialize the frame with random colors
            //frame.FillWithRandomColors();

            // Set up a timer for animation
            animationTimer = new Timer();
            animationTimer.Interval = 1; // Adjust the interval as needed for your desired frame rate
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            // Replace with your 3D point.
            pointIn3DSpace1 += new Vector3(.5f, 0, 0);
            pointIn3DSpace2 += new Vector3(.5f, 0, 0);
            pointIn3DSpace3 += new Vector3(.5f, 0, 0);
            pointIn3DSpace4 += new Vector3(.5f, 0, 0);

            Vector3 screenCenter = new Vector3(0.0f, 0.0f, 0.0f); // Replace with your screen center.
            Vector3 screenNormal = new Vector3(0.0f, 0.0f, 1.0f); // Replace with your screen normal vector.

            Vector2 intersection1 = Geometry.FindIntersectionOnScreen(pointIn3DSpace1, screenCenter, screenNormal);
            Vector2 intersection2 = Geometry.FindIntersectionOnScreen(pointIn3DSpace2, screenCenter, screenNormal);
            Vector2 intersection3 = Geometry.FindIntersectionOnScreen(pointIn3DSpace3, screenCenter, screenNormal);
            Vector2 intersection4 = Geometry.FindIntersectionOnScreen(pointIn3DSpace4, screenCenter, screenNormal);


            Console.WriteLine($"Intersection Point on Screen 1: ({intersection1.X}, {intersection1.Y})");
            Console.WriteLine($"Intersection Point on Screen 2: ({intersection2.X}, {intersection2.Y})");
            Console.WriteLine($"Intersection Point on Screen 3: ({intersection3.X}, {intersection3.Y})");
            Console.WriteLine($"Intersection Point on Screen 4: ({intersection4.X}, {intersection4.Y})");


            // Update the frame for animation
            UpdateFrame();
            frame.SetColor((int)intersection1.Y + 150, (int)intersection1.X + 200, new MyColor(0, 0, 0));
            frame.SetColor((int)intersection2.Y + 150, (int)intersection2.X + 200, new MyColor(0, 0, 0));
            frame.SetColor((int)intersection3.Y + 150, (int)intersection3.X + 200, new MyColor(0, 0, 0));
            frame.SetColor((int)intersection4.Y + 150, (int)intersection4.X + 200, new MyColor(0, 0, 0));
            frame.AddLine(intersection1, intersection2, new MyColor(0, 0, 255));
            frame.AddLine(intersection2, intersection3, new MyColor(0, 0, 255));
            frame.AddLine(intersection3, intersection4, new MyColor(0, 0, 255));
            frame.AddLine(intersection4, intersection1, new MyColor(0, 0, 255));
            // Render the updated frame
            RenderFrame();
        }

        private void UpdateFrame()
        {
            frame.FillWithColor(new MyColor(240, 255, 255));
        }

        private void RenderFrame()
        {
            // Create a bitmap and render the frame to it
            using (var bitmap = frame.ToBitmap())
            {
                // Display the bitmap on the form
                using (var graphics = CreateGraphics())
                {
                    graphics.DrawImage(bitmap, 0, 0);
                }
            }
        }

        public static MyColor HSVToRGB(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            switch (hi)
            {
                case 0:
                    return new MyColor(v, t, p);
                case 1:
                    return new MyColor(q, v, p);
                case 2:
                    return new MyColor(p, v, t);
                case 3:
                    return new MyColor(p, q, v);
                case 4:
                    return new MyColor(t, p, v);
                default:
                    return new MyColor(v, p, q);
            }
        }

    }
}

    public class MyColor
    {
        public int R { get; set; } // Red component (0-255)
        public int G { get; set; } // Green component (0-255)
        public int B { get; set; } // Blue component (0-255)

        public MyColor(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }
    }

    public class Frame
    {
        private MyColor[,] colorMatrix;
        private int cellSize;

        public int Rows => colorMatrix.GetLength(0);
        public int Columns => colorMatrix.GetLength(1);
        public int Width => Columns * cellSize;
        public int Height => Rows * cellSize;

        public Frame(int rows, int columns, int cellSize)
        {
            colorMatrix = new MyColor[rows, columns];
            this.cellSize = cellSize;
        }

        public MyColor GetColor(int row, int column)
        {
            if (IsValidIndex(row, column))
            {
                return colorMatrix[row, column];
            }
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        public void SetColor(int row, int column, MyColor color)
        {
            if (IsValidIndex(row, column))
            {
                colorMatrix[row, column] = color;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid row or column index.");
            }
        }

        public void FillWithColor(MyColor color)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    colorMatrix[i, j] = color;
                }
            }
        }

        public void FillWithRandomColors()
        {
            Random rand = new Random();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    int r = rand.Next(256);
                    int g = rand.Next(256);
                    int b = rand.Next(256);
                    colorMatrix[i, j] = new MyColor(r, g, b);
                }
            }
        }

        public void AddLine(Vector2 startPoint, Vector2 endPoint, MyColor lineColor)
        {
            int x0 = (int)startPoint.X;
            int y0 = (int)startPoint.Y;
            int x1 = (int)endPoint.X;
            int y1 = (int)endPoint.Y;

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = (x0 < x1) ? 1 : -1;
            int sy = (y0 < y1) ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                // Check if the current point is within the frame bounds.
                if (x0 >= 0 && x0 < Width && y0 >= 0 && y0 < Height)
                {
                    int row = y0 / cellSize;
                    int col = x0 / cellSize;
                    colorMatrix[row, col] = lineColor;
                }

                if (x0 == x1 && y0 == y1)
                {
                    break;
                }

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        public Bitmap ToBitmap()
        {
            var bitmap = new Bitmap(Width, Height);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    MyColor color = colorMatrix[i, j];
                    using (var brush = new SolidBrush(Color.FromArgb(color.R, color.G, color.B)))
                    using (var graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.FillRectangle(brush, j * cellSize, i * cellSize, cellSize, cellSize);
                    }
                }
            }

            return bitmap;
        }

        private bool IsValidIndex(int row, int column)
        {
            return row >= 0 && row < Rows && column >= 0 && column < Columns;
        }
    }

    public static class Geometry
{
    public static Vector2 FindIntersectionOnScreen(Vector3 pointIn3DSpace, Vector3 screenCenter, Vector3 screenNormal)
    {
        // Calculate the direction vector from the point to the screen center.
        Vector3 direction = screenCenter - pointIn3DSpace;

        // Calculate the parameter t at which the line intersects the screen plane.
        float t = Vector3.Dot(direction, screenNormal);

        // Calculate the intersection point in 3D space.
        Vector3 intersection3D = pointIn3DSpace + t * direction;

        // Calculate the intersection point on the screen relative to the screen center.
        Vector3 screenVector = intersection3D - screenCenter;
        Vector2 intersection2D = new Vector2(screenVector.X, screenVector.Y); // Assuming Z-axis is screen depth.

        return intersection2D;
    }
}