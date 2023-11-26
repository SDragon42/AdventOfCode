using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.CSharp.Common;
using NUnit.Framework;

namespace AdventOfCode.CSharp.Year2015
{
    [TestFixture]
    internal class Day09 : TestBase
    {
        private const int DAY = 9;



        private class InputData
        {
            private static Regex inputRegex = new Regex(
                "(?<city1>.*) to (?<city2>.*) = (?<distance>.*)",
                RegexOptions.Compiled);

            public InputData(string line)
            {
                var match = inputRegex.Match(line);
                if (!match.Success)
                    throw new ApplicationException("Input line does not match the pattern");

                City1 = match.Groups["city1"].Value;
                City2 = match.Groups["city2"].Value;
                Distance = match.Groups["distance"].Value.ToInt32();
            }

            public string City1 { get; private set; }
            public string City2 { get; private set; }
            public int Distance { get; private set; }
        }

        private (List<InputData> input, int? expected) GetTestData(int part, string inputName)
        {
            var input = InputHelper.ReadLines(DAY, inputName, _rootPath)
                .Select(l => new InputData(l))
                .ToList();

            var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}", _rootPath)
                ?.ToInt32();

            return (input, expected);
        }



        [TestCase(1, "example1")]
        [TestCase(1, "input")]
        public void Part1(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            var (cities, distMap) = BuildLists(input);
            var allroutes = GetAllRouteChains(cities);
            var allRoutesWithDistance = CalculateAllRouteDistances(allroutes, distMap);

            var result = allRoutesWithDistance.Select(a => a.distance).Min();

            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }

        [TestCase(2, "example1")]
        [TestCase(2, "input")]
        public void Part2(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            var (cities, distMap) = BuildLists(input);
            var allroutes = GetAllRouteChains(cities);
            var allRoutesWithDistance = CalculateAllRouteDistances(allroutes, distMap);

            var result = allRoutesWithDistance.Select(a => a.distance).Max();

            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }



        private (List<string>, Dictionary<(string, string), int>) BuildLists(IList<InputData> input)
        {
            var cities = new HashSet<string>();
            var distMap = new Dictionary<(string, string), int>();
            foreach (var item in input)
            {
                cities.Add(item.City1);
                cities.Add(item.City2);

                distMap.Add((item.City1, item.City2), item.Distance);
                distMap.Add((item.City2, item.City1), item.Distance);
            }

            return (cities.ToList(), distMap);
        }

        private (string city1, string city2)[][] GetAllRouteChains(IList<string> cities)
        {
            var result = Helper.GetPermutations(cities)
                .Select(route => route.Select(city => city)
                                      .Windowed(2)
                                      .Select(a => (a[0], a[1]))
                                      .ToArray())
                .ToArray();
            return result;
        }

        private ((string, string)[] route, int distance)[] CalculateAllRouteDistances((string city1, string city2)[][] allRoutes, IDictionary<(string, string), int> distMap)
        {
            var result = allRoutes
                .Select(route => (route, distance: route.Select(pair => distMap[pair]).Sum()))
                .ToArray();
            return result;
        }

    }
}
