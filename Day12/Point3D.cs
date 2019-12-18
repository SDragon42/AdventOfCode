using System;
using System.Collections.Generic;
using System.Text;

namespace Advent_of_Code.Day12
{
    class Point3D
    {
        public Point3D() : this(0, 0, 0) { }
        public Point3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public int Energy => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);

        public override bool Equals(object obj)
        {
            var other = obj as Point3D;
            if (other == null) return false;
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        public override string ToString()
        {
            return $"<x={X,5}, y={Y,5}, z={Z,5}>";
        }

        public Point3D GetCopy()
        {
            var item = new Point3D(X, Y, Z);
            return item;
        }
    }
}
