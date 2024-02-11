using System.Collections.Generic;
using System.Linq;
using AdventOfCode.CSharp.Common;
using NUnit.Framework;

namespace AdventOfCode.CSharp.Year2015
{
    [TestFixture]
    internal class Day05 : TestBase
    {
        private const int DAY = 5;



        private (List<string> input, int? expected) GetTestData(int part, string inputName)
        {
            var input = InputHelper.ReadLines(DAY, inputName, _rootPath)
                .ToList();

            var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}", _rootPath)
                ?.ToInt32();

            return (input, expected);
        }



        [TestCase(1, "example1")]
        [TestCase(1, "example2")]
        [TestCase(1, "example3")]
        [TestCase(1, "example4")]
        [TestCase(1, "example5")]
        [TestCase(1, "input")]
        public void Part1(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            var result = FindNumberOfNiceStrings(input);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }

        [TestCase(2, "example6")]
        [TestCase(2, "example7")]
        [TestCase(2, "example8")]
        [TestCase(2, "example9")]
        [TestCase(2, "input")]
        public void Part2(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            var result = FindNumberOfNiceStrings2(input);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }



        private int FindNumberOfNiceStrings(List<string> input)
        {
            var result = 0;

            foreach (var item in input)
            {
                if (!Rule_HasAtLeast3Vowels(item))
                    continue;
                if (!Rule_HasRepeatedCharacters(item))
                    continue;
                if (!Rule_DoesNotContainStrings(item))
                    continue;
                result++;
            }

            return result;
        }

        private bool Rule_HasAtLeast3Vowels(string text)
        {
            var vowels = new char[] { 'a', 'e', 'i', 'o', 'u' };
            var q = text.Where(c => vowels.Contains(c)).Count();
            return (q >= 3);
        }

        private bool Rule_HasRepeatedCharacters(string text)
        {
            for (var i = 1; i < text.Length; i++)
            {
                if (text[i] == text[i - 1])
                {
                    return true;
                }
            }
            return false;
        }

        private bool Rule_DoesNotContainStrings(string text)
        {
            var excludeStrings = new string[] { "ab", "cd", "pq", "xy" };

            foreach (var pattern in excludeStrings)
            {
                if (text.Contains(pattern))
                {
                    return false;
                }
            }
            return true;
        }


        private int FindNumberOfNiceStrings2(List<string> input)
        {
            var result = 0;

            foreach (var item in input)
            {
                if (!Rule_RepeatedPairNoOverlap(item))
                    continue;
                if (!Rule_HasRepeatedCharacterWith1Space(item))
                    continue;
                result++;
            }

            return result;
        }

        private bool Rule_RepeatedPairNoOverlap(string text)
        {
            int i = 0;
            while (i < text.Length - 1)
            {
                var pair = text.Substring(i, 2);
                var remainder = text.Substring(i + 2);
                if (remainder.Contains(pair))
                {
                    return true;
                }
                i++;
            }
            return false;
        }

        private bool Rule_HasRepeatedCharacterWith1Space(string text)
        {
            for (var i = 2; i < text.Length; i++)
            {
                if (text[i] == text[i - 2])
                {
                    return true;
                }
            }
            return false;
        }

    }
}
