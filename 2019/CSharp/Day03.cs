﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.CSharp.Year2019
{
    /// <summary>
    /// https://adventofcode.com/2019/day/3
    /// </summary>
    public class Day03 : TestBase
    {
        public Day03(ITestOutputHelper output) : base(output, 3) { }


        private readonly Point origin = new Point(0, 0);

        private (List<string>, int?) GetTestData(string name, int part)
        {
            var input = Input.ReadLines(DAY, name)
                .ToList();

            var expected = Input.ReadLines(DAY, $"{name}-answer{part}")
                ?.FirstOrDefault()
                ?.ToInt32();

            return (input, expected);
        }

        [Theory]
        [InlineData("example1")]
        [InlineData("example2")]
        [InlineData("example3")]
        [InlineData("input")]
        public void Part1(string inputName)
        {
            var (input, expected) = GetTestData(inputName, 1);

            var wire1 = BuildWirePoints(input[0], 0, 0);
            var wire2 = BuildWirePoints(input[1], 0, 0);
            var intersctions = FindIntersections(wire1, wire2)
                .Where(p => p != origin);
            var answer = intersctions
                .Select(p => CalcManhattenDistance(origin, p))
                .Min();

            output.WriteLine($"Closest distance : {answer}");

            Assert.Equal(expected, answer);
        }

        [Theory]
        [InlineData("example4")]
        [InlineData("example5")]
        [InlineData("input")]
        public void Part2(string inputName)
        {
            var (input, expected) = GetTestData(inputName, 2);

            var wire1 = BuildWirePoints(input[0], 0, 0);
            var wire2 = BuildWirePoints(input[1], 0, 0);
            var origin = new Point(0, 0);
            var intersctions = FindIntersectionsWithDistance(wire1, wire2)
                .Where(p => p.IntersectPoint != origin);
            var answer = intersctions
                .Select(p => p.Distance)
                .Min();

            output.WriteLine($"The fewest combined steps is : {answer}");

            Assert.Equal(expected, answer);
        }


        List<Point> BuildWirePoints(string input, int x, int y)
        {
            var wire = new List<Point>();
            wire.Add(new Point(x, y));

            var _directions = input.Split(',');
            foreach (var item in _directions)
            {
                var t = item.Substring(1);
                var dist = Convert.ToInt32(t);
                switch (item[0])
                {
                    case 'R': x += dist; break;
                    case 'L': x -= dist; break;
                    case 'U': y += dist; break;
                    case 'D': y -= dist; break;
                }
                wire.Add(new Point(x, y));
            }

            return wire;
        }



        IEnumerable<Point> FindIntersections(List<Point> wire1, List<Point> wire2)
        {
            for (var l1Idx = 1; l1Idx < wire1.Count; l1Idx++)
            {
                for (var l2Idx = 1; l2Idx < wire2.Count; l2Idx++)
                {
                    var line1 = new LineInfo(wire1[l1Idx - 1], wire1[l1Idx]);
                    var line2 = new LineInfo(wire2[l2Idx - 1], wire2[l2Idx]);

                    var p = FindIntersection(line1, line2);
                    if (p.HasValue)
                        yield return p.Value;
                }
            }
        }
        IEnumerable<PointDistance> FindIntersectionsWithDistance(List<Point> wire1, List<Point> wire2)
        {
            var line1Distance = 0;
            for (var l1Idx = 1; l1Idx < wire1.Count; l1Idx++)
            {
                var line1 = new LineInfo(wire1[l1Idx - 1], wire1[l1Idx]);

                var line2Distance = 0;
                for (var l2Idx = 1; l2Idx < wire2.Count; l2Idx++)
                {
                    var line2 = new LineInfo(wire2[l2Idx - 1], wire2[l2Idx]);

                    var p = FindIntersection(line1, line2);
                    if (p.HasValue)
                    {
                        var totalDistance = line1Distance + line2Distance
                            + CalcManhattenDistance(line1.P1, p.Value)
                            + CalcManhattenDistance(line2.P1, p.Value);
                        yield return new PointDistance(p.Value, totalDistance);
                    }

                    line2Distance += CalcManhattenDistance(line2.P1, line2.P2);
                }
                line1Distance += CalcManhattenDistance(line1.P1, line1.P2);
            }
        }

        Point? FindIntersection(LineInfo line1, LineInfo line2)
        {
            var delta = (line1.A * line2.B) - (line2.A * line1.B);
            if (delta == 0)
                return null;

            var newX = ((line2.B * line1.C) - (line1.B * line2.C)) / delta;
            var newY = ((line1.A * line2.C) - (line2.A * line1.C)) / delta;

            var intersectPoint = new Point(newX, newY);

            if (!IsOnLine(line1, intersectPoint))
                return null;
            if (!IsOnLine(line2, intersectPoint))
                return null;

            return intersectPoint;
        }
        bool IsOnLine(LineInfo line, Point intersect)
        {
            if (IsSame(line.P1.X, line.P2.X, intersect.X))
                return IsBetween(line.P1.Y, line.P2.Y, intersect.Y);
            else if (IsSame(line.P1.Y, line.P2.Y, intersect.Y))
                return IsBetween(line.P1.X, line.P2.X, intersect.X);
            return false;
        }
        bool IsSame(int a, int b, int value)
        {
            return (a == b && b == value);
        }
        bool IsBetween(int a, int b, int value)
        {
            return (a <= b)
                ? (a <= value && value <= b)
                : (a >= value && value >= b);
        }

        int CalcManhattenDistance(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }



        class LineInfo
        {
            public LineInfo(Point p1, Point p2)
            {
                P1 = p1;
                P2 = p2;

                A = p2.Y - p1.Y;
                B = p2.X - p1.X;
                C = (A * p1.X) + (B * p1.Y);
            }

            public Point P1 { get; private set; }
            public Point P2 { get; private set; }

            public int A { get; private set; }
            public int B { get; private set; }
            public int C { get; private set; }
        }


        class PointDistance
        {
            public PointDistance(Point intersectPoint, int distance)
            {
                IntersectPoint = intersectPoint;
                Distance = distance;
            }

            public Point IntersectPoint { get; private set; }
            public int Distance { get; private set; }
        }

    }
}
