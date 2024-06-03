using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace Construct
{
    public static class Extensions
    {
        public static Vector2 Deaugment(this Vector3 vec)
        {
            return new(vec.X, vec.Y);
        }

        public static Vector3 Augment(this Vector2 vec)
        {
            return new(vec.X, vec.Y, 0);
        }

        public static Vector3 Transform(this Vector3 vec, Matrix mat)
        {
            return (Matrix.CreateTranslation(vec) * mat).Translation;
        }

        public static Vector2 Transform(this Vector2 vec, Matrix mat)
        {
            return vec.Augment().Transform(mat).Deaugment();
        }

        public static T Pop<T>(this List<T> values)
        {
            return Pop(values, values.Count - 1);
        }

        public static T Pop<T>(this List<T> values, int index)
        {
            T v = values[index];
            values.RemoveAt(index);
            return v;
        }

        public static Point ToPoint(this Point16 point) => new Point(point.X, point.Y);
    }
}
