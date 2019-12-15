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
            string[] rawMapData = null;

            rawMapData = Helper.GetFileContentAsLines("D10P2-Test1.txt");
            WinningBet = CalcBet(8, 2); // 200th asteroid

            //rawMapData = Helper.GetFileContentAsLines("D10-Data.txt");
            //Origin = new MapZone(26, 29, true);
            //WinningBet = null;

            Width = rawMapData[0].Length;
            Height = rawMapData.Length;
            var zoneList = new List<MapZone>();
            NewMethod(rawMapData, zoneList, ref Origin);

            //var x = 8;
            //var y = 3;

            //zoneList.Clear();
            //Origin = new MapZone(x, y);
            //zoneList.Add(Origin);
            ////zoneList.Add(new MapZone(8, 1));
            ////zoneList.Add(new MapZone(9, 0));

            //zoneList.Add(new MapZone(x + 0, y + -4)); // 0°
            //zoneList.Add(new MapZone(x + 2, y + -4)); // 45° --
            //zoneList.Add(new MapZone(x + 4, y + -4)); // 45°
            ////zoneList.Add(new MapZone(x + 4, y + -2)); // 45° --
            //zoneList.Add(new MapZone(x + 4, y + 0)); // 90°
            ////zoneList.Add(new MapZone(x + 4, y + 2)); // 135° --
            //zoneList.Add(new MapZone(x + 4, y + 4)); // 135°
            ////zoneList.Add(new MapZone(x + 2, y + 4)); // 135° --
            //zoneList.Add(new MapZone(x + 0, y + 4)); // 180°
            ////zoneList.Add(new MapZone(x + -2, y + 4)); // 225° --
            //zoneList.Add(new MapZone(x + -4, y + 4)); // 225°
            ////zoneList.Add(new MapZone(x + -4, y + 2)); // 225° --
            //zoneList.Add(new MapZone(x + -4, y + 0)); // 270°
            ////zoneList.Add(new MapZone(x + -4, y + -2)); // 315° --
            //zoneList.Add(new MapZone(x + -4, y + -4)); // 315°
            ////zoneList.Add(new MapZone(x + -2, y + -4)); // 315° --


            Map = zoneList;

        }

        private void NewMethod(string[] rawMapData, List<MapZone> zoneList, ref MapZone origin)
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
            double lastAngle = -1.0;
            var numOrder = 0;
            var foundBet = 0;

            foreach (var z in allAsteroidZones.OrderBy(z => z.Angle))
            {
                if (lastAngle != z.Angle)
                {
                    numOrder++;
                    z.Vaporize();
                    Console.WriteLine($"[{numOrder,3}]: {z}");
                    if (numOrder == 200)
                        Console.WriteLine("#############################");
                }
                lastAngle = z.Angle;
            }

            //Console.WriteLine($"Best: ({best.X},{best.Y})   Sees: {best.Detects}");

            if (foundBet.Equals(WinningBet))
            {
                Console.WriteLine("\tCorrect");
            }

            Console.WriteLine();
        }

        double CalcAngle(Point p1, Point p2)
        {
            var y1 = (p1.Y > p2.Y) ? p1.Y : p2.Y;
            var y2 = (p1.Y > p2.Y) ? p2.Y : p1.Y;

            var numerator = y1 - y2;
            var denominator = Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(y1 - y2, 2));
            
            var angle = Math.Acos(numerator / denominator) * (180 / Math.PI);

            var deltaX = p2.X - p1.X;
            var deltaY = p1.Y - p2.Y;
            var angleOffsetMultiplier = (deltaX >= 0 && deltaY >= 0) ? 0
                : (deltaX >= 0 && deltaY <= 0) ? 1
                : (deltaX <= 0 && deltaY >= 0) ? 2
                : 3;

            angle = angle + (90.0 * angleOffsetMultiplier);

            //Console.WriteLine($"{p1,13}  {p2,13}  Delta: ({deltaX,2},{deltaY,2})   angle: {angle,9:N5}");
            return angle;
        }

        



        /// <summary>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        /// <remarks>https://stackoverflow.com/a/17590923/6136</remarks>
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

        void ShowAsteroids() => ShowMap(z => z == Origin ? "X" : z.HasAsteroid ? "*" : " ");
        void ShowMap(Func<MapZone, string> WriteAction)
        {
            int i = 0;
            while (i < Map.Count)
            {
                if (WriteAction != null)
                    Console.Write(WriteAction(Map[i]));
                i++;
                if (i % Width == 0)
                    Console.WriteLine();
            }
        }


        static int CalcBet(int x, int y) => (x * 100) + y;

        class MapZone
        {
            public MapZone(int x, int y)//, bool hasAsteroid)
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

            public bool EqualsLocation(MapZone other)
            {
                if (other == null)
                    return false;
                return Location.Equals(other.Location);
            }
        }
    }
}
