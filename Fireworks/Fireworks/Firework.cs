using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fireworks
{
    public class Firework
    {
        #region Properties

        private Particle firework;
        private List<Particle> particles;
        private int explosionAlpha = 255;
        private bool exploded = false;
        private bool busy = false;
        private static int explosionMag = 10;

        private Random rng = new Random();

        #endregion

        #region Constructors

        public Firework(Vector2D pos, Vector2D vel)
        {
            firework = new Particle(pos, vel, new Vector2D());
            particles = new List<Particle>();
        }

        #endregion

        #region Methods

        public virtual void Update()
        {
            if (!exploded)
            {
                firework.ApplyForce(PhysConstants.Gravity);
                firework.Update();

                if (firework.Vel.Y >= 0)
                    Explode();
            }
            else
            {
                while (busy) ;
                busy = true;
                lock (particles)
                {
                    explosionAlpha -= 4;
                    foreach (Particle p in particles)
                    {
                        p.ApplyForce(PhysConstants.Gravity);
                        p.Vel *= 0.9;
                        p.Update();
                    }
                    if (explosionAlpha < 0)
                        particles.Clear();
                }
                busy = false;
            }
        }

        public virtual void Show(Graphics g)
        {
            if (!exploded)
                firework.Show(g);
            else
            {
                while (busy) ;
                busy = true;
                lock (particles)
                {
                    foreach (Particle p in particles)
                    {
                        if (explosionAlpha >= 0)
                            p.Color = Color.FromArgb(explosionAlpha, p.Color.R, p.Color.G, p.Color.B);

                        p.Show(g);
                    }
                }
                busy = false;
            }
        }

        public virtual void Explode(int qty = 100)
        {
            exploded = true;

            while (busy) ;
            busy = true;
            lock (particles)
            {
                particles = new List<Particle>(qty);

                for (int i = 0; i < qty; i++)
                {
                    double angle = rng.NextDouble() * 2 * Math.PI;
                    double xVel = (rng.NextDouble() * explosionMag) * Math.Cos(angle);
                    double yVel = (rng.NextDouble() * explosionMag) * Math.Sin(angle);

                    particles.Add(new Particle(new Vector2D(firework.Pos.X, firework.Pos.Y),
                        new Vector2D(xVel, yVel),
                        new Vector2D(), firework.Color));
                }
            }
            busy = false;
        }

        public bool Done()
        {
            return (exploded && particles.Count == 0);
        }

        #endregion
    }
}
