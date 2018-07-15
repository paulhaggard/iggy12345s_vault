using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fireworks
{
    public class Heart_Firework : Firework
    {
        public Heart_Firework(Vector2D pos, Vector2D vel) : base(pos, vel)
        {

        }

        public override void Explode(int qty = 100)
        {
            Task.Factory.StartNew(() =>
            {
                exploded = true;

                while (busy) ;
                busy = true;
                lock (particles)
                {
                    particles = new List<Particle>(qty);

                    for (int i = 0; i < qty; i++)
                    {
                        // Heart update
                        double randomT = rng.NextDouble() * 2 * Math.PI;
                        double targetX = explosionPlacementRadius * 16 * Math.Pow(Math.Sin(randomT), 3);
                        double targetY = explosionPlacementRadius *
                            (13 * Math.Cos(randomT) - 5 * Math.Cos(2 * randomT) - 2 * Math.Cos(3 * randomT) - Math.Cos(4 * randomT));


                        double angle = rng.NextDouble() * 2 * Math.PI;
                        double xVel = (rng.NextDouble() * explosionMag) * Math.Cos(angle);
                        double yVel = (rng.NextDouble() * explosionMag) * Math.Sin(angle);

                        particles.Add(new Particle(new Vector2D(firework.Pos.X + targetX, firework.Pos.Y - targetY),
                            new Vector2D(xVel, yVel),
                            new Vector2D(), firework.Color));
                    }
                }
                busy = false;
            });
        }
    }
}
