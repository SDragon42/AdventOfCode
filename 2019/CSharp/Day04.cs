using System.Collections.Generic;
using System.Linq;
using AdventOfCode.CSharp.Common;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.CSharp.Year2019
{
    /// <summary>
    /// https://adventofcode.com/2019/day/4
    /// </summary>
    public class Day04 : TestBase
    {
        public Day04(ITestOutputHelper output) : base(output, 4) { }


        private (List<int>, int?) GetTestData(string name, int part)
        {
            var input = InputHelper.ReadLines(DAY, name)
                .First()
                .Split('-')
                .Select(l => l.ToInt32())
                .ToList();

            var expected = InputHelper.ReadLines(DAY, $"{name}-answer{part}")
                ?.FirstOrDefault()
                ?.ToInt32();

            return (input, expected);
        }

        [Theory]
        [InlineData("input")]
        public void Part1(string inputName)
        {
            var (input, expected) = GetTestData(inputName, 1);

            var passwordRangeMin = input[0];
            var passwordRangeMax = input[1];

            Rules.Clear();
            Rules.Add(Rule_IsSixDigits);
            Rules.Add(Rule_TwoAdjacentDigitsAreTheSame);
            Rules.Add(Rule_LeftToRightDigitValueNeverDecreases);

            var numValidPasswords = 0;
            for (var password = passwordRangeMin; password <= passwordRangeMax; password++)
            {
                if (IsPasswordValid(password))
                    numValidPasswords++;
            }

            output.WriteLine($"How many different passwords : {numValidPasswords}");

            Assert.Equal(expected, numValidPasswords);
        }

        [Theory]
        [InlineData("input")]
        public void Part2(string inputName)
        {
            var (input, expected) = GetTestData(inputName, 2);

            var passwordRangeMin = input[0];
            var passwordRangeMax = input[1];

            Rules.Clear();
            Rules.Add(Rule_IsSixDigits);
            Rules.Add(Rule_OnlyTwoAdjacentDigitsAreTheSame);
            Rules.Add(Rule_LeftToRightDigitValueNeverDecreases);

            var numValidPasswords = 0;
            for (var password = passwordRangeMin; password <= passwordRangeMax; password++)
            {
                if (IsPasswordValid(password))
                    numValidPasswords++;
            }

            output.WriteLine($"How many different passwords : {numValidPasswords}");

            Assert.Equal(expected, numValidPasswords);
        }



        delegate bool RuleMethod(int[] digits);
        readonly List<RuleMethod> Rules = new List<RuleMethod>();


        bool IsPasswordValid(int password)
        {
            var passwordDigits = GetDigits(password);
            var isValid = Rules.All(r => r.Invoke(passwordDigits));
            return isValid;
        }

        bool Rule_IsSixDigits(int[] passwordDigits)
        {
            const int requiredNumDigits = 6;
            var numDigits = passwordDigits.Length;
            return (numDigits == requiredNumDigits);
        }

        bool Rule_TwoAdjacentDigitsAreTheSame(int[] passwordDigits)
        {
            var hasDouble = false;
            for (var i = 1; i < passwordDigits.Length; i++)
                hasDouble |= (passwordDigits[i] == passwordDigits[i - 1]);
            return hasDouble;
        }

        bool Rule_OnlyTwoAdjacentDigitsAreTheSame(int[] passwordDigits)
        {
            var last = passwordDigits[0];
            var repeatCount = 0;
            foreach (var digit in passwordDigits.Skip(1))
            {
                if (digit == last)
                {
                    repeatCount++;
                    continue;
                }
                else
                {
                    if (repeatCount == 1) // double only
                        break;
                    repeatCount = 0;
                }

                last = digit;
            }

            var hasDouble = (repeatCount == 1);
            return hasDouble;
        }

        bool Rule_LeftToRightDigitValueNeverDecreases(int[] passwordDigits)
        {
            for (var i = 1; i < passwordDigits.Length; i++)
            {
                var current = passwordDigits[i];
                var last = passwordDigits[i - 1];
                if (current < last)
                    return false;
            }
            return true;
        }

        /// <summary>Splits the passed value into an array of digits
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int[] GetDigits(int value)
        {
            return value.ToString()
                .Select(c => (int)char.GetNumericValue(c))
                .ToArray();
        }

    }
}
