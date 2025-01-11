using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.CSharp.Year2019
{
    /// <summary>
    /// https://adventofcode.com/2019/day/10
    /// </summary>
    public class Day10 : TestBase
    {
        public Day10(ITestOutputHelper output) : base(output, 10) { }


        private (Dictionary<Point, AsteroidInfo>, Point, int?) GetTestData(string name, int part)
        {
            var input = Input.ReadLines(DAY, name)
                .ToList();

            var expected = Input.ReadLines(DAY, $"{name}-answer{part}")
                ?.FirstOrDefault()
                ?.ToInt32();


            var baseLocation = default(Point);
            var mapData = new Dictionary<Point, AsteroidInfo>();

            var y = 0;
            foreach (var line in input)
            {
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '.')
                        continue;
                    var z = new AsteroidInfo(x, y);
                    mapData.Add(z.Location, z);
                    if (line[x] == 'X')
                        baseLocation = z.Location;
                }
                y++;
            }

            return (mapData, baseLocation, expected);
        }


        [Theory]
        [InlineData("example1")]
        [InlineData("example2")]
        [InlineData("example3")]
        [InlineData("example4")]
        [InlineData("example5")]
        [InlineData("input")]
        public void Part1(string inputName)
        {
            var (mapData, _, expected) = GetTestData(inputName, 1);

            var result = RunPart1(mapData);

            Assert.Equal(expected, result.NumberCanSee);
        }


        public static IEnumerable<object[]> Part2TheoryInputs()
        {
            yield return new object[] { "example5", 200, new Point(11, 13) };
            yield return new object[] { "input", 200, null };
        }

        [Theory]
        [MemberData(nameof(Part2TheoryInputs))]
        public void Part2(string inputName, int vaporizeNumber, Point? baseLocation)
        {
            var (mapData, _, expected) = GetTestData(inputName, 2);

            if (!baseLocation.HasValue)
            {
                var aa = RunPart1(mapData);
                baseLocation = aa.Location;
            }

            var result = RunPart2(mapData, baseLocation.Value, vaporizeNumber);

            Assert.Equal(expected, result);
        }




        AsteroidInfo RunPart1(Dictionary<Point, AsteroidInfo> mapData)
        {
            foreach (var prospect in mapData.Keys)
                ScanFrom(mapData[prospect], mapData);

            var best = mapData.Keys
                .Select(k => mapData[k])
                .OrderByDescending(m => m.NumberCanSee)
                .First();
            return best;
        }

        int RunPart2(Dictionary<Point, AsteroidInfo> mapData, Point baseLocation, int vaporizeNumber)
        {
            var vaporizeCount = 0;
            var foundBet = 0;

            var allAsteroids = mapData.Values
                .Where(z => z.Location != baseLocation)
                .Select(z => new
                {
                    asteroid = z,
                    angle = CalcAngle(baseLocation, z.Location),
                    distance = CalcDistance(baseLocation, z.Location)
                })
                .OrderBy(z => z.angle)
                .ThenBy(z => z.distance);

            var angle = allAsteroids.Select(z => z.angle).Min();
            while (allAsteroids.Any())
            {
                var target = allAsteroids
                    .Where(z => z.angle == angle)
                    .OrderBy(z => z.distance)
                    .First();

                vaporizeCount++;
                mapData.Remove(target.asteroid.Location);

                if (vaporizeCount == vaporizeNumber)
                {
                    foundBet = (target.asteroid.Location.X * 100) + target.asteroid.Location.Y;
                    break;
                }

                var nextTarget = allAsteroids
                    .Where(z => z.angle > angle)
                    .FirstOrDefault();

                angle = (nextTarget != null)
                    ? nextTarget.angle
                    : allAsteroids.Select(z => z.angle).Min();
            }

            return foundBet;
        }


        void ScanFrom(AsteroidInfo prospect, Dictionary<Point, AsteroidInfo> mapData)
        {
            var toScan = mapData.Keys
                .Where(k => k != prospect.Location)
                .ToList();

            var count = 0;
            foreach (var target in toScan)
            {
                var canSee = toScan
                    .Where(k => k != target)
                    .All(k => !IsOnLine(prospect.Location, target, k));
                if (canSee)
                    count++;
            }
            prospect.NumberCanSee = count;
        }

        /// <summary>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        /// <remarks>https://stackoverflow.com/a/17590923/6136</remarks>
        bool IsOnLine(Point a, Point b, Point p)
        {
            var ab = Calc(a, b);
            var ap = Calc(a, p);
            var pb = Calc(p, b);

            var apb = ap + pb;
            ab = Math.Round(ab, 5);
            apb = Math.Round(apb, 5);
            var result = ab == apb;
            return result;

            double Calc(Point q, Point w) => Math.Sqrt(Math.Pow(w.X - q.X, 2) + Math.Pow(w.Y - q.Y, 2));
        }

        /// <summary>
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        /// <remarks>https://math.stackexchange.com/a/714423</remarks>
        double CalcAngle(Point p1, Point p2)
        {
            var y1 = (p1.Y > p2.Y) ? p1.Y : p2.Y;
            var y2 = (p1.Y > p2.Y) ? p2.Y : p1.Y;

            var numerator = y1 - y2;
            var denominator = Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(y1 - y2, 2));

            var angle = Math.Abs(Math.Acos(numerator / denominator) * (180 / Math.PI));

            var deltaX = p2.X - p1.X;
            var deltaY = p1.Y - p2.Y;

            angle =
                  (deltaX >= 0 && deltaY >= 0) ? angle
                : (deltaX >= 0 && deltaY <= 0) ? (180 - angle)
                : (deltaX <= 0 && deltaY <= 0) ? (180 + angle)
                : (360 - angle);

            angle = Math.Round(angle, 5); // Rounding to 5 decimal places to get correct output in example
            return angle;
        }

        int CalcDistance(Point p1, Point p2)
        {
            return Math.Abs(p2.X - p1.X) + Math.Abs(p2.Y - p1.Y);
        }



        class AsteroidInfo
        {
            public AsteroidInfo(int x, int y)
            {
                Location = new Point(x, y);
            }

            public Point Location { get; private set; }
            public int NumberCanSee { get; set; } = 0;


            public override bool Equals(object obj)
            {
                return Equals(obj as AsteroidInfo);
            }
            public bool Equals(AsteroidInfo other)
            {
                if (other == null)
                    return false;
                return Location.Equals(other.Location);
            }

            public override int GetHashCode()
            {
                return Location.GetHashCode();
            }

        }


        class MapBounds
        {
            public MapBounds(Dictionary<Point, AsteroidInfo> map)
            {
                LowX = map.Keys.Select(k => k.X).Min();
                LowY = map.Keys.Select(k => k.Y).Min();
                HighX = map.Keys.Select(k => k.X).Max();
                HighY = map.Keys.Select(k => k.Y).Max();
            }


            public int LowX { get; private set; }
            public int LowY { get; private set; }
            public int HighX { get; private set; }
            public int HighY { get; private set; }

        }
    }
}
