using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common.Extensions;
using NUnit.Framework;

namespace AdventOfCode.CSharp.Year2015
{
    [TestFixture]
    internal class Day10 : TestBase
    {
        [TestCase("1", 6, 5)]
        [TestCase("1113222113", 252594, 40)]
        [TestCase("1113222113", 3579328, 50)]
        public void Part1And2(string input, int? expected, int NumProcesses)
        {
            var inputFormatted = input.Select(CharToInt).ToList();
            var result = LookAndSay(inputFormatted, NumProcesses);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }



        private int CharToInt(char value)
        {
            return value.ToString().ToInt32();
        }

        private int LookAndSay(List<int> input, int NumProcesses)
        {
            var output = new List<int>();
            while (NumProcesses > 0)
            {
                int? startVal = null;
                int count = 0;
                foreach (var value in input)
                {
                    if (startVal is null || startVal.Value == value)
                    {
                        startVal = value;
                        count++;
                        continue;
                    }

                    output.Add(count);
                    output.Add(startVal.Value);

                    startVal = value;
                    count = 1;
                }

                output.Add(count);
                output.Add(startVal.Value);

                NumProcesses--;

                input = output.ToList();
                output.Clear();
            }

            return input.Count;
        }
    }
}
