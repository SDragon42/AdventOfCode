using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Advent_of_Code.Day09
{
    /*
    --- Part Two ---
    Once you give them the coordinates, the Elves quickly deploy an Instant Monitoring Station to the location and discover the worst: there are simply too many asteroids.

    The only solution is complete vaporization by giant laser.

    Fortunately, in addition to an asteroid scanner, the new monitoring station also comes equipped with a giant rotating laser perfect for vaporizing asteroids. The laser starts by pointing up and always rotates clockwise, vaporizing any asteroid it hits.

    If multiple asteroids are exactly in line with the station, the laser only has enough power to vaporize one of them before continuing its rotation. In other words, the same asteroids that can be detected can be vaporized, but if vaporizing one asteroid makes another one detectable, the newly-detected asteroid won't be vaporized until the laser has returned to the same position by rotating a full 360 degrees.

    For example, consider the following map, where the asteroid with the new monitoring station (and laser) is marked X:

    .#....#####...#..
    ##...##.#####..##
    ##...#...#.#####.
    ..#.....X...###..
    ..#.#.....#....##
    The first nine asteroids to get vaporized, in order, would be:

    .#....###24...#..
    ##...##.13#67..9#
    ##...#...5.8####.
    ..#.....X...###..
    ..#.#.....#....##
    Note that some asteroids (the ones behind the asteroids marked 1, 5, and 7) won't have a chance to be vaporized until the next full rotation. The laser continues rotating; the next nine to be vaporized are:

    .#....###.....#..
    ##...##...#.....#
    ##...#......1234.
    ..#.....X...5##..
    ..#.9.....8....76
    The next nine to be vaporized are then:

    .8....###.....#..
    56...9#...#.....#
    34...7...........
    ..2.....X....##..
    ..1..............
    Finally, the laser completes its first full rotation (1 through 3), a second rotation (4 through 8), and vaporizes the last asteroid (9) partway through its third rotation:

    ......234.....6..
    ......1...5.....7
    .................
    ........X....89..
    .................
    In the large example above (the one with the best monitoring station location at 11,13):

    The 1st asteroid to be vaporized is at 11,12.
    The 2nd asteroid to be vaporized is at 12,1.
    The 3rd asteroid to be vaporized is at 12,2.
    The 10th asteroid to be vaporized is at 12,8.
    The 20th asteroid to be vaporized is at 16,0.
    The 50th asteroid to be vaporized is at 16,9.
    The 100th asteroid to be vaporized is at 10,16.
    The 199th asteroid to be vaporized is at 9,6.
    The 200th asteroid to be vaporized is at 8,2.
    The 201st asteroid to be vaporized is at 10,9.
    The 299th and final asteroid to be vaporized is at 11,1.
    The Elves are placing bets on which will be the 200th asteroid to be vaporized. Win the bet by determining which asteroid that will be; what do you get if you multiply its X coordinate by 100 and then add its Y coordinate? (For example, 8,2 becomes 802.)
     */
    class Day10Puzzle2 : IPuzzle
    {


        public Day10Puzzle2()
        {
            string[] rawMapData;
            var zoneList = new List<MapZone>();

            //rawMapData = Helper.GetFileContentAsLines("D10P2-Test1.txt");
            //Width = rawMapData[0].Length;
            //Height = rawMapData.Length;
            //BuildData(rawMapData, zoneList, ref Origin);
            //WinningBet = CalcBet(8, 2); // 200th asteroid

            //rawMapData = Helper.GetFileContentAsLines("D10P1-Test5.txt");
            //Width = rawMapData[0].Length;
            //Height = rawMapData.Length;
            //BuildData(rawMapData, zoneList, ref Origin);
            //if (Origin == null)
            //    Origin = zoneList.Where(z => z.Location.X == 11 && z.Location.Y == 13).FirstOrDefault();
            //WinningBet = CalcBet(8, 2); // 200th asteroid


            rawMapData = Helper.GetFileContentAsLines("D10-Data.txt");
            Width = rawMapData[0].Length;
            Height = rawMapData.Length;
            BuildData(rawMapData, zoneList, ref Origin);
            if (Origin == null)
                Origin = zoneList.Where(z => z.Location.X == 26 && z.Location.Y == 29).FirstOrDefault();
            WinningBet = 1419;


            //BuildDebugData(zoneList, ref Origin);

            Map = zoneList;

        }

        private void BuildData(string[] rawMapData, List<MapZone> zoneList, ref MapZone origin)
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var hasBase = (rawMapData[y][x] == 'X');
                    var hasRoid = (rawMapData[y][x] == '#') || hasBase;
                    if (!hasRoid)
                        continue;
                    var zone = new MapZone(x, y);
                    zoneList.Add(zone);

                    if (hasBase)
                        origin = zone;
                }
            }
        }
        private void BuildDebugData(List<MapZone> zoneList, ref MapZone origin)
        {
            var x = 0;
            var y = 0;

            zoneList.Clear();
            origin = new MapZone(x, y);
            zoneList.Add(Origin);
            
            zoneList.Add(new MapZone(x + 0, y + -2));
            zoneList.Add(new MapZone(x + 1, y + -2));
            zoneList.Add(new MapZone(x + 2, y + -2));
            zoneList.Add(new MapZone(x + 2, y + -1));
            zoneList.Add(new MapZone(x + 2, y + 0));
            zoneList.Add(new MapZone(x + 2, y + 1));
            zoneList.Add(new MapZone(x + 2, y + 2));
            zoneList.Add(new MapZone(x + 1, y + 2));
            zoneList.Add(new MapZone(x + 0, y + 2));
            zoneList.Add(new MapZone(x + -1, y + 2));
            zoneList.Add(new MapZone(x + -2, y + 2));
            zoneList.Add(new MapZone(x + -2, y + 1));
            zoneList.Add(new MapZone(x + -2, y + 0));
            zoneList.Add(new MapZone(x + -2, y + -1));
            zoneList.Add(new MapZone(x + -2, y + -2));
            zoneList.Add(new MapZone(x + -1, y + -2));
        }

        readonly IReadOnlyList<MapZone> Map;
        readonly MapZone Origin;
        readonly int? WinningBet;

        readonly int Width;
        readonly int Height;


        public void Run()
        {
            Console.WriteLine("--- Day 10: Monitoring Station (part 2) ---");

            var allAsteroidZones = Map
                .Where(z => z.HasAsteroid)
                .Where(z => z != Origin)
                ;
            Console.WriteLine($"# Asteroids: {allAsteroidZones.Count()}");

            foreach (var z in allAsteroidZones)
                z.Angle = Math.Round(CalcAngle(Origin.Location, z.Location), 5);
            
            Console.WriteLine(Origin);
            
            var numOrder = 0;
            var foundBet = -1;
            var toVaporize = allAsteroidZones
                .OrderBy(z => z.Angle)
                .ThenBy(z => z.Distance(Origin));

            while (toVaporize.Count() > 0)
            {
                double lastAngle = -1.0;
                foreach (var z in toVaporize)
                {
                    if (lastAngle != z.Angle)
                    {
                        numOrder++;
                        z.Vaporize();
                        Console.WriteLine($"[{numOrder,3}]: {z}");
                        if (numOrder == 200)
                            foundBet = CalcBet(z.Location.X, z.Location.Y);
                    }
                    lastAngle = z.Angle;
                }
            }

            Console.WriteLine($"Winning Bet: {foundBet}");
            if (foundBet.Equals(WinningBet))
            {
                Console.WriteLine("\tCorrect");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        /// <remarks>
        /// https://math.stackexchange.com/a/714423
        /// </remarks>
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

            return angle;
        }

        /// <summary>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        /// <remarks>
        /// https://stackoverflow.com/a/17590923/6136
        /// </remarks>
        bool CalcIfOnLine(Point a, Point b, Point p)
        {
            Func<Point, Point, double> calc = (q, w) => Math.Sqrt(Math.Pow(w.X - q.X, 2) + Math.Pow(w.Y - q.Y, 2));

            var ab = calc(a, b);
            var ap = calc(a, p);
            var pb = calc(p, b);

            var apb = ap + pb;
            ab = Math.Round(ab, 5);
            apb = Math.Round(apb, 5);
            var result = ab == apb;
            return result;
        }


        static int CalcBet(int x, int y) => (x * 100) + y;

        class MapZone
        {
            public MapZone(int x, int y)
            {
                Location = new Point(x, y);
                HasAsteroid = true;
            }

            public Point Location { get; private set; }
            public bool HasAsteroid { get; private set; }
            public double Angle { get; set; }

            public void Vaporize()
            {
                HasAsteroid = false;
            }

            public override string ToString()
            {
                return $"({Location.X,2},{Location.Y,2}) - {(HasAsteroid ? "#" : ".")}   {Angle}°";
            }

            public override bool Equals(object obj)
            {
                var other = obj as MapZone;
                if (other == null)
                    return false;
                return Location.Equals(other.Location);
            }
            public override int GetHashCode()
            {
                return Location.GetHashCode();
            }

            public int Distance(MapZone other) => Distance(other.Location);
            public int Distance(Point other) => Math.Abs(other.X - Location.X) + Math.Abs(other.Y - Location.Y);
        }
    }
}
