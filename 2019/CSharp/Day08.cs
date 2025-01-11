using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Common.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.CSharp.Year2019
{
    /// <summary>
    /// https://adventofcode.com/2019/day/8
    /// </summary>
    public class Day08 : TestBase
    {
        public Day08(ITestOutputHelper output) : base(output, 8) { }

        private List<int> GetInput(string name)
        {
            var input = Input.ReadLines(DAY, name)
                .First()
                .Select(c => (int)char.GetNumericValue(c))
                .ToList();
            return input;
        }

        private int? GetPart1Expected(string name)
        {
            var expected = Input.ReadLines(DAY, $"{name}-answer1")
                ?.FirstOrDefault()
                ?.ToInt32();
            return expected;
        }

        private string GetPart2Expected(string name)
        {
            var expected = Input.ReadLines(DAY, $"{name}-answer2");
            return string.Join("\r\n", expected);
        }


        [Fact]
        public void Part1()
        {
            var input = GetInput("input");
            var expected = GetPart1Expected("input");
            var result = RunPart1(input);

            Assert.True(expected.HasValue);
            Assert.Equal(expected.Value, result);
        }

        [Fact]
        public void Part2()
        {
            var input = GetInput("input");
            var expected = GetPart2Expected("input");
            var compositeImage = BuildCompositImage(input);

            Assert.Equal(expected, compositeImage);
        }

        const int DimensionX = 25;
        const int DimensionY = 6;
        const int ImageSize = DimensionX * DimensionY;


        int RunPart1(List<int> input)
        {
            var foundLayer = GetFindLayerNumberWithLeast(input, 0);

            var layerData = GetImageLayer(input, foundLayer);
            var numOf1s = CountInLayer(layerData, 1);
            var numOf2s = CountInLayer(layerData, 2);

            var result = numOf1s * numOf2s;

            return result;
        }

        int GetFindLayerNumberWithLeast(IList<int> imageData, int value)
        {
            var layerNumber = -1;
            var lowestCount = int.MaxValue;
            var numLayers = imageData.Count / ImageSize;
            for (var i = 0; i < numLayers; i++)
            {
                var layerData = GetImageLayer(imageData, i);
                var count = CountInLayer(layerData, value);
                if (count < lowestCount)
                {
                    layerNumber = i;
                    lowestCount = count;
                }
            }

            return layerNumber;
        }

        IList<int> GetImageLayer(IList<int> imageData, int layerNumber)
        {
            var layerData = imageData
                .Skip(layerNumber * ImageSize)
                .Take(ImageSize)
                .ToList();
            return layerData;
        }
        int CountInLayer(IList<int> layerData, int value)
        {
            var result = layerData.Where(v => v == value).Count();
            return result;
        }

        string BuildCompositImage(IList<int> imageData)
        {
            var sb = new StringBuilder();
            for (var pixel = 0; pixel < ImageSize; pixel++)
            {
                var pixelChar = GetCompositedPixel(imageData, pixel);
                sb.Append(pixelChar);
                if (((pixel + 1) % DimensionX == 0) && (pixel + 1 < ImageSize))
                    sb.AppendLine();
            }
            return sb.ToString();
        }


        char GetCompositedPixel(IList<int> imageData, int pixel)
        {
            var numLayers = imageData.Count / ImageSize;
            for (var layer = 0; layer < numLayers; layer++)
            {
                var idx = (layer * ImageSize) + pixel;
                var value = imageData[idx];
                switch (value)
                {
                    case 0: return ' ';
                    case 1: return '#';
                    default: break;
                }
            }

            return ' ';
        }

    }
}
