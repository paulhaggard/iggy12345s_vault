using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fireworks
{
    public static class PhysConstants
    {
        public static Vector2D Gravity = new Vector2D(0, 0.2);
    }

    public static class AdvancedMath
    {

        /// <summary>
        /// Returns the theta chebyshev function theta(x)=Sum(Ln(Pk)) for
        /// k = 1 to pi(x)
        /// Pk = the kth prime number
        /// </summary>
        /// <param name="x"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static double ThetaFunction(double x)
        {
            return Math.Log(Primorial(x));
        }

        /// <summary>
        /// Finds the product of all prime numbers less than x
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int Primorial(double x)
        {
            int product = 1;
            for (int i = 1; i < PrimeCountingFunction((int)x); i++)
                product *= kPrime(i);
            return (x < 1) ? 0 : product;
        }

        /// <summary>
        /// Returns the number of prime numbers that are less than x
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int PrimeCountingFunction(int x)
        {
            int sum = -1;
            for (int j = 3; j < x; j++)
            {
                int Fact = Factorial(j - 2);
                sum += Fact - j * (int)Math.Floor((double)(Fact / j));
            }
            return (x <= 1) ? 0 : (x == 2) ? 1 : sum;
        }

        /// <summary>
        /// Returns the factorial of some non-negative integer x
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int Factorial(int x)
        {
            int product = x;
            for (int i = x - 1; i > 0; i--)
                product *= i;
            return (x == 0) ? 1 : product;
        }

        /// <summary>
        /// Returns the kth Prime number
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int kPrime(int n)
        {
            return (n == 1) ? 1 : (n == 2) ? 2 : ((n - 2) * 2 + 1);
        }
    }

    public class Vector2D
    {
        #region Properties

        private double x;
        private double y;

        #endregion

        #region Constructors

        public Vector2D(double x = 0, double y = 0)
        {
            this.x = x;
            this.y = y;
        }

        #endregion

        #region Accessor Methods

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public double Magnitude { get => Math.Sqrt(x * x + y * y); }
        public double Angle { get => Math.Atan(y / x); }

        #endregion

        #region Operator Methods

        public static Vector2D operator +(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.x + b.x, a.y + b.y);
        }

        public static Vector2D operator -(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.x - b.x, a.y - b.y);
        }

        public static Vector2D operator *(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.x * b.x, a.y * b.y);
        }

        public static Vector2D operator /(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.x / b.x, a.y / b.y);
        }

        public static Vector2D operator +(Vector2D a, double b)
        {
            return new Vector2D(a.x + b, a.y + b);
        }

        public static Vector2D operator -(Vector2D a, double b)
        {
            return new Vector2D(a.x - b, a.y - b);
        }

        public static Vector2D operator -(double a, Vector2D b)
        {
            return new Vector2D(a - b.x, a - b.y);
        }

        public static Vector2D operator *(Vector2D a, double b)
        {
            return new Vector2D(a.x * b, a.y * b);
        }

        public static Vector2D operator /(Vector2D a, double b)
        {
            return new Vector2D(a.x / b, a.y / b);
        }

        public static Vector2D operator /(double a, Vector2D b)
        {
            return new Vector2D(a / b.x, a / b.y);
        }

        #endregion
    }
}
