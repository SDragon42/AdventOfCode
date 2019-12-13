﻿using System;
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
            Origin = new MapZone(3, 9, true);
            WinningBet = CalcBet(8, 2); // 200th asteroid

            //rawMapData = Helper.GetFileContentAsLines("D10-Data.txt");
            //Origin = new MapZone(26, 29, true);
            //WinningBet = null;

            Width = rawMapData[0].Length;
            Height = rawMapData.Length;
            var zoneList = new List<MapZone>();
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var zone = new MapZone(x, y, rawMapData[y][x] == '#');
                    zoneList.Add(zone);
                }
            }

            Map = zoneList;

        }

        readonly IReadOnlyList<MapZone> Map;
        readonly MapZone Origin;
        readonly int? WinningBet;

        readonly int Width;
        readonly int Height;


        public void Run()
        {
            Console.WriteLine("--- Day 10: Monitoring Station (part 2) ---");

            //var allAsteroidZones = Map.Where(z => z.HasAsteroid).ToList();
            //Console.WriteLine($"# Asteroids: {allAsteroidZones.Count}");

            //foreach (var home in allAsteroidZones)
            //    ScanFrom(home, allAsteroidZones);

            //var best = allAsteroidZones
            //    .OrderByDescending(z => z.Detects)
            //    .First();

            //Console.WriteLine();
            //ShowDetectionCounts();
            //Console.WriteLine();
            var foundBet = 0;

            //Console.WriteLine($"Best: ({best.X},{best.Y})   Sees: {best.Detects}");

            if (foundBet.Equals(WinningBet))
            {
                Console.WriteLine("\tCorrect");
            }

            Console.WriteLine();
        }

        void ScanFrom(MapZone home, IEnumerable<MapZone> allAlteroids)
        {
            var allButHome = allAlteroids.Where(z => z != home);
            foreach (var target in allButHome)
            {
                var canSee = Map
                    .Where(z => z != home)
                    .Where(z => z != target)
                    .Where(z => CalcIfOnLine(home, target, z))
                    .All(z => !z.HasAsteroid);
                if (canSee)
                    home.Detects++;
            }
        }



        /// <summary>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        /// <remarks>https://stackoverflow.com/a/17590923/6136</remarks>
        bool CalcIfOnLine(MapZone a, MapZone b, MapZone p)
        {
            Func<MapZone, MapZone, double> calc = (q, w) => Math.Sqrt(Math.Pow(w.X - q.X, 2) + Math.Pow(w.Y - q.Y, 2));

            var ab = calc(a, b);
            var ap = calc(a, p);
            var pb = calc(p, b);

            var apb = ap + pb;
            ab = Math.Round(ab, 5);
            apb = Math.Round(apb, 5);
            var result = ab == apb;
            return result;
        }

        void ShowAsteroids() => ShowMap(z => z.HasAsteroid ? " # " : " . ");
        void ShowDetectionCounts() => ShowMap(z => z.HasAsteroid ? z.Detects.ToString().PadLeft(3, ' ').PadRight(3, ' ') : " . ");
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
            public MapZone(int x, int y, bool hasAsteroid)
            {
                X = x;
                Y = y;
                HasAsteroid = hasAsteroid;
                Detects = 0;
            }

            public int X { get; private set; }
            public int Y { get; private set; }
            public bool HasAsteroid { get; private set; }
            public int Detects { get; set; }

            public override string ToString()
            {
                return $"({X},{Y}) - {(HasAsteroid ? "#" : ".")}   Sees:{Detects}";
            }

            public bool EqualsLocation(MapZone other)
            {
                if (other == null)
                    return false;
                return (X == other.X) && (Y == other.Y);
            }
        }
    }
}
