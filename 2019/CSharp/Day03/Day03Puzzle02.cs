using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AdventOfCode.CSharp.Year2019.Day03
{
    /*
    It turns out that this circuit is very timing-sensitive; you actually need 
    to minimize the signal delay.

    To do this, calculate the number of steps each wire takes to reach each 
    intersection; choose the intersection where the sum of both wires' steps is 
    lowest. If a wire visits a position on the grid multiple times, use the 
    steps value from the first time it visits that position when calculating 
    the total value of a specific intersection.

    The number of steps a wire takes is the total number of grid squares the 
    wire has entered to get to that location, including the intersection being 
    considered. Again consider the example from above:

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
    In the above example, the intersection closest to the central port is 
    reached after 8+5+5+2 = 20 steps by the first wire and 7+6+4+3 = 20 steps 
    by the second wire for a total of 20+20 = 40 steps.

    However, the top-right intersection is better: the first wire takes only 
    8+5+2 = 15 and the second wire takes only 7+6+2 = 15, a total of 15+15 = 30 
    steps.

    Here are the best steps for the extra examples from above:

    - R75,D30,R83,U83,L12,D49,R71,U7,L72
      U62,R66,U55,R34,D71,R55,D58,R83 = 610 steps
    - R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51
      U98,R91,D20,R16,D67,R40,U7,R15,U6,R7 = 410 steps

    What is the fewest combined steps the wires must take to reach an intersection? 
    */
    class Day03Puzzle02 : IPuzzle
    {
        public void Run()
        {
            Console.WriteLine("--- Day 3: Crossed Wires (Part 2) ---");

            var line1Segments = BuildLineSegments(Day03Common.Line1Commands).ToList();
            var line2Segments = BuildLineSegments(Day03Common.Line2Commands).ToList();


            var intersectionPoints = new List<PointSteps>();
            var line1Length = 0;
            foreach (var line1 in line1Segments)
            {
                var line2Length = 0;
                foreach (var line2 in line2Segments)
                {
                    var pos = FindIntersection(line1, line2);
                    if (pos != null)
                    {
                        var length = line1Length + line2Length + CalcDistance(line1.P1, pos.Value) + CalcDistance(line2.P1, pos.Value);
                        var item = new PointSteps(pos.Value, length);
                        intersectionPoints.Add(item);
                    }

                    line2Length += CalcDistance(line2.P1, line2.P2);
                }
                line1Length += CalcDistance(line1.P1, line1.P2);
            }

            var minDist = 0;
            minDist = intersectionPoints
                .Where(p => p.IntersectPoint != Point.Empty)
                .Distinct()
                .Select(p => p.Distance)
                .Min();

            Console.WriteLine($"Closest distance: {minDist}");
            if (Day03Common.DesiredWireDistance >= 0)
                Console.WriteLine("    " + (Day03Common.DesiredWireDistance == minDist ? "CORRECT" : "You done it wrong!"));
            Console.WriteLine();
        }

        private int CalcDistance(Point p1, Point p2) => Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);

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

        class PointSteps
        {
            public PointSteps(Point intersectPoint, int distance)
            {
                IntersectPoint = intersectPoint;
                Distance = distance;
            }

            public Point IntersectPoint { get; private set; }
            public int Distance { get; private set; }
        }

    }
}
