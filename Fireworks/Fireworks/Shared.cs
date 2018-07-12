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
