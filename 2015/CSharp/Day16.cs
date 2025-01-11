using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Common.Extensions;
using NUnit.Framework;


namespace AdventOfCode.CSharp.Year2015
{
    [TestFixture]
    internal class Day16 : TestBase
    {
        private const int DAY = 16;


        private delegate bool RuleMethodDelegate(DetailsDictionary details, string key, int expectedValue);

        private class DetailsDictionary : Dictionary<string, int> { }

        private class RulesDictionary : Dictionary<string, RuleMethodDelegate> { }

        private class AuntSue
        {
            public AuntSue(int id)
            {
                Id = id;
            }

            public int Id { get; private set; }
            public DetailsDictionary Details { get; private set; } = new DetailsDictionary();
        }

        private (IList<AuntSue> input, int? expected) GetTestData(int part, string inputName)
        {
            var inputRegex1 = new Regex("Sue (?<id>\\d+): (?<details>.*)", RegexOptions.Compiled);
            var inputRegex2 = new Regex("(?<stat>\\w+): (?<value>\\d+)", RegexOptions.Compiled);

            var input = Input.ReadLines(DAY, inputName)
                .Select(ParseInput)
                .ToList();

            var expected = Input.ReadText(DAY, $"{inputName}-answer{part}")
                ?.ToInt32();

            return (input, expected);



            AuntSue ParseInput(string text)
            {
                var match = inputRegex1.Match(text);
                if (!match.Success)
                    throw new ApplicationException("Input line does not match the pattern");

                var aunt = new AuntSue(match.Groups["id"].Value.ToInt32());

                foreach (Match match2 in inputRegex2.Matches(match.Groups["details"].Value))
                {
                    aunt.Details.Add(
                        match2.Groups["stat"].Value,
                        match2.Groups["value"].Value.ToInt32());
                }

                return aunt;
            }
        }



        [TestCase(1, "input")]
        public void Part1(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            var rulesDict = new RulesDictionary
            {
                { "children",    RuleExactMatch },
                { "cats",        RuleExactMatch },
                { "samoyeds",    RuleExactMatch },
                { "pomeranians", RuleExactMatch },
                { "akitas",      RuleExactMatch },
                { "vizslas",     RuleExactMatch },
                { "goldfish",    RuleExactMatch },
                { "trees",       RuleExactMatch },
                { "cars",        RuleExactMatch },
                { "perfumes",    RuleExactMatch },
            };
            var analysis = GetMfcsamAnalysis();

            var result = FindTheIdOfTheSuethatGaveTheGift(input, analysis, rulesDict);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }

        [TestCase(2, "input")]
        public void Part2(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            var rulesDict = new RulesDictionary
            {
                { "children",    RuleExactMatch },
                { "cats",        RuleGreaterThanMatch },
                { "samoyeds",    RuleExactMatch },
                { "pomeranians", RuleLessThanMatch },
                { "akitas",      RuleExactMatch },
                { "vizslas",     RuleExactMatch },
                { "goldfish",    RuleLessThanMatch },
                { "trees",       RuleGreaterThanMatch },
                { "cars",        RuleExactMatch },
                { "perfumes",    RuleExactMatch },
            };
            var analysis = GetMfcsamAnalysis();

            var result = FindTheIdOfTheSuethatGaveTheGift(input, analysis, rulesDict);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }

        private DetailsDictionary GetMfcsamAnalysis()
        {
            var result = new DetailsDictionary
            {
                { "children",    3 },
                { "cats",        7 },
                { "samoyeds",    2 },
                { "pomeranians", 3 },
                { "akitas",      0 },
                { "vizslas",     0 },
                { "goldfish",    5 },
                { "trees",       3 },
                { "cars",        2 },
                { "perfumes",    1 },
            };
            return result;
        }


        private int FindTheIdOfTheSuethatGaveTheGift(IList<AuntSue> input, DetailsDictionary analysis, RulesDictionary rules)
        {
            var foundId = input
                .Where(aunt => analysis.All(a => rules[a.Key](aunt.Details, a.Key, a.Value)))
                .First()
                .Id;
            return foundId;
        }


        private static bool NoData(DetailsDictionary details, string key, out int foundValue)
        {
            return !details.TryGetValue(key, out foundValue);
        }
        private static bool RuleExactMatch(DetailsDictionary details, string key, int expectedValue)
        {
            var hasNoData = NoData(details, key, out int foundValue);
            return hasNoData || foundValue == expectedValue;
        }
        private static bool RuleGreaterThanMatch(DetailsDictionary details, string key, int expectedValue)
        {
            var hasNoData = NoData(details, key, out int foundValue);
            return hasNoData || foundValue > expectedValue;
        }
        private static bool RuleLessThanMatch(DetailsDictionary details, string key, int expectedValue)
        {
            var hasNoData = NoData(details, key, out int foundValue);
            return hasNoData || foundValue < expectedValue;
        }
    }
}
