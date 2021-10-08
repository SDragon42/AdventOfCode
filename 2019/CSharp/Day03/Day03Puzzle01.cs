using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AdventOfCode.CSharp.Year2019.Day03
{
    /*
    --- Day 3: Crossed Wires ---
    The gravity assist was successful, and you're well on your way to the Venus 
    refuelling station. During the rush back on Earth, the fuel management 
    system wasn't completely installed, so that's next on the priority list.

    Opening the front panel reveals a jumble of wires. Specifically, two wires 
    are connected to a central port and extend outward on a grid. You trace the 
    path each wire takes as it leaves the central port, one wire per line of 
    text (your puzzle input).

    The wires twist and turn, but the two wires occasionally cross paths. To 
    fix the circuit, you need to find the intersection point closest to the 
    central port. Because the wires are on a grid, use the Manhattan distance 
    for this measurement. While the wires do technically cross right at the 
    central port where they both start, this point does not count, nor does a 
    wire count as crossing with itself.

    For example, if the first wire's path is R8,U5,L5,D3, then starting from 
    the central port (o), it goes right 8, up 5, left 5, and finally down 3:

    ...........
    ...........
    ...........
    ....+----+.
    ....|....|.
    ....|....|.
    ....|....|.
    .........|.
    .o-------+.
    ...........

    Then, if the second wire's path is U7,R6,D4,L4, it goes up 7, right 6, down 
    4, and left 4:

    ...........
    .+-----+...
    .|.....|...
    .|..+--X-+.
    .|..|..|.|.
    .|.-X--+.|.
    .|..|....|.
    .|.......|.
    .o-------+.
    ...........
    These wires cross at two locations (marked X), but the lower-left one is 
    closer to the central port: its distance is 3 + 3 = 6.

    These wires cross at two locations (marked X), but the lower-left one is 
    closer to the central port: its distance is 3 + 3 = 6.

    Here are a few more examples:

    - R75,D30,R83,U83,L12,D49,R71,U7,L72
      U62,R66,U55,R34,D71,R55,D58,R83 = distance 159
    - R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51
      U98,R91,D20,R16,D67,R40,U7,R15,U6,R7 = distance 135

    What is the Manhattan distance from the central port to the closest 
    intersection?
    */
    class Day03Puzzle01b : IPuzzle
    {
        public void Run()
        {
            Console.WriteLine("--- Day 3: Crossed Wires ---");

            var line1Segments = BuildLineSegments(Day03Common.Line1Commands).ToList();
            var line2Segments = BuildLineSegments(Day03Common.Line2Commands).ToList();


            var intersectionPoints = new List<Point>();
            foreach (var line1 in line1Segments)
            {
                foreach (var line2 in line2Segments)
                {
                    var pos = FindIntersection(line1, line2);
                    if (pos != null)
                        intersectionPoints.Add(pos.Value);
                }
            }

            var minDist = 0;
            minDist = intersectionPoints
                .Where(p => p != Point.Empty)
                .Distinct()
                .Select(CalcManhattanDistance)
                .Min();

            Console.WriteLine($"Closest distance: {minDist}");
            if (Day03Common.DesiredClosestIntersectDistance >= 0)
                Console.WriteLine("    " + (Day03Common.DesiredClosestIntersectDistance == minDist ? "CORRECT" : "You done it wrong!"));
            Console.WriteLine();
        }

        private int CalcManhattanDistance(Point p) => Math.Abs(p.X) + Math.Abs(p.Y);

        IEnumerable<PointPair> BuildLineSegments(IEnumerable<string> commands)
        {
            var startPoint = new Point(0, 0);

            foreach (var cmd in commands)
            {
                var dir = cmd[0];
                var dist = Convert.ToInt32(cmd.Substring(1));

                var endPoint = default(Point);
                switch (dir)
                {
                    case 'U': endPoint = new Point(startPoint.X, startPoint.Y + dist); break;
                    case 'D': endPoint = new Point(startPoint.X, startPoint.Y - dist); break;
                    case 'L': endPoint = new Point(startPoint.X - dist, startPoint.Y); break;
                    case 'R': endPoint = new Point(startPoint.X + dist, startPoint.Y); break;
                }

                yield return new PointPair(startPoint, endPoint);
                startPoint = endPoint;
            }

        }

        class PointPair
        {
            public PointPair(Point p1, Point p2)
            {
                P1 = p1;
                P2 = p2;

                A = P2.Y - P1.Y;
                B = P2.X - P1.X;
                C = (A * P1.X) + (B * P1.Y);
            }

            public Point P1 { get; private set; }
            public Point P2 { get; private set; }

            public int A { get; private set; }
            public int B { get; private set; }
            public int C { get; private set; }
        }

        private Point? FindIntersection(PointPair line1, PointPair line2)
        {
            var delta = (line1.A * line2.B) - (line2.A * line1.B);
            if (delta == 0)
                return null;

            var newX = ((line2.B * line1.C) - (line1.B * line2.C)) / delta;
            var newY = ((line1.A * line2.C) - (line2.A * line1.C)) / delta;

            var intersect = new Point(newX, newY);

            if (!IsOnLine(line1, intersect))
                return null;
            if (!IsOnLine(line2, intersect))
                return null;

            return intersect;
        }

        private bool IsOnLine(PointPair line, Point intersect)
        {
            if (IsSame(line.P1.X, line.P2.X, intersect.X))
                return IsBetween(line.P1.Y, line.P2.Y, intersect.Y);
            else if (IsSame(line.P1.Y, line.P2.Y, intersect.Y))
                return IsBetween(line.P1.X, line.P2.X, intersect.X);
            return false;
        }
        private bool IsSame(int a, int b, int value)
        {
            return (a == b && b == value);
        }
        private bool IsBetween(int a, int b, int value)
        {
            return (a <= b)
                ? (a <= value && value <= b)
                : (a >= value && value >= b);
        }

    }

}
