﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.CSharp.Common;

namespace AdventOfCode.CSharp.Year2019
{
    /// <summary>
    /// https://adventofcode.com/2019/day/4
    /// </summary>
    class Day04 : PuzzleBase
    {
        delegate bool RuleMethod(int[] digits);
        readonly List<RuleMethod> Rules = new List<RuleMethod>();


        public Day04(bool benchmark) : base(benchmark) { }

        public override IEnumerable<string> SolvePuzzle()
        {
            yield return "Day 4: Secure Container";

            yield return string.Empty;
            yield return "Part 1) " + base.Run(() => RunPart1(GetPuzzleData(1, "input")));

            yield return string.Empty;
            yield return "Part 2) " + base.Run(() => RunPart2(GetPuzzleData(2, "input")));
        }


        class InputAnswer : InputAnswer<List<int>, int?>
        {
            public int PasswordRangeMin => Input[0];
            public int PasswordRangeMax => Input[1];
        }
        InputAnswer GetPuzzleData(int part, string name)
        {
            const int DAY = 4;

            var result = new InputAnswer()
            {
                Input = InputHelper.LoadInputFile(DAY, name)
                    .Split('-')
                    .Select(l => l.ToInt32())
                    .ToList(),
                ExpectedAnswer = InputHelper.LoadAnswerFile(DAY, part, name)?.ToInt32()
            };
            return result;
        }


        string RunPart1(InputAnswer puzzleData)
        {
            Rules.Clear();
            Rules.Add(Rule_IsSixDigits);
            Rules.Add(Rule_TwoAdjacentDigitsAreTheSame);
            Rules.Add(Rule_LeftToRightDigitValueNeverDecreases);

            var numValidPasswords = 0;
            for (var password = puzzleData.PasswordRangeMin; password <= puzzleData.PasswordRangeMax; password++)
            {
                if (IsPasswordValid(password))
                    numValidPasswords++;
            }

            return Helper.GetPuzzleResultText($"Number of valid passwords: {numValidPasswords}", numValidPasswords, puzzleData.ExpectedAnswer);
        }

        string RunPart2(InputAnswer puzzleData)
        {
            Rules.Clear();
            Rules.Add(Rule_IsSixDigits);
            Rules.Add(Rule_OnlyTwoAdjacentDigitsAreTheSame);
            Rules.Add(Rule_LeftToRightDigitValueNeverDecreases);

            var numValidPasswords = 0;
            for (var password = puzzleData.PasswordRangeMin; password <= puzzleData.PasswordRangeMax; password++)
            {
                if (IsPasswordValid(password))
                    numValidPasswords++;
            }

            return Helper.GetPuzzleResultText($"Number of valid passwords: {numValidPasswords}", numValidPasswords, puzzleData.ExpectedAnswer);
        }


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