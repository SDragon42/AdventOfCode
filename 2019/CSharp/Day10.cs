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
        public Day10(bool benchmark) : base(benchmark) { }

        public override IEnumerable<string> SolvePuzzle()
        {
            yield return "Day 10: Monitoring Station";

            yield return string.Empty;
            //yield return " ex. 1) " + base.Run(() => RunPart1(GetPuzzleData(1, "example1"), out _));
            //yield return " ex. 2) " + base.Run(() => RunPart1(GetPuzzleData(1, "example2"), out _));
            //yield return " ex. 3) " + base.Run(() => RunPart1(GetPuzzleData(1, "example3"), out _));
            //yield return " ex. 4) " + base.Run(() => RunPart1(GetPuzzleData(1, "example4"), out _));
            //yield return " ex. 5) " + base.Run(() => RunPart1(GetPuzzleData(1, "example5"), out _));
            var baseLocation = default(Point);
            yield return "Part 1) " + base.Run(() => RunPart1(GetPuzzleData(1, "input"), out baseLocation));

            yield return string.Empty;
            //yield return "Part 2) " + base.Run(() => RunPart2(GetPuzzleData(2, "example5"), new Point(11, 13), 200));
            yield return "Part 2) " + base.Run(() => RunPart2(GetPuzzleData(2, "input"), baseLocation, 200));
        }


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
            const int DAY = 10;

            var result = part switch
            {
                1 => new InputAnswer(
                    InputHelper.LoadInputFile(DAY, name).Split("\r\n"),
                    InputHelper.LoadAnswerFile(DAY, part, name).ToInt32()
                    ),
                2 => new InputAnswer(
                    InputHelper.LoadInputFile(DAY, name).Split("\r\n"),
                    InputHelper.LoadAnswerFile(DAY, part, name).ToInt32()
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
            var sb = new StringBuilder();

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
                    sb.AppendLine($"{vaporizeCount}th vaporized at ({target.asteroid.Location.X},{target.asteroid.Location.Y})");
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

            
            var resultMessage = Helper.GetPuzzleResultText($"Winning Bet: {foundBet}",
                foundBet,
                puzzleData.ExpectedAnswer);

            sb.AppendLine(resultMessage);
            return sb.ToString();
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
