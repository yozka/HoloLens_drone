using System;
using Urho;


namespace Utils
{
    public class Mathf
    {
        public const float Deg2Rad = (float)(Math.PI / 180.0);
        public const float Rad2Deg = (float)(180.0 / Math.PI);
    }


    public static class MathHelperExt
    {
        public static double Clamp(double value, double min, double max)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            return value;
        }


        public static Vector2 intersect(Vector2 pos)
        {
            Vector2 div = new Vector2(0.5f, 0.5f);
            Vector2 b = pos - div;
            Vector2 pt = Vector2.Zero;

            //стороны правая
            if (b.X > 0 && b.Y > -0.5f && b.Y < 0.5 && b.X > b.Y)
            {
                pt.X = 0.5f;
                pt.Y = 0.5f * b.Y / b.X;
            }
            else
            //пересечение слевой стороны
            if (b.X < 0 && b.Y > -0.5f && b.Y < 0.5 && b.X > b.Y)
            {
                pt.X = -0.5f;
                pt.Y = -0.5f * b.Y / b.X;
            }
            else
            //пересечение сверху
            if (b.X > 0)
            {
                pt.X = 0.5f * b.X / b.Y;
                pt.Y = 0.5f;
            }
            else
            //пересечение снизу
            if (b.X > 0)
            {
                pt.X = -0.5f * b.X / b.Y;
                pt.Y = -0.5f;
            }

            pt += div;

            return pt;
        }
    }
}
