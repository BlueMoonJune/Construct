﻿using Microsoft.Xna.Framework;

namespace Construct
{
	public struct Direction
    {
        public static Direction Left => new(-1, 0);
        public static Direction Right => new(1, 0);
        public static Direction Up => new(0, -1);
        public static Direction Down => new(0, 1);

        public Point point {
            get => num switch
            {
                0 => new(1, 0),
                1 => new(0, 1),
                2 => new(-1, 0),
                3 => new(0, -1)
            };
        }
        public int num;


        public static Direction[] directions()
        {
            return new Direction[4] { new Direction(0), new Direction(1), new Direction(2), new Direction(3) };
        }

        public Direction(Point p)
        {

            if (p.X == 1) num = 0;
            if (p.X == -1) num = 2;
            if (p.Y == 1) num = 1;
            if (p.Y == -1) num = 3;

        }

        public static implicit operator Direction(Point p) => new(p);
        public static implicit operator Point(Direction d) => d.point;

        public Direction(int x, int y) : this(new Point(x, y)) { }

        public Direction(int n)
        {
            num = n % 4;
        }

        public static implicit operator int(Direction d) => d.num;
        public static implicit operator Direction(int n) => new(n);

		public static implicit operator Vector2(Direction d) => new(d.point.X, d.point.Y);

        public void Rotate(int steps)
        {
            Direction newDir = new Direction((num + steps) % 4);
            num = newDir.num;
        }

        public Direction Rotated(int steps) => new((num + steps) % 4);
    }
}
