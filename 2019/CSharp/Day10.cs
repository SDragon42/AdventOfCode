using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.CSharp.Common;

namespace AdventOfCode.CSharp.Year2019
{
    /// <summary>
    /// https://adventofcode.com/2019/day/10
    /// </summary>
    class Day10 : PuzzleBase
    {
        const int DAY = 10;

        
        public override IEnumerable<string> SolvePuzzle()
        {
            yield return "Day 10: Monitoring Station";

            yield return string.Empty;
            yield return RunExample(Example1);
            yield return RunExample(Example2);
            yield return RunExample(Example3);
            yield return RunExample(Example4);
            yield return RunExample(Example5);
            var baseLocation = default(Point);
            yield return Run(() => Part1(out baseLocation));

            yield return string.Empty;
            yield return RunExample(Example5P2);
            yield return Run(() => Part2(baseLocation));
        }

        string Example1() => " Ex. 1) " + RunPart1(GetPuzzleData(1, "example1"), out _);
        string Example2() => " Ex. 2) " + RunPart1(GetPuzzleData(1, "example2"), out _);
        string Example3() => " Ex. 3) " + RunPart1(GetPuzzleData(1, "example3"), out _);
        string Example4() => " Ex. 4) " + RunPart1(GetPuzzleData(1, "example4"), out _);
        string Example5() => " Ex. 5) " + RunPart1(GetPuzzleData(1, "example5"), out _);
        string Part1(out Point baseLocation) => "Part 1) " + RunPart1(GetPuzzleData(1, "input"), out baseLocation);

        string Example5P2() => " Ex. 6) " + RunPart2(GetPuzzleData(2, "example5"), new Point(11, 13), 200);
        string Part2(Point baseLocation) => "Part 2) " + RunPart2(GetPuzzleData(2, "input"), baseLocation, 200);



        class InputAnswer : InputAnswer<string[], int?>
        {
            public InputAnswer(string[] input, int? expectedAnswer = null, Point? fountPosition = null)
            {
                Input = input;
                ExpectedAnswer = expectedAnswer;

                BaseLocation = default(Point);
                MapData = new Dictionary<Point, AsteroidInfo>();

                var y = 0;
                foreach (var line in Input)
                {
                    for (var x = 0; x < line.Length; x++)
                    {
                        if (line[x] == '.')
                            continue;
                        var z = new AsteroidInfo(x, y);
                        MapData.Add(z.Location, z);
                        if (line[x] == 'X')
                            BaseLocation = z.Location;
                    }
                    y++;
                }
            }

            public Dictionary<Point, AsteroidInfo> MapData { get; private set; }
            public Point BaseLocation { get; private set; }
        }
        InputAnswer GetPuzzleData(int part, string name)
        {
            var result = part switch
            {
                1 => new InputAnswer(
                    InputHelper.LoadInputFile(DAY, name).ToArray(),
                    InputHelper.LoadAnswerFile(DAY, part, name)?.FirstOrDefault()?.ToInt32()
                    ),
                2 => new InputAnswer(
                    InputHelper.LoadInputFile(DAY, name).ToArray(),
                    InputHelper.LoadAnswerFile(DAY, part, name)?.FirstOrDefault()?.ToInt32()
                    ),
                _ => throw new ApplicationException($"Invalid part ({part}) value")
            };
            return result;
        }



        


        string RunPart1(InputAnswer puzzleData, out Point baseLocation)
        {
            foreach (var prospect in puzzleData.MapData.Keys)
                ScanFrom(puzzleData.MapData[prospect], puzzleData.MapData);

            var best = puzzleData.MapData.Keys
                .Select(k => puzzleData.MapData[k])
                .OrderByDescending(m => m.NumberCanSee)
                .First();

            baseLocation = best.Location;

            return Helper.GetPuzzleResultText($"# asteroids that can be detected from ({best.Location.X},{best.Location.Y})?: {best.NumberCanSee}",
                best.NumberCanSee,
                puzzleData.ExpectedAnswer);
        }

        string RunPart2(InputAnswer puzzleData, Point baseLocation, int vaporizeNumber)
        {
            var vaporizeCount = 0;
            var foundBet = 0;

            var allAsteroids = puzzleData.MapData.Values
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
            while (allAsteroids.Count() > 0)
            {
                var target = allAsteroids
                    .Where(z => z.angle == angle)
                    .OrderBy(z => z.distance)
                    .First();

                vaporizeCount++;
                puzzleData.MapData.Remove(target.asteroid.Location);

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

            
            return Helper.GetPuzzleResultText($"Winning Bet: {foundBet}",
                foundBet,
                puzzleData.ExpectedAnswer);
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

            public Point Location { get; init; }
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
