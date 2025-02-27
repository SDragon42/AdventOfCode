﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common.Extensions;
using AdventOfCode.CSharp.Year2019.IntCodeComputer;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.CSharp.Year2019
{
    /// <summary>
    /// https://adventofcode.com/2019/day/17
    /// </summary>
    public class Day17 : TestBase
    {
        public Day17(ITestOutputHelper output) : base(output, 17) { }

        readonly bool DISPLAY = false;

        private (List<long>, long?) GetTestData(string name, int part)
        {
            var input = Input.ReadLines(DAY, name)
                .First()
                .Split(',')
                .Select(v => v.ToInt64())
                .ToList();

            var expected = Input.ReadLines(DAY, $"{name}-answer{part}")
                ?.FirstOrDefault()
                ?.ToInt64();

            return (input, expected);
        }

        private (List<List<char>>, long?) GetMapDataFromInput(string name, int part)
        {
            var map = new List<List<char>>();
            var lines = Input.ReadLines(DAY, name);
            foreach (var line in lines)
            {
                var t = line.Select(c => c).ToList();
                map.Add(t);
            }

            return (
                map,
                Input.ReadLines(DAY, $"{name}-answer{part}")?.FirstOrDefault()?.ToInt64()
            );
        }


        [Fact]
        public void Part1Example()
        {
            var (input, expected) = GetMapDataFromInput("example1", 1);

            var result = RunPart1Example(input);

            Assert.Equal(expected, result);
        }
        [Fact]
        public void Part1()
        {
            var (input, expected) = GetTestData("input", 1);

            var result = RunPart1(input);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2()
        {
            var (input, expected) = GetTestData("input", 2);

            var result = RunPart2(input);

            Assert.Equal(expected, result);
        }



        void Add2Map(long value, List<List<char>> map)
        {
            switch (value)
            {
                case Code_NewLine:
                    map.Add(new List<char>());
                    break;
                default:
                    var c = Convert.ToChar(value);
                    var line = GetLastLine();
                    line.Add(c);
                    break;
            }

            List<char> GetLastLine()
            {
                var line = map.LastOrDefault();
                if (line == null)
                {
                    line = new List<char>();
                    map.Add(line);
                }
                return line;
            }
        }

        void ShowOutput(long value)
        {
            if (!DISPLAY)
                return;
            switch (value)
            {
                case Code_NewLine:
                    Console.WriteLine();
                    break;
                default:
                    var c = Convert.ToChar(value);
                    Console.Write(c);
                    Console.Write(' ');
                    break;
            }
        }

        long RunPart1(List<long> code)
        {
            var map = new List<List<char>>();

            var bot = new IntCode(code);
            bot.Output += (s, e) => Add2Map(e.OutputValue, map);
            bot.Output += (s, e) => ShowOutput(e.OutputValue);
            bot.Run();

            var intersections = MarkIntersections(map);

            var answer = intersections
                .Select(p => p.X * p.Y)
                .Sum();
            return answer;
        }
        long RunPart1Example(List<List<char>> map)
        {
            var intersections = MarkIntersections(map);

            var answer = intersections
                    .Select(p => p.X * p.Y)
                    .Sum();
            return answer;
        }


        long RunPart2(List<long> code)
        {
            var answer = 0L;
            var map = new List<List<char>>();

            var bot = new IntCode(code);
            bot.Output += (s, e) =>
            {
                if (e.OutputValue < 256)
                    ShowOutput(e.OutputValue);
                else
                    answer = e.OutputValue;
            };
            bot.Poke(0, 2); // Wake up bot

            var movementCode =
                "A,A,B,C,A,C,B,C,A,B\n" +
                "L,4,L,10,L,6\n" +
                "L,6,L,4,R,8,R,8\n" +
                "L,6,R,8,L,10,L,8,L,8\n" +
                "n\n";
            var input = movementCode.ToCharArray()
                .Select(x => (long)(int)x)
                .ToArray();

            bot.AddInput(input);
            bot.Run();

            return answer;
        }



        const char Char_Scaffolding = '#';
        const char Char_Intersection = 'O';

        const long Code_NewLine = 10;

        private List<List<char>> GetMapData(IList<long> code)
        {
            var line = new List<char>();
            var map = new List<List<char>>();

            var bot = new IntCode(code);
            bot.Output += (s, e) => Add2Map(e.OutputValue);
            bot.Run();

            return map;

            void Add2Map(long value)
            {
                if (value != Code_NewLine)
                {
                    var c = Convert.ToChar(value);
                    line.Add(c);
                    return;
                }
                if (line.Count > 0)
                    map.Add(line);
                line = new List<char>();
            }
        }

        private void ShowMap(List<List<char>> map)
        {
            foreach (var line in map)
            {
                var text = string.Join("", line);
                Console.WriteLine(text);
            }
        }

        private List<Point> MarkIntersections(List<List<char>> map)
        {
            var offsets = new Point[] {
            new Point(0, -1),
            new Point(1, 0),
            new Point(0, 1),
            new Point(-1, 0),
        };

            var intersections = new List<Point>();

            for (var y = 0; y < map.Count; y++)
            {
                for (var x = 0; x < map[y].Count; x++)
                {
                    var tile = map[y][x];
                    if (tile != Char_Scaffolding)
                        continue;

                    var SidesWithScaffolding = offsets
                        .Select(o => hasScaffolding(new Point(x, y), o))
                        .Where(o => o)
                        .ToList();
                    if (SidesWithScaffolding.Count >= 3)
                    {
                        map[y][x] = Char_Intersection;
                        intersections.Add(new Point(x, y));
                    }
                }
            }

            return intersections;

            bool hasScaffolding(Point location, Point shift)
            {
                var loc = location;
                loc.Offset(shift);
                if (loc.Y < 0 || loc.Y >= map.Count)
                    return false;
                if (loc.X < 0 || loc.X >= map[loc.Y].Count)
                    return false;
                var tile = map[loc.Y][loc.X];
                var result = (tile == Char_Scaffolding || tile == Char_Intersection);
                return result;
            }
        }

    }
}
