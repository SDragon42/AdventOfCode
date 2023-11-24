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
    internal class Day08 : TestBase
    {
        private const int DAY = 8;



        private (List<string> input, int? expected) GetTestData(int part, string inputName)
        {
            var input = InputHelper.ReadLines(DAY, inputName, _rootPath)
                .ToList();

            var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}", _rootPath)
                ?.ToInt32();

            return (input, expected);
        }



        [TestCase(1, "example1")]
        [TestCase(1, "input")]
        public void Part1(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            var result = FindDifferenceInList(input, ParseString);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }

        [TestCase(2, "example1")]
        [TestCase(2, "input")]
        public void Part2(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            var result = FindDifferenceInList(input, EncodeString);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }



        private int FindDifferenceInList(List<string> codeStrings, Func<string, string> StringProcessor)
        {
            var result = 0;
            foreach (var s in codeStrings)
            {
                var newStr = StringProcessor(s);
                result += Math.Abs(s.Length - newStr.Length);
            }

            return result;
        }


        private string ParseString(string text)
        {
            var output = new List<char>();
            var i = 1;
            while (i < text.Length - 1)
            {
                var c = text[i];
                i++;

                if (c != '\\')
                {
                    output.Add(c);
                    continue;
                }

                var c2 = text[i];
                i++;
                switch (c2)
                {
                    case '\\':
                    case '"':
                        output.Add(c2);
                        break;

                    case 'x':
                        var hexCode = text.Substring(i, 2);
                        i += 2;
                        var newChar = (char)int.Parse(hexCode, System.Globalization.NumberStyles.HexNumber);
                        output.Add(newChar);
                        break;

                    default:
                        throw new Exception("ERROR");
                }
            }

            var result = string.Join("", output);
            return result.ToString();
        }

        private string EncodeString(string text)
        {
            var output = new List<char>();
            var i = 0;
            output.Add('"');
            while (i < text.Length)
            {
                var c = text[i];
                i++;

                switch (c)
                {
                    case '"':
                    case '\\':
                        output.Add('\\');
                        break;
                }
                output.Add(c);
            }
            output.Add('"');

            var result = string.Join("", output);
            return result.ToString();
        }
    }
}
