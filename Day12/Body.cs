using System;
using System.Collections.Generic;
using System.Text;

namespace Advent_of_Code.Day12
{
    class Body
    {
        public Body(int x, int y, int z)
        {
            Position = new Point3D(x, y, z);
            Velocity = new Point3D();
        }

        public Point3D Position { get; private set; }
        public Point3D Velocity { get; private set; }

        public int KineticEnergy => Velocity.Energy;
        public int PotentialEnergy => Position.Energy;
        public int TotalEnergy => PotentialEnergy * KineticEnergy;

        public override bool Equals(object obj)
        {
            var other = obj as Body;
            if (other == null) return false;
            return Position.Equals(other.Position) && Velocity.Equals(other.Velocity);
        }
        public override int GetHashCode()
        {
            return Position.GetHashCode() ^ Velocity.GetHashCode();
        }

        public override string ToString()
        {
            return $"pos={Position}, vel={Velocity}";
        }

    }

}
