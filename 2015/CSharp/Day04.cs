using System;
using System.Security.Cryptography;
using System.Text;
using AdventOfCode.CSharp.Common;
using NUnit.Framework;

namespace AdventOfCode.CSharp.Year2015
{
    [TestFixture]
    internal class Day04 : TestBase
    {
        private const int DAY = 4;



        private (string input, int? expected) GetTestData(int part, string inputName)
        {
            var input = InputHelper.ReadText(DAY, inputName, _rootPath);

            var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}", _rootPath)
                ?.ToInt32();

            return (input, expected);
        }



        [TestCase(1, "example1")]
        [TestCase(1, "example2")]
        [TestCase(1, "input")]
        public void Part1(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            var result = FindValue(input, Has5LeadingZeros);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }

        [TestCase(2, "input")]
        public void Part2(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            var result = FindValue(input, Has6LeadingZeros);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }



        public int FindValue(string input, Func<byte[], bool> HashTest)
        {
            var result = -1;

            var md5 = MD5.Create();
            byte[] buffer;
            while (result < int.MaxValue)
            {
                result++;
                var testValue = input + result.ToString();
                buffer = Encoding.ASCII.GetBytes(testValue);
                var hash = md5.ComputeHash(buffer);

                var isValid = HashTest(hash);
                if (isValid )
                {
                    return result;
                }
            }

            throw new Exception("Value not found");
        }

        private bool Has5LeadingZeros(byte[] hash)
        {
            if (hash[0] > 0) return false;
            if (hash[1] > 0) return false;
            if (hash[2] > 15) return false;
            return true;
        }

        private bool Has6LeadingZeros(byte[] hash)
        {
            if (hash[0] > 0) return false;
            if (hash[1] > 0) return false;
            if (hash[2] > 0) return false;
            return true;
        }

    }
}
