using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.CSharp.Common;
using NUnit.Framework;

namespace AdventOfCode.CSharp.Year2015
{
    [TestFixture]
    internal class Day14 : TestBase
    {
        private const int DAY = 14;



        private interface IReindeerStats
        {
            int Velocity { get; }
            int FlyTime { get; }
            int RestTime { get; }
        }
        private class ReindeerStats : IReindeerStats
        {
            public int Velocity { get; set; }
            public int FlyTime { get; set; }
            public int RestTime { get; set; }
        }

        private (IDictionary<string, IReindeerStats> input, int? expected) GetTestData(int part, string inputName)
        {
            var inputRegex = new Regex("(?<reindeer>.*) can fly (?<velocity>.*) km/s for (?<flyTime>.*) seconds, but then must rest for (?<restTime>.*) seconds\\.");

            var input = InputHelper.ReadLines(DAY, inputName, _rootPath)
                .Select(ParseInput)
                .ToDictionary(d => d.reindeer,
                              v => v.stats);

            var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}", _rootPath)
                ?.ToInt32();

            return (input, expected);



            (string reindeer, IReindeerStats stats) ParseInput(string text)
            {
                var match = inputRegex.Match(text);
                if (!match.Success)
                    throw new ApplicationException("Input line does not match the pattern");

                return (
                    match.Groups["reindeer"].Value,
                    new ReindeerStats()
                    {
                        Velocity = match.Groups["velocity"].Value.ToInt32(),
                        FlyTime = match.Groups["flyTime"].Value.ToInt32(),
                        RestTime = match.Groups["restTime"].Value.ToInt32()
                    });
            }
        }



        [TestCase(1, "example1", 1000)]
        [TestCase(1, "input", 2503)]
        public void Part1(int part, string inputName, int raceTime)
        {
            var (input, expected) = GetTestData(part, inputName);

            var result = GetTheDistanceWinningReindeerTraveled(input, raceTime);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }

        [TestCase(2, "example1", 1000)]
        [TestCase(2, "input", 2503)]
        public void Part2(int part, string inputName, int raceTime)
        {
            var (input, expected) = GetTestData(part, inputName);

            var result = GetThePointsWinningReindeerHas(input, raceTime);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }



        private int GetTheDistanceWinningReindeerTraveled(IDictionary<string, IReindeerStats> input, int raceTime)
        {
            var result = input
                .Select(kv => CalculateTotalFightDistance(kv.Value, raceTime))
                .OrderByDescending(x => x)
                .First();

            return result;
        }

        private int GetThePointsWinningReindeerHas(IDictionary<string, IReindeerStats> input, int raceTime)
        {
            var reindeerScores = input.ToDictionary(
                kv => kv.Key,
                kv => 0);

            for (int i = 0; i < raceTime; i++)
            {
                var distanceAtTime = input.Select(kv => (reindeer: kv.Key, distance: CalculateTotalFightDistance(kv.Value, i + 1)))
                    .ToArray();
                var maxDistance = distanceAtTime.Max(x => x.distance);
                var leadingReindeers = distanceAtTime.Where(x => x.distance == maxDistance);
                foreach (var reindeer in leadingReindeers)
                    reindeerScores[reindeer.reindeer] += 1;
            }

            return reindeerScores
                .OrderByDescending(rs => rs.Value)
                .First()
                .Value;
        }


        private int CalculateTotalFightDistance(IReindeerStats stats, int raceTime)
        {
            var flyRestTime = stats.FlyTime + stats.RestTime;
            var wholeIntervals = raceTime / flyRestTime;
            var remainderTime = raceTime - (wholeIntervals * flyRestTime);

            var totalDistance = (stats.FlyTime * stats.Velocity) * wholeIntervals;
            totalDistance += Math.Min(remainderTime, stats.FlyTime) * stats.Velocity;

            return totalDistance;
        }
    }
}
