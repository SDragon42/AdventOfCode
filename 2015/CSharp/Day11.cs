using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.CSharp.Common;
using NUnit.Framework;

namespace AdventOfCode.CSharp.Year2015
{
    [TestFixture]
    internal class Day11 : TestBase
    {
        private readonly char[] _invalidChars = new[] { 'i', 'o', 'l' };



        [TestCase("abbceffg", false)]
        [TestCase("abbcegjk", false)]
        [TestCase("hijklmmn", false)]
        public void Part1TestIsPasswordValid(string input, bool isValid)
        {
            var inputArray = input.ToArray();
            var result = IsPasswordValid(inputArray);
            Output($"Answer: {result}");
            Assert.AreEqual(isValid, result);
        }

        [TestCase("abcdefgh", "abcdffaa")]
        [TestCase("ghijklmn", "ghjaabcc")]
        [TestCase("cqjxjnds", "cqjxxyzz")] // Part 1 Puzzle Input
        [TestCase("cqjxxyzz", "cqkaabcc")] // Part 2 Puzzle Input
        public void Part1And2(string input, string expected)
        {
            var result = FindNextPassword(input);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }



        private string FindNextPassword(string input)
        {
            var password = input.ToArray();

            do
            {
                password = GetNextCandidate(password);
            } while (!IsPasswordValid(password));

            var result = string.Join("", password);
            return result;
        }

        private char[] GetNextCandidate(char[] password)
        {
            var repeat = false;
            var i = password.Length - 1;
            do
            {
                repeat = false;
                password[i] = (char)(password[i] + 1);

                if (password[i] > 'z')
                {
                    password[i] = 'a';
                    repeat = true;
                    i--;
                }

            } while (repeat);
            return password;
        }

        private bool IsPasswordValid(char[] password)
        {
            var isValid = true;

            if (isValid)
                isValid &= Test_HasStraitXchars(password, 3);

            if (isValid)
                isValid &= Test_NoInvalidChars(password);

            if (isValid)
                isValid &= Test_HasXnonOverlapingPairs(password, 2);

            return isValid;
        }

        private bool Test_HasStraitXchars(char[] password, int minNumberChars)
        {
            var result = password
                .Windowed(minNumberChars)
                .Any(InAStrait);
            return result;

            bool InAStrait(char[] values)
            {
                if (values.Length < minNumberChars)
                    return false;

                var i = 1;
                while (i < values.Length)
                {
                    var a = (char)(values[i - 1] + 1);
                    var b = values[i];
                    if (a != b)
                        return false;
                    i++;
                }
                return true;
            }
        }

        private bool Test_NoInvalidChars(char[] password)
        {
            return !password.Any(i => _invalidChars.Contains(i));
        }

        private bool Test_HasXnonOverlapingPairs(char[] password, int minNumberOfPairs)
        {
            var pairCount = 0;
            var pairs = password.Windowed(2).ToArray();

            for (var i = 0; i < pairs.Length; i++)
            {
                if (pairs[i][0] == pairs[i][1])
                {
                    pairCount++;
                    i++;

                    if (pairCount >= minNumberOfPairs)
                        return true;
                }
            }

            return false;
        }
    }
}
