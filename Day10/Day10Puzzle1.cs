using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Advent_of_Code.Day10
{
    /*
    --- Day 10: Monitoring Station ---
    You fly into the asteroid belt and reach the Ceres monitoring station. The Elves here have
    an emergency: they're having trouble tracking all of the asteroids and can't be sure they're
    safe.

    The Elves would like to build a new monitoring station in a nearby area of space; they hand
    you a map of all of the asteroids in that region (your puzzle input).

    The map indicates whether each position is empty (.) or contains an asteroid (#). The
    asteroids are much smaller than they appear on the map, and every asteroid is exactly in the
    center of its marked position. The asteroids can be described with X,Y coordinates where X
    is the distance from the left edge and Y is the distance from the top edge (so the top-left
    corner is 0,0 and the position immediately to its right is 1,0).

    Your job is to figure out which asteroid would be the best place to build a new monitoring
    station. A monitoring station can detect any asteroid to which it has direct line of sight -
    that is, there cannot be another asteroid exactly between them. This line of sight can be at
    any angle, not just lines aligned to the grid or diagonally. The best location is the 
    asteroid that can detect the largest number of other asteroids.

    For example, consider the following map:

    .#..#
    .....
    #####
    ....#
    ...##

    The best location for a new monitoring station on this map is the highlighted asteroid at 3,4
    because it can detect 8 asteroids, more than any other location. (The only asteroid it cannot
    detect is the one at 1,0; its view of this asteroid is blocked by the asteroid at 2,2.) All
    other asteroids are worse locations; they can detect 7 or fewer other asteroids. Here is the
    number of other asteroids a monitoring station on each asteroid could detect:

    .7..7
    .....
    67775
    ....7
    ...87

    Here is an asteroid (#) and some examples of the ways its line of sight might be blocked. If
    there were another asteroid at the location of a capital letter, the locations marked with
    the corresponding lowercase letter would be blocked and could not be detected:

    #.........
    ...A......
    ...B..a...
    .EDCG....a
    ..F.c.b...
    .....c....
    ..efd.c.gb
    .......c..
    ....f...c.
    ...e..d..c

    Here are some larger examples:

    Best is 5,8 with 33 other asteroids detected:

    ......#.#.
    #..#.#....
    ..#######.
    .#.#.###..
    .#..#.....
    ..#....#.#
    #..#....#.
    .##.#..###
    ##...#..#.
    .#....####

    Best is 1,2 with 35 other asteroids detected:

    #.#...#.#.
    .###....#.
    .#....#...
    ##.#.#.#.#
    ....#.#.#.
    .##..###.#
    ..#...##..
    ..##....##
    ......#...
    .####.###.

    Best is 6,3 with 41 other asteroids detected:

    .#..#..###
    ####.###.#
    ....###.#.
    ..###.##.#
    ##.##.#.#.
    ....###..#
    ..#.#..#.#
    #..#.#.###
    .##...##.#
    .....#.#..

    Best is 11,13 with 210 other asteroids detected:

    .#..##.###...#######
    ##.############..##.
    .#.######.########.#
    .###.#######.####.#.
    #####.##.#.##.###.##
    ..#####..#.#########
    ####################
    #.####....###.#.#.##
    ##.#################
    #####.##.###..####..
    ..######..##.#######
    ####.##.####...##..#
    .#####..#.######.###
    ##...#.##########...
    #.##########.#######
    .####.#.###.###.#.##
    ....##.##.###..#####
    .#.#.###########.###
    #.#.#.#####.####.###
    ###.##.####.##.#..##

    Find the best location for a new monitoring station. How many other asteroids can be detected
    from that location?
     */
    class Day10Puzzle1 : IPuzzle
    {


        public Day10Puzzle1()
        {
            string[] rawMapData = null;

            //rawMapData = Helper.GetFileContentAsLines("D10P1-Test1.txt");
            //CorrectAnswer = new MapZone(3, 4, true) { Detects = 8 };

            //rawMapData = Helper.GetFileContentAsLines("D10P1-Test2.txt");
            //CorrectAnswer = new MapZone(5, 8, true) { Detects = 33 };

            //rawMapData = Helper.GetFileContentAsLines("D10P1-Test3.txt");
            //CorrectAnswer = new MapZone(1, 2, true) { Detects = 35 };

            //rawMapData = Helper.GetFileContentAsLines("D10P1-Test4.txt");
            //CorrectAnswer = new MapZone(6, 3, true) { Detects = 41 };

            //rawMapData = Helper.GetFileContentAsLines("D10P1-Test5.txt");
            //CorrectAnswer = new MapZone(11, 13, true) { Detects = 210 };

            rawMapData = Helper.GetFileContentAsLines("D10-Data.txt");
            CorrectAnswer = new MapZone(26, 29, true) { Detects = 299 };

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
        readonly MapZone CorrectAnswer;

        readonly int Width;
        readonly int Height;


        public void Run()
        {
            Console.WriteLine("--- Day 10: Monitoring Station ---");

            var allAsteroidZones = Map.Where(z => z.HasAsteroid).ToList();
            Console.WriteLine($"# Asteroids: {allAsteroidZones.Count}");

            foreach (var home in allAsteroidZones)
                ScanFrom(home, allAsteroidZones);

            var best = allAsteroidZones
                .OrderByDescending(z => z.Detects)
                .First();

            Console.WriteLine();
            ShowDetectionCounts();
            Console.WriteLine();

            Console.WriteLine($"Best: ({best.X},{best.Y})   Sees: {best.Detects}");

            if (best.EqualsLocation(CorrectAnswer))
            {
                Console.WriteLine("\tCorrect location");
                if (best.Detects == CorrectAnswer.Detects)
                    Console.WriteLine("\tCorrect detection");
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
