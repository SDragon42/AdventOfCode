using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common.Extensions;
using NUnit.Framework;

namespace AdventOfCode.CSharp.Year2015
{
    [TestFixture]
    internal class Day03 : TestBase
    {
        private const int DAY = 3;



        private (List<char> input, int? expected) GetTestData(int part, string inputName)
        {
            var input = Input.ReadText(DAY, inputName)
                .Select(c => c)
                .ToList();

            var expected = Input.ReadText(DAY, $"{inputName}-answer{part}")
                ?.ToInt32();

            return (input, expected);
        }



        [TestCase(1, "example1")]
        [TestCase(1, "example2")]
        [TestCase(1, "example3")]
        [TestCase(1, "input")]
        public void Part1(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            var result = GetNumHousesVisited(input, 1);

            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }



        [TestCase(2, "example4")]
        [TestCase(2, "example2")]
        [TestCase(2, "example3")]
        [TestCase(2, "input")]
        public void Part2(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            var result = GetNumHousesVisitedWithRoboSanta(input, 2);

            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }



        public int GetNumHousesVisited(List<char> directions, int numPresents)
        {
            var houseDict = new Dictionary<Point, int>();

            var santaPos = new Point(0, 0);
            VisitHouse(houseDict, santaPos, numPresents);

            for (int i = 0; i < directions.Count; i++)
            {
                var direction = directions[i];
                santaPos = CalculatePosition(santaPos, direction);

                VisitHouse(houseDict, santaPos, numPresents);
            }

            return houseDict.Count;
        }

        public int GetNumHousesVisitedWithRoboSanta(List<char> directions, int numPresents)
        {
            var houseDict = new Dictionary<Point, int>();

            var santaPos = new Point(0, 0);
            var roboSantaPos = new Point(0, 0);
            VisitHouse(houseDict, santaPos, numPresents);

            for (int i = 0; i < directions.Count; i = i + 2)
            {
                var santaDirection = directions[i];
                var roboSantaDirection = directions[i + 1];

                santaPos = CalculatePosition(santaPos, santaDirection);
                roboSantaPos = CalculatePosition(roboSantaPos, roboSantaDirection);

                VisitHouse(houseDict, santaPos, numPresents);
                VisitHouse(houseDict, roboSantaPos, numPresents);
            }

            return houseDict.Count;
        }



        private static Point CalculatePosition(Point current, char direction)
        {
            switch (direction)
            {
                case '^': return new Point(current.X, current.Y + 1);
                case '>': return new Point(current.X + 1, current.Y);
                case 'v': return new Point(current.X, current.Y - 1);
                case '<': return new Point(current.X - 1, current.Y);
                default: throw new InvalidOperationException($"The token '{direction}'");
            }
        }

        private static void VisitHouse(IDictionary<Point, int> houseDict, Point pos, int numPresents)
        {
            if (houseDict.ContainsKey(pos))
                houseDict[pos] = houseDict[pos] + numPresents;
            else
                houseDict.Add(pos, numPresents);
        }
    }
}
