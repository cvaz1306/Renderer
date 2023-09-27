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
        Vector3 pointIn3DSpace1 = new Vector3(0, 5, -6);
        Vector3 pointIn3DSpace2 = new Vector3(0, 0, -5);
        Vector3 pointIn3DSpace3 = new Vector3(5, 5, -6);
        Vector3 pointIn3DSpace4 = new Vector3(5, 0, -5);
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public Form1()
        {
            InitializeComponent();

            // Create a frame with 300 rows, 400 columns, and a cell size of 2 pixels
            frame = new Frame(300, 400, 1);

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
            // Get the mouse position in screen coordinates.
            Point screenMousePos = Control.MousePosition;

            // Convert the screen coordinates to client coordinates (relative to your form).
            Point clientMousePos = PointToClient(screenMousePos);

            // Now, clientMousePos contains the mouse position relative to your form.
            int mouseX = clientMousePos.X;
            int mouseY = clientMousePos.Y;

            // You can use mouseX and mouseY in your code as needed.

            // Replace with your 3D point.
            pointIn3DSpace1 += new Vector3((mouseX - 500) / 100, 0, 0);
            pointIn3DSpace2 += new Vector3((mouseX - 500) / 100, 0, 0);
            pointIn3DSpace3 += new Vector3((mouseX - 500) / 100, 0, 0);
            pointIn3DSpace4 += new Vector3((mouseX - 500) / 100, 0, 0);
            Console.WriteLine("Mouse X: " + mouseX);
            Vector3 screenCenter = new Vector3(0.0f, 0.0f, 0.0f); // Replace with your screen center.
            Vector3 screenNormal = new Vector3(0.0f, 0.0f, 1.0f); // Replace with your screen normal vector.

            Vector2 intersection1 = Geometry.FindIntersectionOnScreen(pointIn3DSpace1, screenCenter, screenNormal);
            Vector2 intersection2 = Geometry.FindIntersectionOnScreen(pointIn3DSpace2, screenCenter, screenNormal);
            Vector2 intersection3 = Geometry.FindIntersectionOnScreen(pointIn3DSpace3, screenCenter, screenNormal);
            Vector2 intersection4 = Geometry.FindIntersectionOnScreen(pointIn3DSpace4, screenCenter, screenNormal);



            // Update the frame for animation
            UpdateFrame();
            // Set colors for the corners.
            frame.SetColor((int)intersection1.Y + 150, (int)intersection1.X + 200, new MyColor(0, 0, 0));
            frame.SetColor((int)intersection2.Y + 150, (int)intersection2.X + 200, new MyColor(255, 0, 0));
            frame.SetColor((int)intersection3.Y + 150, (int)intersection3.X + 200, new MyColor(0, 255, 0));
            frame.SetColor((int)intersection4.Y + 150, (int)intersection4.X + 200, new MyColor(0, 0, 255));

            // Draw lines to connect the corners and form a square.
            frame.AddLine(intersection1 * 2, intersection3 * 2, new MyColor(255, 0, 0));
            frame.AddLine(intersection3 * 2, intersection4 * 2, new MyColor(0, 255, 0));
            frame.AddLine(intersection2 * 2, intersection4 * 2, new MyColor(0, 0, 255));
            frame.AddLine(intersection1 * 2, intersection2 * 2, new MyColor(0, 255, 255));

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

        private void button1_Click(object sender, EventArgs e)
        {
            pointIn3DSpace1 = new Vector3(0, 5, -6);
            pointIn3DSpace2 = new Vector3(0, 0, -5);
            pointIn3DSpace3 = new Vector3(5, 5, -6);
            pointIn3DSpace4 = new Vector3(5, 0, -5);
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

    
    