using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.CSharp.Common;
using NUnit.Framework;

namespace AdventOfCode.CSharp.Year2015
{
    [TestFixture]
    public class Day02 : TestBase
    {
        private const int DAY = 2;



        private (List<Box> input, int? expected) GetTestData(int part, string inputName)
        {
            var input = InputHelper.ReadLines(DAY, inputName, _rootPath)
                .Select(dim => new Box(dim))
                .ToList();

            var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}", _rootPath)
                ?.ToInt32();

            return (input, expected);
        }


        [TestCase(1, "example1")]
        [TestCase(1, "example2")]
        [TestCase(1, "input")]
        public void Part1(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            var result = GetTotalSquareFeetOfPaper(input);

            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }



        [TestCase(2, "example1")]
        [TestCase(2, "example2")]
        [TestCase(2, "input")]
        public void Part2(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            var result = GetTotalFeetOfRibbon(input);

            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }




        private int GetTotalSquareFeetOfPaper(List<Box> input)
        {
            var paperNeeded = 0;
            foreach (var box in input)
            {
                var areas = GetSideAreas(box).ToList();

                var totalArea = areas
                    .Select(a => a * 2)
                    .Sum();

                var slack = areas.Min();

                paperNeeded += totalArea + slack;
            }

            return paperNeeded;
        }

        private int GetTotalFeetOfRibbon(List<Box> input)
        {
            var ribbonNeeded = 0;
            foreach (var box in input)
            {
                var toWrap = GetFacePerimeterLengths(box).Min();

                var bow = box.Length * box.Width * box.Height;

                ribbonNeeded += toWrap + bow;
            }

            return ribbonNeeded;
        }



        private static IEnumerable<int> GetSideAreas(Box box)
        {
            yield return box.Length * box.Width;
            yield return box.Width *  box.Height;
            yield return box.Height * box.Length;
        }

        private static IEnumerable<int> GetFacePerimeterLengths(Box box)
        {
            yield return (box.Length + box.Width) * 2;
            yield return (box.Width +  box.Height) * 2;
            yield return (box.Height + box.Length) * 2;
        }



        private class Box
        {
            public Box(string dimensions)
            {
                var parts = dimensions.Split('x');

                Length = parts[0].ToInt32();
                Width = parts[1].ToInt32();
                Height = parts[2].ToInt32();
            }

            public int Length { get; private set; }
            public int Width { get; private set; }
            public int Height { get; private set; }
        }

    }

}
