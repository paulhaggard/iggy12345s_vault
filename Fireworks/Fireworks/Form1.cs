using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fireworks
{
    public partial class Form1 : Form
    {
        #region Properties

        private List<Firework> firework;
        private static int refresh = 10;   // refresh delay in ms
        private static double launchProb = 0.05;
        private static int minVel = -5;
        private static int maxVel = -20;

        #endregion

        private Thread painter;
        private Random rng = new Random();

        private bool canDraw = false;
        private bool isExitting = false;
        private bool isDrawing = false;

        public Form1()
        {
            InitializeComponent();

            // Sets up the background color of the canvas.
            canvasBox.Image = new Bitmap(Width, Height);
            canvasBox.BackColor = Color.Black;
            canvasBox.Invalidate();

            firework = new List<Firework>();

            painter = new Thread(ExecutionModule);
            painter.Start();
        }

        #region Execution Engine

        private void UpdateParticles()
        {
            lock (firework)
            {
                for(int i = firework.Count - 1; i >= 0; i--)
                {
                    if (firework[i].Done())
                        firework.RemoveAt(i);
                    else
                        firework[i].Update();
                }
            }
        }

        private void DrawParticles()
        {
            if (canDraw)
            {
                isDrawing = true;
                lock (canvasBox.Image)
                {
                    using (Graphics g = Graphics.FromImage(canvasBox.Image))
                    {
                        Task t = Task.Factory.StartNew(() =>
                        {
                            Image image = (Image)canvasBox.Image.Clone();

                            g.Clear(Color.Black);

                            Bitmap bmp = new Bitmap(image.Width, image.Height);

                            //create a color matrix object  
                            ColorMatrix matrix = new ColorMatrix();

                            //set the opacity  
                            matrix.Matrix33 *= 0.95f;

                            //create image attributes  
                            ImageAttributes attributes = new ImageAttributes();

                            //set the color(opacity) of the image  
                            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                            //now draw the image  
                            g.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                        });

                        while (!t.IsCompleted) ;

                        lock (firework)
                            foreach(Firework f in firework)
                                f.Show(g);

                        g.Dispose();
                    }
                    canvasBox.Invalidate();
                }
            }
            isDrawing = false;
        }

        /// <summary>
        /// Controls painting the particles on the screen, time updates, etc...
        /// Causes frames to happen.
        /// </summary>
        private void ExecutionModule()
        {
            while(!isExitting)
            {
                Thread.Sleep(refresh);

                if (rng.NextDouble() <= launchProb)
                    lock (firework)
                    {
                        /*
                        firework.Add(new Pikachu_Firework(new Vector2D(
                                Width/2, Height/2), //rng.Next(Width), Height),
                                new Vector2D(0, 0)));// rng.Next(maxVel, minVel))));
                                */
                        
                        double r = rng.NextDouble();

                        if (r > 0.25)
                            firework.Add(new Firework(new Vector2D(
                                rng.Next(Width), Height),
                                new Vector2D(0, rng.Next(maxVel, minVel))));
                        else if (r > 0.05)
                            firework.Add(new Heart_Firework(new Vector2D(
                                rng.Next(Width), Height),
                                new Vector2D(0, rng.Next(maxVel, minVel))));
                        /*else
                            firework.Add(new Pikachu_Firework(new Vector2D(
                                rng.Next(Width), Height),
                                new Vector2D(0, rng.Next(maxVel, minVel))));*/
                                
                    }
                        

                UpdateParticles();
                canvasBox.Invalidate();
            }
        }

        #endregion



        private void Form1_Load(object sender, EventArgs e)
        {
            while (isDrawing) ;

            canDraw = false;
            // Resizes the drawing canvas to fill the window.
            canvasBox.Left = 0;
            canvasBox.Top = 0;
            canvasBox.Width = this.Size.Width;
            canvasBox.Height = this.Size.Height;

            lock(canvasBox.Image)
                canvasBox.Image = new Bitmap(Width, Height);

            canvasBox.Invalidate();

            canDraw = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            isExitting = true;
        }

        private void canvasBox_Paint(object sender, PaintEventArgs e)
        {
            DrawParticles();
        }
    }
}
