using System;
using System.IO;
using System.Linq;
using AdventOfCode.CSharp.Common;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AdventOfCode.CSharp.Year2015
{
    [TestFixture]
    internal class Day12 : TestBase
    {
        private const int DAY = 12;



        private (string input, int? expected) GetTestData(int part, string inputName)
        {
            var input = InputHelper.ReadText(DAY, inputName, _rootPath);

            var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}", _rootPath)
                ?.ToInt32();

            return (input, expected);
        }



        [TestCase(@"[1,2,3]", 6)]
        [TestCase(@"{""a"":2,""b"":4}", 6)]
        [TestCase(@"[[[3]]]", 3)]
        [TestCase(@"{""a"":{""b"":4},""c"":-1}", 3)]
        [TestCase(@"{""a"":[-1,1]}", 0)]
        [TestCase(@"[-1,{""a"":1}]", 0)]
        [TestCase(@"[]", 0)]
        [TestCase(@"{}", 0)]
        public void Part1TestCases(string input, int expected)
        {
            var result = SumNumbersInJson(input);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }

        [TestCase(1, "input")]
        public void Part1(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            var result = SumNumbersInJson(input);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }

        [TestCase(@"[1,2,3]", 6)]
        [TestCase(@"[1,{""c"":""red"",""b"":2},3]", 4)]
        [TestCase(@"{""d"":""red"",""e"":[1,2,3,4],""f"":5}", 0)]
        [TestCase(@"[1,""red"",5]", 6)]
        [TestCase(@"[[[3]]]", 3)]
        public void Part2TestCases(string input, int expected)
        {
            var result = SumNumbersInJsonIgnoreRed(input);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }

        [TestCase(2, "input")]
        public void Part2(int part, string inputName)
        {
            var (input, expected) = GetTestData(part, inputName);

            var result = SumNumbersInJsonIgnoreRed(input);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }



        private int SumNumbersInJson(string jsonData)
        {
            var result = 0;

            var reader = new JsonTextReader(new StringReader(jsonData));
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.Integer)
                    result += Convert.ToInt32(reader.Value);
            }

            return result;
        }

        private readonly JsonToken[] StartTokens = { JsonToken.StartObject, JsonToken.StartArray };
        private readonly JsonToken[] EndTokens = { JsonToken.EndObject, JsonToken.EndArray };
        private int SumNumbersInJsonIgnoreRed(string jsonData)
        {
            var result = 0;
            
            var reader = new JsonTextReader(new StringReader(jsonData));
            while (reader.Read())
            {
                result += SumLevel(reader, reader.TokenType == JsonToken.StartObject);
            }

            return result;
        }

        private int SumLevel(JsonTextReader reader, bool isObject)
        {
            var result = 0;
            var ignoreAll = false;

            while (reader.Read())
            {
                if (StartTokens.Contains(reader.TokenType))
                {
                    result += SumLevel(reader, reader.TokenType == JsonToken.StartObject);
                }
                else if (reader.TokenType == JsonToken.String && Convert.ToString(reader.Value) == "red")
                {
                    if (isObject)
                        ignoreAll = true;
                }
                else if (reader.TokenType == JsonToken.Integer)
                    result += Convert.ToInt32(reader.Value);
                else if (EndTokens.Contains(reader.TokenType))
                    break;
            }

            return ignoreAll ? 0 : result;
        }
    }
}
