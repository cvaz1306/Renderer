using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Renderer
{
    public class Frame
    {
        static Vector2 offset = new Vector2(200, 150);
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
                //throw new ArgumentOutOfRangeException("Invalid row or column index.");
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

            startPoint += offset;
            endPoint += offset;
            int x0 = (int)startPoint.X;
            int y0 = (int)startPoint.Y;
            int x1 = (int)endPoint.X;
            int y1 = (int)endPoint.Y;

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = (x0 < x1) ? 1 : -1;
            int sy = (y0 < y1) ? 1 : -1;
            int err = dx - dy;
            int ct = 0;
            while (true)
            {
                // Check if the current point is within the frame bounds.
                if (x0 >= 0 && x0 < Width && y0 >= 0 && y0 < Height)
                {
                    int row = y0 / cellSize;
                    int col = x0 / cellSize;
                    colorMatrix[row, col] = (ct!=0)? lineColor: new MyColor(0,0,0);
                    ct++;
                }
                Console.WriteLine("Drawing Line");
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
}
