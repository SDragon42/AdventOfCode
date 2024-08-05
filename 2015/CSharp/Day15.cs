using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.CSharp.Common;
using NUnit.Framework;


namespace AdventOfCode.CSharp.Year2015
{
    [TestFixture]
    internal class Day15 : TestBase
    {
        private const int DAY = 15;


        private const int TEASPOONS = 100;

        private class Ingredient
        {
            public Ingredient(string name, int capacity, int durability, int flavor, int texture, int calories)
            {
                Name = name;
                Capacity = capacity;
                Durability = durability;
                Flavor = flavor;
                Texture = texture;
                Calories = calories;
            }

            public string Name { get; private set; }
            public int Capacity { get; private set; }
            public int Durability { get; private set; }
            public int Flavor { get; private set; }
            public int Texture { get; private set; }
            public int Calories { get; private set; }

            public override string ToString() => Name;
        }

        private (IList<Ingredient> input, int? expected) GetTestData(int part, string inputName)
        {
            var inputRegex = new Regex("(?<ingredient>.*): capacity (?<capacity>.*), durability (?<durability>.*), flavor (?<flavor>.*), texture (?<texture>.*), calories (?<calories>.*)");

            var input = InputHelper.ReadLines(DAY, inputName, _rootPath)
                .Select(ParseInput)
                .ToList();

            var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}", _rootPath)
                ?.ToInt32();

            return (input, expected);



            Ingredient ParseInput(string text)
            {
                var match = inputRegex.Match(text);
                if (!match.Success)
                    throw new ApplicationException("Input line does not match the pattern");

                return new Ingredient(
                        match.Groups["ingredient"].Value,
                        match.Groups["capacity"].Value.ToInt32(),
                        match.Groups["durability"].Value.ToInt32(),
                        match.Groups["flavor"].Value.ToInt32(),
                        match.Groups["texture"].Value.ToInt32(),
                        match.Groups["calories"].Value.ToInt32()
                    );
            }
        }



        [TestCase(1, "example1")]
        [TestCase(1, "input")]
        public void Part1(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            var result = GetTotalScoreOfHighestScoringCookie(input);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }

        [TestCase(2, "example1")]
        [TestCase(2, "input")]
        public void Part2(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            var result = GetTotalScoreOfHighestScoringCookie(input, 500);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }


        private int GetTotalScoreOfHighestScoringCookie(IList<Ingredient> input, int? targetCalories = null)
        {
            var amountLists = input.Select(i => Enumerable.Range(1, TEASPOONS))
                                   .ToArray();

            var withScore = ZipN(amountLists)
                .Where(l => l.Sum() == TEASPOONS)
                .Select(l => l.Select((amount, index) => (amount, input[index])))
                .Select(d => (pair: d, score: CalculateTotalScore(d), calories: CalculateTotalCalories(d)));

            if (targetCalories != null)
            {
                withScore = withScore.Where(l => l.calories == targetCalories.Value);
            }

            var result = withScore.Select(l => l.score)
                                  .Max();

            return result;
        }



        private int CalculateTotalScore(IEnumerable<(int amount, Ingredient ingredients)> data)
        {
            int capacity = 0;
            int durability = 0;
            int flavor = 0;
            int texture = 0;

            foreach (var (amount, ingredients) in data)
            {
                capacity += amount * ingredients.Capacity;
                durability += amount * ingredients.Durability;
                flavor += amount * ingredients.Flavor;
                texture += amount * ingredients.Texture;
            }

            capacity = Math.Max(capacity, 0);
            durability = Math.Max(durability, 0);
            flavor = Math.Max(flavor, 0);
            texture = Math.Max(texture, 0);

            return capacity * durability * flavor * texture;
        }

        private int CalculateTotalCalories(IEnumerable<(int amount, Ingredient ingredients)> data)
        {
            int calories = 0;

            foreach (var (amount, ingredients) in data)
            {
                calories += amount * ingredients.Calories;
            }

            calories = Math.Max(calories, 0);

            return calories;
        }


        private static IEnumerable<List<T>> ZipN<T>(params IEnumerable<T>[] args)
        {
            var firstArg = args.First().ToArray();
            var remainingArgs = args.Skip(1).ToArray();

            if (remainingArgs.Length > 0)
            {
                var subResult = ZipN(remainingArgs).ToArray();

                foreach (var item in firstArg)
                {
                    foreach (var sItem in subResult)
                    {
                        var result = new List<T>() { item };
                        result.AddRange(sItem);
                        yield return result;
                    }
                }
            }
            else
            {
                foreach (var item in firstArg)
                    yield return new List<T>() { item };
            }
        }
    }
}
