using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fireworks
{
    public class Pikachu_Firework : Firework
    {
        public Pikachu_Firework(Vector2D pos, Vector2D vel):base(pos,vel)
        {
            particleDiminishRate = 10;
        }

        public override void Explode(int qty = 15000)
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
                        // Algorithm goes here
                        double t = rng.NextDouble() * 52 * Math.PI;
                        Task<Tuple<double, double>> a = GenPikachuCoord(t);
                        while (!a.IsCompleted) ;
                        double targetX = a.Result.Item1;
                        double targetY = a.Result.Item2;

                        double angle = rng.NextDouble() * 2 * Math.PI;
                        double xVel = (rng.NextDouble() * explosionMag) * Math.Cos(angle);
                        double yVel = (rng.NextDouble() * explosionMag) * Math.Sin(angle);

                        particles.Add(new Particle(new Vector2D(firework.Pos.X + targetX, firework.Pos.Y - targetY),
                            new Vector2D(0, 0),//new Vector2D(xVel, yVel),
                            new Vector2D(), firework.Color));
                    }
                }
                busy = false;
            });
        }

        private async Task<Tuple<double,double>> GenPikachuCoord(double t)
        {
            double xCoord, yCoord;

            xCoord = await Task.Run(() =>
            {
                double comp1, comp2, comp3, comp4, comp5, comp6, comp7, comp8, comp9, comp10, comp11, comp12, comp13;

                // Double Checked
                comp1 = (-0.25 * Sin(10 / 7 - 23 * t) - 0.3 * Sin(4 / 3 - 22 * t) - 0.4 * Sin(1.4 - 19 * t) - 0.2 * Sin(1.4 - 16 * t) -
                3 / 7 * Sin(10 / 7 - 15 * t) - 0.375 * Sin(13 / 9 - 9 * t) - 19 / 13 * Sin(11 / 7 - 3 * t) + 44.4 * Sin(t + 11 / 7) + 20.5 * Sin(2 * t + 11 / 7) +
                34 / 9 * Sin(4 * t + 11 / 7) + 1 / 3 * Sin(5 * t + 1.6) + 0.375 * Sin(6 * t + 1.6) + 12 / 7 * Sin(7 * t + 1.625) + 11 / 7 * Sin(8 * t + 1.625) +
                0.25 * Sin(10 * t + 20 / 13) + 2 / 9 * Sin(11 * t + 16 / 9) + 0.375 * Sin(12 * t + 1.6) + 1 / 3 * Sin(13 * t + 1.75) +
                0.5 * Sin(14 * t + 1.7) + 5 / 7 * Sin(17 * t + 1.7) + 1 / 28 * Sin(18 * t + 4.5) + 0.5 * Sin(20 * t + 12 / 7) + 3 / 7 * Sin(21 * t + 16 / 9) +
                6 / 11 * Sin(24 * t + 1.75) - 979 / 9) * AdvancedMath.ThetaFunction(51 * Math.PI - t) * AdvancedMath.ThetaFunction(t - 47 * Math.PI);


                comp2 = (-1.2 * Sin(14 / 9 - 22 * t) - 1 / 9 * Sin(1.4 - 19 * t) -
                1.125 * Sin(14 / 9 - 18 * t) - 1 / 14 * Sin(15 / 11 - 15 * t) - 1.2 * Sin(11 / 7 - 12 * t) - 7 / 6 * Sin(11 / 7 - 8 * t) -
                2.9 * Sin(11 / 7 - 6 * t) - 104 / 3 * Sin(11 / 7 - 2 * t) + 415 / 18 * Sin(t + 11 / 7) + 71 / 18 * Sin(3 * t + 11 / 7) + 2.375 * Sin(4 * t + 33 / 7) +
                22 / 21 * Sin(5 * t + 1.6) + 0.375 * Sin(7 * t + 61 / 13) + 5 / 9 * Sin(9 * t + 11 / 7) + 0.125 * Sin(10 * t + 14 / 3) + 4 / 7 * Sin(11 * t + 11 / 7) +
                4 / 11 * Sin(13 * t + 14 / 3) + 1 / 7 * Sin(14 * t + 14 / 3) + 2 / 7 * Sin(16 * t + 5 / 3) + 1 / 6 * Sin(17 * t + 5 / 3) +
                6 / 7 * Sin(20 * t + 1.6) + 1 / 7 * Sin(21 * t + 5 / 3) + 1 / 6 * Sin(23 * t + 1.6) - 345.625) * AdvancedMath.ThetaFunction(47 * Math.PI - t) * AdvancedMath.ThetaFunction(t - 43 * Math.PI);

                comp3 = (1189 / 22 * Sin(t + 11 / 7) + 0.75 * Sin(2 * t + 1.625) + 5.5 * Sin(3 * t + 1.6) + 2 / 7 * Sin(4 * t + 17 / 7) + 22 / 9 * Sin(5 * t + 18 / 11) +
                0.25 * Sin(6 * t + 17 / 7) + 16 / 17 * Sin(7 * t + 20 / 11) + 0.2 * Sin(8 * t + 29 / 9) - 1627 / 7) * AdvancedMath.ThetaFunction(43 * Math.PI - t) *
                AdvancedMath.ThetaFunction(t - 39 * Math.PI);

                comp4 = (-3 / 7 * Sin(1 / 18 - 5 * t) - 0.75 * Sin(0.5 - 3 * t) + 109 / 9 * Sin(t + 1.3) + 0.625 * Sin(2 * t + 11 / 3) + 5 / 9 * Sin(4 * t + 10 / 3) +
                3 / 10 * Sin(6 * t + 2.625) + 2 / 9 * Sin(7 * t + 2 / 3) + 0.25 * Sin(8 * t + 2.875) - 1190 / 9) * AdvancedMath.ThetaFunction(39 * Math.PI - t) * AdvancedMath.ThetaFunction(t - 35 * Math.PI);

                comp5 = (188 / 21 * Sin(t + 27 / 28) + 0.4 * Sin(2 * t + 17 / 6) + 2 / 3 * Sin(3 * t + 91 / 23) + 0.375 * Sin(4 * t + 53 / 18) + 2 / 11 * Sin(5 * t + 1 / 7) - 369) *
                AdvancedMath.ThetaFunction(35 * Math.PI - t) * AdvancedMath.ThetaFunction(t - 31 * Math.PI);

                comp6 = (-8 / 9 * Sin(0.1 - 12 * t) - 34 / 9 * Sin(10 / 9 - 6 * t) - 13.7 * Sin(5 / 7 - 2 * t) +
                5.2 * Sin(t + 3.25) + 23.6 * Sin(3 * t + 1.375) + 5.375 * Sin(4 * t + 13 / 7) + 49 / 6 * Sin(5 * t + 11 / 12) +
                4.4 * Sin(7 * t + 3.25) + 17 / 16 * Sin(8 * t + 1 / 7) + 1.25 * Sin(9 * t + 0.25) + 5 / 7 * Sin(10 * t + 3.4) + 29 / 15 * Sin(11 * t + 5 / 6) - 239.375) *
                AdvancedMath.ThetaFunction(31 * Math.PI - t) * AdvancedMath.ThetaFunction(t - 27 * Math.PI);

                comp7 = (-2 / 7 * Sin(10 / 7 - 7 * t) - Sin(1 / 27 - 4 * t) + 68 / 7 * Sin(t + 44 / 15) +
                76 / 9 * Sin(2 * t + 3.7) + 30 / 7 * Sin(3 * t + 1) + 8 / 9 * Sin(5 * t + 1.5) + 0.8 * Sin(6 * t + 3.875) + 3 / 7 * Sin(8 * t + 10 / 3) +
                6 / 13 * Sin(9 * t + 8 / 7) + 1 / 3 * Sin(10 * t + 31 / 9) - 2135 / 9) * AdvancedMath.ThetaFunction(27 * Math.PI - t) * AdvancedMath.ThetaFunction(t - 23 * Math.PI);

                comp8 = (-0.375 * Sin(0.25 - 23 * t) - 0.6 * Sin(0.125 - 22 * t) - 1.625 * Sin(1.25 - 20 * t) - 9 / 7 * Sin(1.5 - 16 * t) - 8.2 * Sin(4 / 3 - 4 * t) + 768 / 7 * Sin(t + 2.2) +
                21.8 * Sin(2 * t + 16 / 7) + 150 / 13 * Sin(3 * t + 11 / 6) + 33 / 7 * Sin(5 * t + 97 / 24) + 5.75 * Sin(6 * t + 5 / 7) +
                69 / 7 * Sin(7 * t + 9 / 8) + 6.4 * Sin(8 * t + 4.2) + 7 / 6 * Sin(9 * t + 22 / 9) + 5.6 * Sin(10 * t + 5 / 6) + 4.3 * Sin(11 * t + 26 / 7) +
                14 / 9 * Sin(12 * t + 5 / 11) + 13 / 9 * Sin(13 * t + 40 / 9) + 11 / 6 * Sin(14 * t + 0.4) + 1.5 * Sin(15 * t + 1.7) +
                7 / 11 * Sin(17 * t + 4 / 3) + 0.375 * Sin(18 * t + 3.1) + 4 / 7 * Sin(19 * t + 14 / 9) + 6 / 5 * Sin(21 * t + 17 / 7) + 4 / 7 * Sin(24 * t + 3.375) + 1006 / 11) *
                AdvancedMath.ThetaFunction(23 * Math.PI) * AdvancedMath.ThetaFunction(t - 19 * Math.PI);

                comp9 = (-7.875 * Sin(2 / 7 - 8 * t) - 38 / 13 * Sin(11 / 9 - 6 * t) - 2.8 * Sin(1 / 17 - 4 * t) +
                77 / 9 * Sin(t + 0.5) + 52 / 7 * Sin(2 * t + 10 / 3) + 22 / 9 * Sin(3 * t + 76 / 17) + 2.625 * Sin(5 * t + 26 / 7) +
                3 * Sin(7 * t + 1.875) + 64 / 7 * Sin(9 * t + 57 / 14) + 6 * Sin(10 * t + 17 / 6) - 544 / 7) * AdvancedMath.ThetaFunction(19 * Math.PI - t) * AdvancedMath.ThetaFunction(t - 15 * Math.PI);

                comp10 = (-37 / 10 * Sin(4 / 7 - 5 * t) - 3 * Sin(3 / 7 - 3 * t) + 24 / 7 * Sin(t + 7 / 6) + 9 / 7 * Sin(2 * t + 0.4) + 31 / 15 * Sin(4 * t + 4.625) + 1.8 * Sin(6 * t + 2.4) +
                59 / 12 * Sin(7 * t + 13 / 6) + 15 / 7 * Sin(8 * t + 3.125) + 134 / 15 * Sin(9 * t + 7 / 3) + 9.125 * Sin(10 * t + 0.2) -
                4406 / 11) * AdvancedMath.ThetaFunction(15 * Math.PI - t) * AdvancedMath.ThetaFunction(t - 11 * Math.PI);

                comp11 = (236 / 7 * Sin(t + 6 / 5) + 0.5 * Sin(2 * t + 47 / 12) - 627 / 5) *
                AdvancedMath.ThetaFunction(11 * Math.PI - t) * AdvancedMath.ThetaFunction(t - 7 * Math.PI);

                comp12 = (69 / 2 * Sin(t + 5 / 6) - 715 / 2) *
                AdvancedMath.ThetaFunction(7 * Math.PI - t) * AdvancedMath.ThetaFunction(t - 3 * Math.PI);

                comp13 = (-19 / 9 * Sin(1.2 - 21 * t) - 3.7 * Sin(7 / 9 - 19 * t) -
                2.875 * Sin(1 - 17 * t) - 16 / 3 * Sin(7 / 6 - 16 * t) - 5.8 * Sin(0.2 - 9 * t) - 919 / 11 * Sin(1 / 7 - 3 * t) + 1573 * Sin(t + 91 / 45) +
                42.8 * Sin(2 * t + 4.125) + 421 / 14 * Sin(4 * t + 1.625) + 61 / 6 * Sin(5 * t + 3.8) + 401 / 16 * Sin(6 * t + 43 / 14) +
                511 / 51 * Sin(7 * t + 4.375) + 144 / 7 * Sin(8 * t + 5 / 6) + 13.7 * Sin(10 * t + 25 / 13) + 18 / 7 * Sin(11 * t + 15 / 7) +
                17 / 9 * Sin(12 * t + 41 / 9) + 9 / 7 * Sin(13 * t + 13 / 7) + 2.9 * Sin(14 * t + 22 / 7) + 3.125 * Sin(15 * t + 0.25) + 2.4 * Sin(18 * t + 1.375) +
                2.8 * Sin(20 * t + 27 / 7) + 1.625 * Sin(22 * t + 12 / 7) + 7 / 6 * Sin(23 * t + 7 / 9) + 26 / 11 * Sin(24 * t + 23 / 7) -
                236.375) * AdvancedMath.ThetaFunction(3 * Math.PI - t) * AdvancedMath.ThetaFunction(t + Math.PI);

                return ((comp1 + comp2 + comp3 + comp4 + comp5 + comp6 + comp7 + comp8 + comp9 + comp10 + comp11 + comp12 + comp13) *
                AdvancedMath.ThetaFunction(Math.Sqrt(Math.Sign(Sin(t / 2)))))/2/* * explosionPlacementRadius / 600*/;

            });

            yCoord = await Task.Run(() =>
            {
                double comp1, comp2, comp3, comp4, comp5, comp6, comp7, comp8, comp9, comp10, comp11, comp12, comp13;

                comp1 = (-8 / 11 * Sin(1.375 - 22 * t) - 0.5 * Sin(10 / 7 - 21 * t) + 67 / 6 * Sin(t + 33 / 7) + 1478 / 29 * Sin(2 * t + 11 / 7) +
                0.6 * Sin(3 * t + 30 / 7) + 26 / 3 * Sin(4 * t + 11 / 7) + 1 / 6 * Sin(5 * t + 13 / 9) + 30 / 29 * Sin(6 * t + 1.375) + 0.4 * Sin(7 * t + 14 / 3) +
                88 / 29 * Sin(8 * t + 1.375) + 0.25 * Sin(9 * t + 31 / 7) + 1.375 * Sin(10 * t + 1.6) + 1 / 16 * Sin(11 * t + 4.5) +
                1 / 12 * Sin(12 * t + 1.25) + 0.1 * Sin(13 * t + 25 / 11) + 1.375 * Sin(14 * t + 18 / 11) + 2 / 7 * Sin(15 * t + 4.625) + 1 / 6 * Sin(16 * t + 1.375) +
                2 / 9 * Sin(17 * t + 5 / 3) + 0.2 * Sin(18 * t + 1.7) + 1 / 13 * Sin(19 * t + 2.375) + 23 / 24 * Sin(20 * t + 12 / 7) +
                7 / 11 * Sin(23 * t + 1.8) + 9 / 7 * Sin(24 * t + 1.75) - 1538 / 7) * AdvancedMath.ThetaFunction(51 * Math.PI - t) * AdvancedMath.ThetaFunction(t - 47 * Math.PI);

                comp2 = (-2 / 7 * Sin(20 / 13 - 23 * t) - 1 / 6 * Sin(1.5 - 20 * t) - 5 / 7 * Sin(20 / 13 - 17 * t) - 1 / 9 * Sin(20 / 13 - 11 * t) - 1 / 6 * Sin(13 / 9 - 9 * t) - 19 / 6 * Sin(17 / 11 - 3 * t) +
                52.6 * Sin(t + 11 / 7) + 614 / 15 * Sin(2 * t + 11 / 7) + 8.7 * Sin(4 * t + 11 / 7) + 1 / 7 * Sin(5 * t + 1.375) + 19 / 11 * Sin(6 * t + 11 / 7) +
                1.4 * Sin(7 * t + 11 / 7) + 4 / 3 * Sin(8 * t + 1.6) + 1.8 * Sin(10 * t + 14 / 9) + 4 / 7 * Sin(12 * t + 1.6) +
                3 / 11 * Sin(13 * t + 1.5) + 0.125 * Sin(14 * t + 22 / 15) + 1 / 9 * Sin(15 * t + 12 / 7) + 1.2 * Sin(16 * t + 11 / 7) + 2 / 9 * Sin(18 * t + 11 / 7) +
                0.6 * Sin(19 * t + 1.6) + 1 / 26 * Sin(21 * t + 15 / 11) + 6 / 7 * Sin(22 * t + 1.6) - 233.375) *
                AdvancedMath.ThetaFunction(47 * Math.PI - t) * AdvancedMath.ThetaFunction(t - 43 * Math.PI);

                comp3 = (118 / 39 * Sin(t + 11 / 7) + 40 / 7 * Sin(2 * t + 33 / 7) + 49 / 25 * Sin(3 * t + 14 / 3) + 2.4 * Sin(4 * t + 1.6) +
                1 / 9 * Sin(5 * t + 32 / 13) + 2.5 * Sin(6 * t + 1.625) + 0.4 * Sin(7 * t + 4.4) + 0.75 * Sin(8 * t + 1.75) - 14.3) *
                AdvancedMath.ThetaFunction(43 * Math.PI - t) * AdvancedMath.ThetaFunction(t - 39 * Math.PI);

                comp4 = (-0.125 * Sin(2 / 3 - 8 * t) - 0.5 * Sin(1.4 - 2 * t) - 246 / 19 * Sin(1 / 7 - t) + 0.25 * Sin(3 * t + 33 / 16) + 1 / 6 * Sin(4 * t + 17 / 6) +
                0.2 * Sin(5 * t + 31 / 7) + 1 / 11 * Sin(6 * t + 50 / 17) + 0.125 * Sin(7 * t + 30 / 7) + 665 / 6) * AdvancedMath.ThetaFunction(39 * Math.PI - t) * AdvancedMath.ThetaFunction(t - 35 * Math.PI);

                comp5 = (-11.9 * Sin(7 / 15 - t) + 2 / 11 * Sin(2 * t + 25 / 7) + 2 / 9 * Sin(3 * t + 0.625) + 0.2 * Sin(4 * t + 33 / 7) + 0.25 * Sin(5 * t + 1.9) +
                102.3) * AdvancedMath.ThetaFunction(35 * Math.PI - t) * AdvancedMath.ThetaFunction(t - 31 * Math.PI);

                comp6 = (-1 / 7 * Sin(2 / 7 - 12 * t) - 0.125 * Sin(0.3 - 5 * t) + 25 / 7 * Sin(t + 77 / 17) +
                355 / 59 * Sin(2 * t + 41 / 40) + 5.4 * Sin(3 * t + 46 / 15) + 33 / 7 * Sin(4 * t + 11 / 3) + 2.7 * Sin(6 * t + 13 / 9) +
                5 / 11 * Sin(7 * t + 2.2) + 0.625 * Sin(8 * t + 3) + 1.6 * Sin(9 * t + 16 / 15) + 16 / 15 * Sin(10 * t + 1 / 7) + 7 / 9 * Sin(11 * t + 2.4) - 862 / 7) *
                AdvancedMath.ThetaFunction(31 * Math.PI - t) * AdvancedMath.ThetaFunction(t - 27 * Math.PI);

                comp7 = (-1 / 3 * Sin(1.25 - 8 * t) - 0.4 * Sin(5 / 9 - 7 * t) - 5 / 7 * Sin(1.375 - 5 * t) -
                3.5 * Sin(15 / 14 - 2 * t) + 3.625 * Sin(t + 4.1) + 11 / 6 * Sin(3 * t + 13 / 3) + 7 / 6 * Sin(4 * t + 1 / 27) + 2 / 7 * Sin(6 * t + 8 / 7) +
                1 / 9 * Sin(9 * t + 1.8) + 2 / 7 * Sin(10 * t + 0.1) + 40.2) * AdvancedMath.ThetaFunction(27 * Math.PI - t) * AdvancedMath.ThetaFunction(t - 23 * Math.PI);

                comp8 = (4 / 11 * Sin(8 / 9 - 12 * t) - 10 / 7 * Sin(19 / 13 - 10 * t) + 623 / 3 * Sin(t + 10 / 7) + 7.8 * Sin(2 * t + 10 / 11) + 251 / 9 * Sin(3 * t + 4 / 3) + 5 / 7 * Sin(4 * t + 4 / 3) +
                61 / 6 * Sin(5 * t + 4 / 3) + 14 / 9 * Sin(6 * t + 23 / 7) + 76 / 25 * Sin(7 * t + 9 / 7) + 0.75 * Sin(8 * t + 0.25) + 3.8 * Sin(9 * t + 1.5) +
                17 / 6 * Sin(11 * t + 1.2) + 1.625 * Sin(13 * t + 14 / 13) + 8 / 9 * Sin(14 * t + 17 / 6) + 24 / 25 * Sin(15 * t + 0.5) +
                1 / 6 * Sin(16 * t + 1.625) + 0.625 * Sin(17 * t + 1) + 1 / 7 * Sin(18 * t + 18 / 17) + 6 / 7 * Sin(19 * t + 1) + 0.25 * Sin(20 * t + 4 / 9) +
                2 / 7 * Sin(21 * t + 1.4) + 1 / 3 * Sin(22 * t + 8 / 7) + 0.4 * Sin(23 * t + 1 / 26) + 2 / 11 * Sin(24 * t + 8 / 7) - 30.375) *
                AdvancedMath.ThetaFunction(23 * Math.PI - t) * AdvancedMath.ThetaFunction(t - 19 * Math.PI);

                comp9 = (-11.1 * Sin(0.8 - 9 * t) - 2.4 * Sin(7 / 13 - 2 * t) + 1 / 6 * Sin(t + 48 / 11) + 1.625 * Sin(3 * t + 27 / 7) +
                71 / 24 * Sin(4 * t + 6 / 11) + 22 / 9 * Sin(5 * t + 3.5) + 19 / 7 * Sin(6 * t + 8 / 17) + 20 / 7 * Sin(7 * t + 34 / 9) +
                55 / 7 * Sin(8 * t + 1.2) + 64 / 9 * Sin(10 * t + 38 / 9) + 5.4) * AdvancedMath.ThetaFunction(19 * Math.PI - t) * AdvancedMath.ThetaFunction(t - 15 * Math.PI);

                comp10 = (-22 / 7 * Sin(4 / 3 - 8 * t) - 19 / 7 * Sin(20 / 13 - 6 * t) + 38 / 13 * Sin(t + 1 / 24) + 12 / 11 * Sin(2 * t + 5 / 9) + 26 / 7 * Sin(3 * t + 7 / 9) + 2.2 * Sin(4 * t + 12 / 11) +
                3.7 * Sin(5 * t + 1.7) + 5.1 * Sin(7 * t + 10 / 3) + 8.25 * Sin(9 * t + 26 / 7) + 8.2 * Sin(10 * t + 1.8) - 13.5) *
                AdvancedMath.ThetaFunction(15 * Math.PI - t) * AdvancedMath.ThetaFunction(t - 11*Math.PI);

                comp11 = (-14.4 * Sin(0.375 - t) + 1.25 * Sin(2 * t + 3.5) + 2303 / 24) * AdvancedMath.ThetaFunction(11 * Math.PI - t) * AdvancedMath.ThetaFunction(t - 7 * Math.PI);

                comp12 = (88.2 - 455 / 12 * Sin(7 / 9 - t)) * AdvancedMath.ThetaFunction(7 * Math.PI - t) * AdvancedMath.ThetaFunction(t - 3 * Math.PI);

                comp13 = (-1 / 3 * Sin(1 / 20 - 18 * t) - 1.4 * Sin(7 / 9 - 17 * t) - 18 / 11 * Sin(0.4 - 14 * t) -
                4.8 * Sin(1 / 13 - 9 * t) + 2767 / 7 * Sin(t + 11 / 3) + 45.8 * Sin(2 * t + 17 / 7) + 39.125 * Sin(3 * t + 4.4) +
                32 / 3 * Sin(4 * t + 4.4) + 169 / 6 * Sin(5 * t + 2.625) + 23 / 7 * Sin(6 * t + 26 / 11) + 10.5 * Sin(7 * t + 5 / 6) +
                55 / 6 * Sin(8 * t + 2.8) + 212 / 13 * Sin(10 * t + 24 / 7) + 26 / 9 * Sin(11 * t + 4.5) + 3.2 * Sin(12 * t + 25 / 6) + 35 / 17 * Sin(13 * t + 4 / 11) +
                1.875 * Sin(15 * t + 0.7) + 2 / 3 * Sin(16 * t + 20 / 9) + 16 / 7 * Sin(19 * t + 0.8) + 13 / 7 * Sin(20 * t + 29 / 7) +
                14 / 3 * Sin(21 * t + 1.4) + 4 / 3 * Sin(22 * t + 1.75) + 12 / 7 * Sin(23 * t + 34 / 33) + 1.75 * Sin(24 * t + 27 / 7) -
                42.2) * AdvancedMath.ThetaFunction(3 * Math.PI - t) * AdvancedMath.ThetaFunction(t + Math.PI);

                return ((comp1 + comp2 + comp3 + comp4 + comp5 + comp6 + comp7 + comp8 + comp9 + comp10 + comp11 + comp12 + comp13) *
                AdvancedMath.ThetaFunction(Math.Sqrt(Math.Sign(Sin(t / 2)))))/2/* * explosionPlacementRadius / 500*/;
            });

            double Sin(double x)
            {
                return Math.Sin(x);
            }

            return new Tuple<double, double>(xCoord, yCoord);
        }
    }
}
