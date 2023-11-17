using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode.CSharp.Common;
using NUnit.Framework;

namespace AdventOfCode.CSharp.Year2015
{
    [TestFixture]
    internal class Day06 : TestBase
    {
        private const int DAY = 6;



        private (List<Instruction> input, int? expected) GetTestData(int part, string inputName)
        {
            var inputRegex = new Regex("(?<cmd>.*) (?<x>.*),(?<y>.*) through (?<x2>.*),(?<y2>.*)");

            var input = InputHelper.ReadLines(DAY, inputName, _rootPath)
                .Select(ParseInput)
                .ToList();

            var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}", _rootPath)
                ?.ToInt32();

            return (input, expected);

            Instruction ParseInput(string text)
            {
                var match = inputRegex.Match(text);
                if (!match.Success)
                    throw new ApplicationException("Input line does not match the pattern");

                var cmd = match.Groups["cmd"].Value;

                var p1 = new Point(
                    (int)Convert.ChangeType(match.Groups["x"].Value, typeof(int)),
                    (int)Convert.ChangeType(match.Groups["y"].Value, typeof(int)));

                var p2 = new Point(
                    (int)Convert.ChangeType(match.Groups["x2"].Value, typeof(int)),
                    (int)Convert.ChangeType(match.Groups["y2"].Value, typeof(int)));

                return new Instruction(cmd, p1, p2);
            }
        }



        [TestCase(1, "example1")]
        [TestCase(1, "example2")]
        [TestCase(1, "example3")]
        [TestCase(1, "input")]
        public void Part1(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            var result = GetNumberOfLitLights(input);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }

        [TestCase(2, "example4")]
        [TestCase(2, "example5")]
        [TestCase(2, "input")]
        public void Part2(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            var result = GetTotalLightBrightness(input);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }



        private int GetNumberOfLitLights(List<Instruction> input)
        {
            const int Size = 1000;
            var lights = new bool[Size * Size];

            foreach (var instruction in input)
            {
                for (var x = instruction.From.X; x <= instruction.To.X; x++)
                {
                    for (var y = instruction.From.Y; y <= instruction.To.Y; y++)
                    {
                        var index = x + (y * Size);
                        switch (instruction.Command)
                        {
                            case "turn on": lights[index] = true; break;
                            case "turn off": lights[index] = false; break;
                            case "toggle": lights[index] = !lights[index]; break;
                        }
                    }
                }
            }

            return lights.Where(l => l).Count();
        }

        private int GetTotalLightBrightness(List<Instruction> input)
        {
            const int Size = 1000;
            var lights = new int[Size * Size];

            foreach (var instruction in input)
            {
                for (var x = instruction.From.X; x <= instruction.To.X; x++)
                {
                    for (var y = instruction.From.Y; y <= instruction.To.Y; y++)
                    {
                        var index = x + (y * Size);
                        switch (instruction.Command)
                        {
                            case "turn on": lights[index] += 1; break;
                            case "turn off":
                                lights[index] -= 1; 
                                if (lights[index] < 0)
                                    lights[index] = 0;
                                break;
                            case "toggle": lights[index] += 2; break;
                        }
                    }
                }
            }

            return lights.Sum();
        }



        private class Instruction
        {
            public Instruction(string command, Point from, Point to)
            {
                Command = command;
                From = from;
                To = to;
            }

            public string Command { get; private set; }
            public Point From { get; private set; }
            public Point To { get; private set; }
        }
    }
}
