using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.CSharp.Common;
using NUnit.Framework;

namespace AdventOfCode.CSharp.Year2015
{
    using DinnerGuestDictionary = Dictionary<string, Dictionary<string, int>>;

    [TestFixture]
    internal class Day13 : TestBase
    {
        private const int DAY = 13;



        private (DinnerGuestDictionary input, int? expected) GetTestData(int part, string inputName)
        {
            var inputRegex = new Regex("(?<name1>.*) would (?<dir>.*) (?<value>.*) happiness units by sitting next to (?<name2>.*)\\.");

            var input = InputHelper.ReadLines(DAY, inputName, _rootPath)
                .Select(ParseInput);

            var dict = new DinnerGuestDictionary();
            foreach (var (name, nextToName, happinessUnits) in input)
            {
                if (!dict.ContainsKey(name))
                    dict.Add(name, new Dictionary<string, int>());
                dict[name].Add(nextToName, happinessUnits);
            }

            var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}", _rootPath)
                ?.ToInt32();

            return (dict, expected);

            (string name, string nextToName, int happinessUnits) ParseInput(string text)
            {
                var match = inputRegex.Match(text);
                if (!match.Success)
                    throw new ApplicationException("Input line does not match the pattern");

                var units = match.Groups["value"].Value.ToInt32();
                if (match.Groups["dir"].Value == "lose")
                    units *= -1;
                return (
                    match.Groups["name1"].Value,
                    match.Groups["name2"].Value,
                    units);
            }
        }



        [TestCase(1, "example1")]
        [TestCase(1, "input")]
        public void Part1(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            var result = GetTotalHappiness(input);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }

        [TestCase(2, "input")]
        public void Part2(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            AddSelf(input);
            var result = GetTotalHappiness(input);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }

        

        private int GetTotalHappiness(DinnerGuestDictionary dict)
        {
            var b = Helper.GetPermutations(dict.Keys)
                .Select(x => x.ToList())
                .Select(CalculateHappiness)
                .Max();

            return b;

            int CalculateHappiness(IList<string> people)
            {
                var happiness = 0;
                for (var i = 0; i < people.Count; i++)
                {
                    var name = people[i];

                    var prevName = i > 0 
                        ? people[i - 1] 
                        : people[people.Count - 1];
                    var nextName = i < people.Count - 1 
                        ? people[i + 1] 
                        : people[0];

                    happiness += dict[name][nextName] + dict[name][prevName];
                }
                return happiness;
            }
        }

        private void AddSelf(DinnerGuestDictionary input)
        {
            const string self = "self";

            var selfDict = new Dictionary<string, int>();

            foreach (var guest in input.Keys)
            {
                selfDict.Add(guest, 0);
                input[guest].Add(self, 0);
            }

            input.Add(self, selfDict);
        }
    }
}
