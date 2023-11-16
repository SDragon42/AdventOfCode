using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.CSharp.Common;
using NUnit.Framework;

namespace AdventOfCode.CSharp.Year2015
{
    public class Day01
    {
        private const int DAY = 1;

        private readonly string rootPath = TestContext.CurrentContext.TestDirectory;

        [SetUp]
        public void Setup()
        {
        }



        private (List<char> input, int? expected) GetTestData(int part, string inputName)
        {
            var input = InputHelper.ReadText(DAY, inputName, rootPath)
                .Select(c => c)
                .ToList();

            var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}", rootPath)
                ?.ToInt32();

            return (input, expected);
        }



        [TestCase(1, "example1")]
        [TestCase(1, "example2")]
        [TestCase(1, "example3")]
        [TestCase(1, "example4")]
        [TestCase(1, "example5")]
        [TestCase(1, "example6")]
        [TestCase(1, "example7")]
        [TestCase(1, "example8")]
        [TestCase(1, "example9")]
        [TestCase(1, "input")]
        public void Part1(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            var result = GetFloorTakenTo(input);

            Assert.AreEqual(expected, result);
        }


        [TestCase(2, "example10")]
        [TestCase(2, "example11")]
        [TestCase(2, "input")]
        public void Part2(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            var result = GetPositionFirstEnteringBasement(input);

            Assert.AreEqual(expected, result);
        }



        private int GetFloorTakenTo(List<char> input)
        {
            var result = input
                .Select(FloorChange)
                .Sum();

            return result;
        }

        private static int FloorChange(char c)
        {
            return c == '(' ? 1 : -1;
        }

        private int GetPositionFirstEnteringBasement(List<char> input)
        {
            var floor = 0;

            for (var i = 0; i < input.Count; i++)
            {
                floor += FloorChange(input[i]);
                if (floor < 0)
                {
                    return i + 1;
                }
            }

            return -1;
        }
    }
}