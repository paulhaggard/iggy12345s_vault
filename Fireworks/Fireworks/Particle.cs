using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fireworks
{
    public class Particle
    {
        #region Properties

        private Vector2D pos;
        private Vector2D vel;
        private Vector2D acc;
        private Color color;
        private Brush brush;
        private Pen pen;
        private Random rng = new Random();

        #endregion

        #region Accessor Methods

        public Vector2D Vel { get => vel; set => vel = value; }
        public Vector2D Acc { get => acc; set => acc = value; }
        public Vector2D Pos { get => pos; set => pos = value; }
        public Color Color { get => color; set => UpdateColor(value); }

        private void UpdateColor(Color c)
        {
            brush = new SolidBrush(c);
            pen = new Pen((Brush)brush.Clone());
        }

        #endregion

        #region Constructors

        public Particle(Vector2D pos)
        {
            this.Pos = pos;
            Vel = new Vector2D();
            Acc = new Vector2D();

            color = Color.FromArgb(255, rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));

            brush = new SolidBrush(color);
            pen = new Pen((Brush)brush.Clone(), 1);
        }

        public Particle(Vector2D pos, Color color)
        {
            this.Pos = pos;
            Vel = new Vector2D();
            Acc = new Vector2D();
            this.color = color;

            brush = new SolidBrush(color);
            pen = new Pen((Brush)brush.Clone(), 1);
        }

        public Particle(Vector2D pos, Vector2D vel, Vector2D acc)
        {
            this.Pos = pos;
            Vel = vel;
            Acc = acc;

            color = Color.FromArgb(255, rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));

            brush = new SolidBrush(color);
            pen = new Pen((Brush)brush.Clone(), 1);
        }

        public Particle(Vector2D pos, Vector2D vel, Vector2D acc, Color color)
        {
            this.Pos = pos;
            Vel = vel;
            Acc = acc;
            this.color = color;

            brush = new SolidBrush(color);
            pen = new Pen((Brush)brush.Clone(), 1);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Steps the particle through n seconds of movement, resets the acceleration after 1 second.
        /// </summary>
        /// <param name="steps">Number of seconds to do.</param>
        public virtual void Update(int steps = 1)
        {
            for(int i = 0; i < steps; i++)
            {
                Vel += Acc;
                Pos += Vel;
                Acc *= 0;
            }
        }

        /// <summary>
        /// Applies a force to the particle.
        /// </summary>
        /// <param name="force">force to apply to the particle.</param>
        public virtual void ApplyForce(Vector2D force)
        {
            Acc += force;
        }

        /// <summary>
        /// Draws the particle on the canvas supplied
        /// </summary>
        /// <param name="g">Graphics object to use</param>
        public virtual void Show(Graphics g)
        {
            g.FillEllipse(brush, 
                new Rectangle((int)Math.Round(Pos.X), (int)Math.Round(Pos.Y),
                4, 4));
        }

        #endregion
    }
}
